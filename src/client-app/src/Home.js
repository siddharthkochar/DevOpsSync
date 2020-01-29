import React from 'react';
import './Home.css'
import config from './config';

class Home extends React.Component {
    constructor() {
        super();
        this.state = { bookList: [] };
    }

    componentDidMount() {
        const accessToken = this.props.auth.getAccessToken()
        console.log('accessToken', accessToken)
        fetch(`${config.api.devopssync}/values`, {
            headers: new Headers({
                "Accept": "application/json",
                "Authorization": `Bearer ${accessToken}`
            })
        })
        .then(response => response.json())
        .then(books => this.setState({ bookList: books }))
        .catch(error => console.log(error))
    }

    render() {
        let bookList = this.state.bookList.map((book) =>
            <li><i>{book}</i></li>);

        return <ul>
            {bookList}
        </ul>;
    }
}

export default Home;