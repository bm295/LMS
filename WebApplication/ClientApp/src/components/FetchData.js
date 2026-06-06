import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { courses: [], loading: true };
  }

  componentDidMount() {
    this.populateCourseData();
  }

  static renderCoursesTable(courses) {
    return (
      <table className='table table-striped' aria-labelledby="tableLabel">
        <thead>
          <tr>
            <th>Course</th>
            <th>Description</th>
            <th>Status</th>
            <th>Lessons</th>
          </tr>
        </thead>
        <tbody>
          {courses.map(course =>
            <tr key={course.id}>
              <td>{course.title}</td>
              <td>{course.description}</td>
              <td>{course.status}</td>
              <td>
                <ol className="mb-0 pl-3">
                  {course.lessons.map(lesson =>
                    <li key={lesson.id}>{lesson.title}</li>
                  )}
                </ol>
              </td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderCoursesTable(this.state.courses);

    return (
      <div>
        <h1 id="tableLabel" >Published courses</h1>
        <p>This page reads LMS course catalog data through the application use case layer.</p>
        {contents}
      </div>
    );
  }

  async populateCourseData() {
    const response = await fetch('api/courses');
    const data = await response.json();
    this.setState({ courses: data, loading: false });
  }
}
