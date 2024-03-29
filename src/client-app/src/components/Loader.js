import React from 'react'
import { Spinner } from 'reactstrap'

const Loader = () => (
	<div style={{ width: '100vw', height: '80vh', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
		<Spinner type='grow' color='primary' />
		<Spinner type='grow' color='secondary' />
		<Spinner type='grow' color='success' />
		<Spinner type='grow' color='danger' />
		<Spinner type='grow' color='warning' />
		<Spinner type='grow' color='info' />
		<Spinner type='grow' color='light' />
		<Spinner type='grow' color='dark' />
	</div>
)

export default Loader
