from datetime import datetime
from uuid import UUID

from pydantic import BaseModel, ConfigDict, Field, field_validator
from pydantic.alias_generators import to_camel


class Contract(BaseModel):
    model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)


class LessonSummary(Contract):
    id: UUID
    title: str
    sequence: int


class CourseSummary(Contract):
    id: UUID
    organization_id: UUID
    title: str
    description: str
    status: str
    lessons: list[LessonSummary]


class EnrollRequest(Contract):
    user_id: UUID
    start_date: datetime
    standard_days: int = Field(gt=0, le=2147483647)
    has_extension: bool = False

    @field_validator("user_id")
    @classmethod
    def nonempty_user(cls, value: UUID) -> UUID:
        if value.int == 0:
            raise ValueError("User id must be provided.")
        return value


class EnrollResponse(Contract):
    enrollment_id: UUID
    user_id: UUID
    course_id: UUID
    start_date: datetime
    deadline: datetime
    has_extension: bool
