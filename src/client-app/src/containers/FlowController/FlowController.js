import React from 'react'
import { toast } from 'react-toastify'
import config from '../../config'
import Services from '../../components/Services'
import Loader from '../../components/Loader'

export default class FlowController extends React.Component {
	constructor() {
		super()

		this.state = {
			services: []
		}
	}

	componentDidMount() {
		const accessToken = this.props.auth.getAccessToken()

		fetch(`${config.api.devopssync}/services`, {
			headers: new Headers({
				Accept: 'application/json',
				Authorization: `Bearer ${accessToken}`
			})
		})
			.then(response => response.json())
			.then(data => {
				if (data) {
					this.setState({
						services: data
					})
				}
			})
			.catch(error => {
				console.log(error)
				toast.success('something went wrong!', {
					position: toast.POSITION.TOP_CENTER
				})
			})
	}

	render() {
		if (this.state.services.length === 0) {
			return <Loader />
		}

		return <Services services={this.state.services} />
	}
}
