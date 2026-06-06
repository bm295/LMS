import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>AI-native LMS</h1>
        <p>This application now follows a Clean Architecture structure:</p>
        <ul>
          <li><strong>Domain</strong> contains LMS entities and business rules.</li>
          <li><strong>Application</strong> exposes use cases and DTOs for workflows.</li>
          <li><strong>Infrastructure</strong> provides repository implementations and persistence adapters.</li>
          <li><strong>WebApplication</strong> hosts ASP.NET Core controllers and the React client.</li>
        </ul>
        <p>Use the course catalog page to verify the UI is reading data through the new layered backend.</p>
      </div>
    );
  }
}
