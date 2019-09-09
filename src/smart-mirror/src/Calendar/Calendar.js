import React, { Component } from 'react';
import ApiCalendar from 'react-google-calendar-api';
import './Calendar.css';
import GoogleSignIn from './GoogleSignIn'
import Event from './Event'


export default class Calendar extends Component {
    constructor(props) {
        super(props);

        this.state = {events: null}

        this.signUpdate = this.signUpdate.bind(this);
        ApiCalendar.onLoad(() => {
          this.signUpdate(ApiCalendar.sign);
          ApiCalendar.listenSign(this.signUpdate);
        });
    }

    signUpdate(sign) {
        this.setState({
            sign
        });
        this.getEvents();
    }
    
    componentDidMount() {
      this.timerID = setInterval(
        () => this.getEvents(),
        60000
      );
    }

    componentWillUnmount() {
      clearInterval(this.timerID);
    }

    getEvents() {
      if(this.state.sign) {
        ApiCalendar.listUpcomingEvents(this.props.count || 5)
        .then(({result}) => {
          this.setState({
            events: result.items
          });
        });   
      }   
    }
    
    render() {
      
      if(this.state.sign) {
        if (this.state.events) {
          let events = this.state.events.filter(function(event) {
            let start = event.start.hasOwnProperty("date") ? event.start.date : event.start.dateTime;
            let startDate = new Date(start);
    
            let now = new Date();
            now.setDate(now.getDate() + 7);
            return startDate <= now;
          });

          if(events.length > 0) {
          return(
            <div className="d-flex flex-column">
              {events.map(event => (
                <Event key={event.id} event={JSON.stringify(event)} />
              ))}
          </div>
          );
          }
          else {
            return (
              <div>
                No Events
              </div>
            )
          }
        }

        return(
          <div></div>
        );
      }

      return (
        <GoogleSignIn />
      );
    }
}
