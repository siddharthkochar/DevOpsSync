import React, { useCallback, useState, useEffect } from 'react'
import {Button} from 'reactstrap'
import config from '../config'
import { toast } from 'react-toastify'

const params = `scrollbars=no,resizable=no,status=no,location=no,toolbar=no,menubar=no,
width=0,height=0,left=-1000,top=-1000`

const ServiceAuth = ({ service, auth, onNextStep }) => {
    const [startUrl, setStartUrl] = useState(null)
	const { name } = service
    const accessToken = auth.getAccessToken()
    
	useEffect(() => {
		fetch(`${config.api.devopssync}/api/${name}`, {
			headers: new Headers({
				Accept: 'application/json',
				Authorization: `Bearer ${accessToken}`
			})
		})
        .then(response => response.json())
        .then(data => {
            if (data) {
                setStartUrl(data)
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
	}, [accessToken, name, setStartUrl])

    useEffect(() =>{
        setTimeout(() => {
            onNextStep && onNextStep()
        }, 3000)   
    }, [])

    // const onClick = useCallback(() => startUrl && window.open(startUrl, name, params), [startUrl, name])

    return (
        <div
            style={{ width: '100vw', height: '80vh', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
            <Button color='primary' onClick={x => x} size='lg'>
                {'Connect'}
            </Button>
        </div>
    )
}

export default ServiceAuth