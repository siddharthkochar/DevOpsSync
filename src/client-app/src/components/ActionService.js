import React, { useEffect, useState } from 'react'
import Services from './Services'
import config from '../config'
import {toast} from 'react-toastify'

const ActionService = ({ actionServices, serviceId, triggerId, auth, fetchActionServices, onActionServiceSelect, onNextStep }) => {
    const accessToken = auth.getAccessToken()

    console.log('ActionService', actionServices)

    useEffect(() => {
        if(actionServices.length === 0) {
            fetchActionServices(serviceId, triggerId, accessToken)   
        }
    }, [actionServices, accessToken, serviceId, triggerId])

    if (actionServices.length === 0) {
        return <div>{'Loading Action Services....'}</div>
    }

    return (
        <Services 
            services={actionServices} 
            onServiceSelect={onActionServiceSelect} 
            onNextStep={onNextStep} />
    )
}

export default ActionService