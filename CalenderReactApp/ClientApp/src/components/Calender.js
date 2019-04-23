import React, { Component } from 'react';
import Calendar from 'react-calendar';

class Calender extends Component {
    constructor(props) {
        super(props)
        this.state = {
            date: new Date(),
        }
    }
    onChange = date => this.setState({ date })
    
    render() {
        return (
            <div>
                <Calendar
                    onChange={this.onChange}
                    value={this.state.date}
                />
            </div>
        );
    }
}

export default Calender;