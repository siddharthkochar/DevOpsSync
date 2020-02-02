import React from 'react'
import first from 'lodash/first'
import filter from 'lodash/filter'
import { toast } from 'react-toastify'
import config from '../../config'
import Services from '../../components/Services'
import Loader from '../../components/Loader'
import Wizard from '../../components/Wizard'
import ServiveAuth from '../../components/ServiceAuth'
import Triggers from '../../components/Triggers'
import TriggerData from '../../components/TriggerData'
import ActionService from '../../components/ActionService'
import Actions from '../../components/Actions'
import ActionData from '../../components/ActionData'

export default class FlowController extends React.Component {
	constructor() {
		super()

		this.state = {
			services: [],
			actionServices: [],
			selectedService: {},
			selectedTriggerId: null,
			selectedTriggerData: {},
			selectedActionService: {},
			selectedActionId: null,
			selectedActionData: {}
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

	onServiceSelect = (serviceId) => {
		this.setState(prevState => ({
			selectedService: first(filter(prevState.services, s => s.id === serviceId)),
			actionServices: [],
			selectedTriggerId: null,
			selectedTriggerData: {},
			selectedActionService: {},
			selectedActionId: null,
			selectedActionData: {}
		}))
	}

	onTriggerSelect = (triggerId) => {
		this.setState(prevState => ({
			selectedTriggerId: triggerId,
			selectedTriggerData: {},
			actionServices: [],
			selectedActionService: {},
			selectedActionId: null,
			selectedActionData: {}
		}))
	}

	onActionServiceSelect = (actionServiceId) => {
		this.setState(prevState => ({
			selectedActionService: first(filter(prevState.actionServices, s => s.id === actionServiceId)),
			selectedActionId: null,
			selectedActionData: {}
		}))
	}

	onActionSelect = (actionId) => {
		this.setState({
			selectedActionId: actionId,
			selectedActionData: {}
		})
	}

	setActionServices = (services) => {
		this.setState({
			actionServices: services
		})
	}

	fetchActionServices = (serviceId, triggerId, accessToken) => {
		fetch(`${config.api.devopssync}/services/${serviceId}/triggers/${triggerId}/services`, {
            headers: new Headers({
                Accept: 'application/json',
                Authorization: `Bearer ${accessToken}`
            })
        })
		.then(response => response.json())
		.then(data => {
			if (data) {
				this.setActionServices(data)
			} else {
				toast.success('something went wrong!', {
					position: toast.POSITION.TOP_CENTER
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

	saveTriggerData = (data) => {
		this.setState({
			selectedTriggerData: data
		})
	}

	render() {
		if (this.state.services.length === 0) {
			return <Loader />
		}

		{console.log(this.state.actionServices)}
		return (
			<Wizard>
				<Services services={this.state.services} onServiceSelect={this.onServiceSelect}/>
				
				<ServiveAuth service={this.state.selectedService} auth={this.props.auth}/>
				
				<Triggers 
					service={this.state.selectedService}
					auth={this.props.auth}
					onTriggerSelect={this.onTriggerSelect}/>
				
				<TriggerData
					triggerId={this.state.selectedTriggerId}
					auth={this.props.auth}
					onSave={this.saveTriggerData}/>

				<ActionService
					serviceId={this.state.selectedService?.id}
					triggerId={this.state.selectedTriggerId}
					actionServices={this.state.actionServices}
					fetchActionServices={this.fetchActionServices}
					auth={this.props.auth}
					onActionServiceSelect={this.onActionServiceSelect} />

				<Actions
					serviceId={this.state.selectedService?.id}
					triggerId={this.state.selectedTriggerId}
					actionService={this.state.selectedActionService}
					auth={this.props.auth}
					onActionSelect={this.onActionSelect} />

				<ActionData
					actionId={this.state.selectedActionId}
					auth={this.props.auth}
					onSave={this.saveActionData}/>
			</Wizard>
		)
	}
}
