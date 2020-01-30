import React, { useEffect, useState, useCallback } from 'react'
import { useRouteMatch } from 'react-router-dom'
import { Button } from 'reactstrap'
import { toast } from 'react-toastify'
import config from '../../config'

const params = `scrollbars=no,resizable=no,status=no,location=no,toolbar=no,menubar=no,
width=0,height=0,left=-1000,top=-1000`

const ServiceController = props => {
	const match = useRouteMatch()
	console.log(match)
	const [startUrl, setStartUrl] = useState(null)
	const { name } = match.params
	const accessToken = props.auth.getAccessToken()
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
	}, [accessToken, name])

	const onClick = useCallback(() => startUrl && window.open(startUrl, name, params), [startUrl, name])
	return (
		<div
			style={{ width: '100vw', height: '80vh', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
			<Button color='primary' onClick={onClick} size='lg'>
				{'Connect'}
			</Button>
		</div>
	)
}

export default ServiceController
