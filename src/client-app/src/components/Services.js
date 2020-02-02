import React from 'react'
import map from 'lodash/map'
import { Link } from 'react-router-dom'
import { Container, CardDeck, Card, CardImg, CardText, CardBody } from 'reactstrap'

const Services = ({ services, onServiceSelect, onNextStep }) => (
	<Container style={{ marginTop: 30 }}>
		<CardDeck>
			{map(services, service => (
				<Card
					inverse
					key={service.id}
					outline
					style={{
						backgroundColor: service.color,
						borderColor: service.color,
						textAlign: 'center',
						maxWidth: '25%',
						padding: 20,
						cursor: 'pointer'
					}}>
					<Link onClick={() => {onServiceSelect(service.id); onNextStep && onNextStep()}}>
						<CardImg
							top
							style={{ height: 150, margin: 'auto', width: 'auto' }}
							src={service.imageUrl || require('../logo.svg')}
							alt='Card image cap'
						/>
						<CardBody>
							<CardText style={{ fontSize: '1.5rem', fontWeight: 'bold' }}>{service.name}</CardText>
						</CardBody>
					</Link>
				</Card>
			))}
		</CardDeck>
	</Container>
)

export default Services
