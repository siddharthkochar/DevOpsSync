import React, { useState, useEffect } from 'react'
import map from 'lodash/map'
import config from '../config'
import { toast } from 'react-toastify'
import { Link } from 'react-router-dom'
import { Container, CardDeck, Card, CardImg, CardText, CardBody } from 'reactstrap'

const Trigger = ({ service, auth, onTriggerSelect, onNextStep }) => {

    const [triggers, setTriggers] = useState([])
    const accessToken = auth.getAccessToken()

    useEffect(() => {
        fetch(`${config.api.devopssync}/services/${service.id}/triggers`, {
            headers: new Headers({
                Accept: 'application/json',
                Authorization: `Bearer ${accessToken}`
            })
        })
            .then(response => response.json())
            .then(data => {
                if (data) {
                    setTriggers(data)
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
    }, [accessToken, service, setTriggers])

    if (triggers.length === 0) {
        return <div>Loading......</div>
    }

    return (
        <div>
            <Container style={{ marginTop: 30 }}>
                <CardDeck>
                    {map(triggers, trigger => (
                        <Card
                            inverse
                            key={trigger.id}
                            outline
                            style={{
                                backgroundColor: service.color,
                                borderColor: service.color,
                                textAlign: 'center',
                                maxWidth: '25%',
                                padding: 20,
                                cursor: 'pointer'
                            }}>
                            <Link onClick={() => { onTriggerSelect(trigger.id); onNextStep && onNextStep() }}>
                                <CardBody>
                                    <CardText style={{ fontSize: '1.5rem', fontWeight: 'bold' }}>
                                        {trigger.name}
                                        <br />
                                        {trigger.description}
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

export default Trigger