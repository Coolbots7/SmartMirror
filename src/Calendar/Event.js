import React, { Component } from 'react';
import './Event.css';
import EventLocation from './EventLocation'
import Icon from '@mdi/react'
import { mdiCalendar } from '@mdi/js'

export default class Event extends Component {    

    isToday(date) {
        let today = new Date();
        return (today.getFullYear() === date.getFullYear() && today.getMonth() === date.getMonth() && today.getDate() === date.getDate());
    }

    getTimeString(startDate) {
        let now = new Date();
        let tomorrow = new Date(now);
        tomorrow.setDate(tomorrow.getDate() + 1);

        //Today
        if(this.isToday(startDate)) {
            return startDate.toLocaleTimeString();
        }

        //Tomorrow
        else if(tomorrow.getFullYear() === startDate.getFullYear() && tomorrow.getMonth() === startDate.getMonth() && tomorrow.getDate() === startDate.getDate()) {
            return("Tomorrow");
        }
        //More
        //if(Math.floor((startDate - now) / 86400000) > 1) {
        else {
            return(`In ${Math.floor((startDate - now) / 86400000)} day${Math.floor((startDate - now) / 86400000) > 1 ? 's' : ''}`);
        }
    }

    render() {
        let event = JSON.parse(this.props.event);
        let start = event.start.hasOwnProperty("date") ? event.start.date : event.start.dateTime;
        let startDate = new Date(start);

        return(
            <div className="CalendarEvent">
                <div className="d-inline-flex justify-content-start EventDetails">
                    <div className="mr-2"><Icon path={mdiCalendar} size='1.8rem' color='#FFFFFF'/></div>
                    <div className="EventTime mr-1">{startDate.getHours().toString().padStart(2, '0')}:{startDate.getMinutes().toString().padStart(2, '0')}</div>
                    <div className="text-left EventTitle">{event.summary}</div>                
                </div>
                {(event.hasOwnProperty('location') && this.isToday(startDate)) &&
                    <EventLocation location={event.location} startDate={startDate}/>
                }
            </div>            
        );
    }
}
