import React from 'react'
import PropTypes from 'prop-types'
import findIndex from 'lodash/findIndex'
import size from 'lodash/size'

export default class Wizard extends React.Component {
    static propTypes = {
        children: PropTypes.any.isRequired
    }

    constructor(props) {
        super(props)

        const activePage = findIndex(React.Children.toArray(props.children), x => x && x.props.isDefaultPage)
        this.state = {
            activePage: activePage === -1 ? 0 : activePage
        }
    }

    handleNextStep = (index = 1, totalPages) => {
        this.setState(({ activePage }) => ({
            activePage: activePage + index < totalPages ? activePage + index : activePage
        }))
    }

    handlePrevStep = (index = 1) => {
        this.setState(({ activePage }) => ({
            activePage: activePage - index >= 0 ? activePage - index : activePage
        }))
    }

    render() {
        const children = React.Children.toArray(this.props.children)
        const totalPages = size(children)
        const props = {
            onNextStep: index => this.handleNextStep(index, totalPages),
            onPrevStep: this.handlePrevStep
        }

        return <div className="wizard">{React.cloneElement(children[this.state.activePage], props)}</div>
    }
}