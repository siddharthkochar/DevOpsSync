import React, { Component } from 'react'
import { Collapse, Navbar, NavbarBrand, NavbarText, Button } from 'reactstrap'
import { ToastContainer } from 'react-toastify'
import './App.css'
import AuthService from './AuthService'
import FlowController from './containers/FlowController'
import { Switch, Route } from 'react-router-dom'
import Loader from './components/Loader'
import ServiceController from './containers/ServiceController'
import 'react-toastify/dist/ReactToastify.css';

class App extends Component {
	constructor() {
		super()
		this.authService = new AuthService()
	}

	renderHome() {
		let resultComponent = <FlowController auth={this.authService} />

		if (!this.authService.isAuthenticated()) {
			this.authService.login()
		}

		return resultComponent
	}

	renderService(props) {
		let resultComponent = <ServiceController {...props} auth={this.authService} />

		if (!this.authService.isAuthenticated()) {
			this.authService.login()
		}

		return resultComponent
	}

	startSession(history) {
		this.authService.handleAuthentication(history)
		return <Loader />
	}

	render() {
		return (
			<div className='App'>
				<div>
					<Navbar color='dark' light expand='md'>
						<NavbarBrand href='/' style={{ color: '#fff' }}>
							{'DevOpsSync'}
						</NavbarBrand>
						<Collapse navbar></Collapse>
						<NavbarText>
							{this.authService.isAuthenticated && (
								<Button outline color='primary' onClick={this.authService.logout}>
									{'LogOut'}
								</Button>
							)}
						</NavbarText>
					</Navbar>
				</div>
				<Switch>
					<Route exact path='/' render={() => this.renderHome()} />
					<Route exact path='/service/:name' render={props => this.renderService(props)} />
					<Route path='/callback' render={({ history }) => this.startSession(history)} />
				</Switch>
				<ToastContainer />
			</div>
		)
	}
}

export default App
