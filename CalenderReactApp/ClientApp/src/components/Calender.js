import React, { Component } from 'react';

class Calender extends Component {
    constructor(props) {
        super(props)
        this.state = {
            date: new Date(),
        }
    }

    render() {
        return (
            <div>
                <BigCalendar
                    localizer={localizer}
                    events={myEventsList}
                    startAccessor="start"
                    endAccessor="end"
                />
            </div>
        );
    }
}

export default Calender;