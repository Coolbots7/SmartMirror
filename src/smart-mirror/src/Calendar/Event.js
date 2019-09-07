import React, { Component } from 'react';
import './Event.css';


export default class Event extends Component {    
    render() {
        let event = JSON.parse(this.props.event);
        let start = event.start.hasOwnProperty("date") ? event.start.date : event.start.dateTime;
        return(
            <div className="row d-flex justify-content-between CalendarEvent">
                <span className="col-auto EventTitle">{event.summary}</span>
                <span className="col-auto EventDuration">{start}</span>
            </div>
        );
    }
}
