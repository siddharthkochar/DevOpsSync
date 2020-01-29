import React, { Component } from 'react'
import logo from './logo.svg';
import './App.css';
import AuthService from './AuthService'
import Home from './Home'
import { Switch, Route } from 'react-router-dom'

class App extends Component {
  constructor() {
    super()
    this.authService = new AuthService()
  }

  renderHome() {
    let resultComponent = <Home auth={this.authService} />

    if (!this.authService.isAuthenticated()) {
      this.authService.login();

      resultComponent = (
        <div>
          <p className="App-intro">
            To get started, edit <code>src/App.js</code> and save to reload.
          </p>
        </div>
      )
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
        <header className="App-header">
          <h1 className="App-title">Welcome to React</h1>
          {logoutButton}
        </header>
        <img src={logo} className="App-log" alt="logo" />

        <Switch>
          <Route exact path="/" render={() => this.renderHome()} />
          <Route path="/callback" render={({ history }) => this.startSession(history)} />
        </Switch>
      </div>
    )
  }
}

export default App;
