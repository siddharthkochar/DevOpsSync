import React from 'react'
import config from '../../config'
import Services from '../../components/Services'
import Wizard from '../../components/Wizard'

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
                "Accept": "application/json",
                "Authorization": `Bearer ${accessToken}`
            })
        })
        .then(response => response.json())
        .then(data => {
            if(data) {
                this.setState({
                    services: data
                })
            }
        })
        .catch(error => {
            console.log(error)
            alert('something went wrong!')
        })
    }


    render() {
        if (this.state.services.length === 0) {
            return "loading...."
        }

        return (
            <Wizard>
                <Services services={this.state.services} />
            </Wizard>
        )
    }
}