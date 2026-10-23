import asyncio
from datetime import datetime, timedelta
import unittest
from uuid import uuid4

from httpx import ASGITransport, AsyncClient

from lms_api.domain import calculate_deadline
from lms_api.main import create_app


class ApiTests(unittest.IsolatedAsyncioTestCase):
    async def asyncSetUp(self):
        self.client = AsyncClient(transport=ASGITransport(app=create_app()), base_url="http://test")
        self.course = "75ed4c5d-113d-46a1-bf84-5322b8c82aa1"
        self.url = f"/api/courses/{self.course}/enrollments"
        self.body = {"userId": str(uuid4()), "startDate": "2026-10-23T09:30:00", "standardDays": 1}

    async def asyncTearDown(self):
        await self.client.aclose()

    async def test_catalog_contract_and_seed(self):
        response = await self.client.get("/api/courses")
        self.assertEqual(response.status_code, 200)
        courses = response.json()
        self.assertEqual([c["title"] for c in courses], ["Course Authoring Essentials", "LMS Foundations"])
        self.assertEqual(courses[1]["id"], self.course)
        self.assertEqual(set(courses[0]), {"id", "organizationId", "title", "description", "status", "lessons"})
        self.assertTrue(all(c["status"] == "Published" for c in courses))
        self.assertEqual([l["sequence"] for l in courses[0]["lessons"]], [1, 2])

    async def test_enroll_contract_deadline_and_location(self):
        response = await self.client.post(self.url, json=self.body)
        self.assertEqual(response.status_code, 201)
        data = response.json()
        self.assertEqual(set(data), {"enrollmentId", "userId", "courseId", "startDate", "deadline", "hasExtension"})
        self.assertEqual(data["userId"], self.body["userId"])
        self.assertEqual(data["deadline"], "2026-10-26T09:30:00")
        self.assertFalse(data["hasExtension"])
        self.assertEqual(response.headers["location"], f'{self.url}/{data["enrollmentId"]}')

    async def test_extension(self):
        response = await self.client.post(self.url, json={**self.body, "hasExtension": True})
        self.assertEqual(response.json()["deadline"], "2026-11-04T09:30:00")

    async def test_duplicate_is_atomic(self):
        responses = await asyncio.gather(*(self.client.post(self.url, json=self.body) for _ in range(4)))
        self.assertEqual(sorted(r.status_code for r in responses), [201, 409, 409, 409])

    async def test_different_course_and_user(self):
        self.assertEqual((await self.client.post(self.url, json=self.body)).status_code, 201)
        self.assertEqual((await self.client.post(self.url, json={**self.body, "userId": str(uuid4())})).status_code, 201)
        self.assertEqual((await self.client.post(f"/api/courses/{uuid4()}/enrollments", json=self.body)).status_code, 201)

    async def test_invalid_requests(self):
        for changes in ({"standardDays": 0}, {"standardDays": -1}, {"userId": "bad"},
                        {"userId": str(uuid4()).replace("-", "x")},
                        {"userId": "00000000-0000-0000-0000-000000000000"}, {"startDate": "bad"}):
            with self.subTest(changes=changes):
                self.assertEqual((await self.client.post(self.url, json={**self.body, **changes})).status_code, 422)
        self.assertEqual((await self.client.post(self.url, json={})).status_code, 422)
        self.assertEqual((await self.client.post("/api/courses/bad/enrollments", json=self.body)).status_code, 422)
        self.assertEqual((await self.client.post("/api/courses/00000000-0000-0000-0000-000000000000/enrollments", json=self.body)).status_code, 400)

    async def test_overflow_does_not_save_enrollment(self):
        self.assertEqual((await self.client.post(self.url, json={**self.body, "standardDays": 2147483647})).status_code, 400)
        self.assertEqual((await self.client.post(self.url, json=self.body)).status_code, 201)

    async def test_deadline_matches_original_weekday_algorithm(self):
        for weekday in range(7):
            start = datetime(2026, 10, 19, 12) + timedelta(days=weekday)
            for duration in (1, 2, 5, 7, 10, 30):
                for extension in (False, True):
                    expected = start
                    count = 0
                    while count < duration + (7 if extension else 0):
                        expected += timedelta(days=1)
                        count += expected.weekday() < 5
                    self.assertEqual(calculate_deadline(start, duration, extension), expected)


if __name__ == "__main__":
    unittest.main()
