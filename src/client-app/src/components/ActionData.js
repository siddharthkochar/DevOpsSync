import React from 'react'
import filter from 'lodash/filter'
import first from 'lodash/first'

const ActionData = ({ actionId, onSave, onNextStep }) => {

    const Component = first(filter(actionDatePageMapping, m => m.actionId === actionId))

    if(!Component) {
        return null;
    }

    return (
        <Component.component onSave={(data) => { onSave(data); onNextStep && onNextStep() }} />
    )
}

const actionDatePageMapping = []

export default ActionData