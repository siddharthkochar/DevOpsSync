import React from 'react'
import map from 'lodash/map'

const Services = ({ services }) => (
    <div>
        <ul>
            {map(services, service => (
                <li key={service.id} 
                    className="service-tile"
                    style={{backgroundColor: service.color}}>
                    <a>
                        <div className="service-content">
                            <img src={service.imageUrl} alt="500px" title="500px" />
                            <span>{service.name}</span>
                        </div>
                    </a>
                </li>
            ))}
        </ul>
    </div>
)

export default Services