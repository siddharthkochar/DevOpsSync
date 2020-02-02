import React from 'react'
import filter from 'lodash/filter'
import { useForm } from 'react-hook-form'
import first from 'lodash/first'

const TriggerData = ({ triggerId, onSave, onNextStep }) => {

    console.log('triggerData', triggerId)
    const Component = first(filter(triggerDatePageMapping, m => m.triggerId === triggerId))

    if(!Component) {
        return null;
    }

    console.log('Component', Component, triggerId)

    return (
        <Component.component onSave={(data) => { console.log(data);  onSave(data); onNextStep && onNextStep() }} />
    )
}

const GithubPushTrigger = ({ onSave }) => {
    const { register, handleSubmit, errors } = useForm()
    return (
        <form onSubmit={handleSubmit(onSave)}>
            <label>{'Repository Name'}</label>
            <input type="text" name="repositoryName" ref={register({ required: true })} />
            {errors.repositoryName && <p>{'This field is required !'}</p>}
            <button type="submit">{'Submit'}</button>
        </form>
    )
}

const triggerDatePageMapping = [{
    triggerId: 1,
    component: GithubPushTrigger
}]

export default TriggerData