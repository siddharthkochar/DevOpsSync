import React, { Component } from 'react'
import logo from './logo.svg';
import './App.css';
import AuthService from './AuthService'
import FlowController from './containers/FlowController'
import { Switch, Route } from 'react-router-dom'

class App extends Component {
  constructor() {
    super()
    this.authService = new AuthService()
  }

  renderHome() {
    let resultComponent = <FlowController auth={this.authService}/>

    if (!this.authService.isAuthenticated()) {
      this.authService.login();
    }

    return resultComponent
  }

  startSession(history) {
    this.authService.handleAuthentication(history);
    return <div><p>Starting session...</p></div>;
  }

  createLogoutButton() {
    let button = null

    if(this.authService.isAuthenticated) {
      button = <button onClick={this.authService.logout}>Logout</button>
    }
    return button
  }

  render() {
    var logoutButton = this.createLogoutButton()

    return (
      <div className="App">
        <div>
          {logoutButton}
        </div>
        <Switch>
          <Route exact path="/" render={() => this.renderHome()} />
          <Route path="/callback" render={({ history }) => this.startSession(history)} />
        </Switch>
      </div>
    )
  }
}

export default App;
