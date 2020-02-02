import React, { useState, useEffect } from 'react'
import map from 'lodash/map'
import config from '../config'
import { toast } from 'react-toastify'
import { Link } from 'react-router-dom'
import { Container, CardDeck, Card, CardText, CardBody } from 'reactstrap'

const Actions = ({ serviceId, triggerId, actionService, auth, onActionSelect, onNextStep }) => {

    const [actions, setActions] = useState([])
    const accessToken = auth.getAccessToken()

    useEffect(() => {
        fetch(`${config.api.devopssync}/services/${serviceId}/triggers/${triggerId}/services/${actionService.id}/actions`, {
            headers: new Headers({
                Accept: 'application/json',
                Authorization: `Bearer ${accessToken}`
            })
        })
            .then(response => response.json())
            .then(data => {
                if (data) {
                    setActions(data)
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
    }, [accessToken, serviceId, setActions])

    if (actions.length === 0) {
        return <div>Loading......</div>
    }

    return (
        <div>
            <Container style={{ marginTop: 30 }}>
                <CardDeck>
                    {map(actions, action => (
                        <Card
                            inverse
                            key={action.id}
                            outline
                            style={{
                                backgroundColor: actionService.color,
                                borderColor: actionService.color,
                                textAlign: 'center',
                                maxWidth: '25%',
                                padding: 20,
                                cursor: 'pointer'
                            }}>
                            <Link onClick={() => { onActionSelect(action.id); onNextStep && onNextStep() }}>
                                <CardBody>
                                    <CardText style={{ fontSize: '1.5rem', fontWeight: 'bold' }}>
                                        {action.name}
                                        <br />
                                        {action.description}
                                    </CardText>
                                </CardBody>
                            </Link>
                        </Card>
                    ))}
                </CardDeck>
            </Container>
        </div>
    )
}

export default Actions