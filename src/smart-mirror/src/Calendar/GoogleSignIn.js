import React, { Component } from 'react';
import ApiCalendar from 'react-google-calendar-api';
import './GoogleSignIn.css';


export default class GoogleSignIn extends Component {
      
    handleItemClick(event, name) {
        if (name === 'sign-in') {
          ApiCalendar.handleAuthClick();
        } else if (name === 'sign-out') {
          ApiCalendar.handleSignoutClick();
        }
      }

    render() {
        return (
            <button
                  onClick={(e) => this.handleItemClick(e, 'sign-in')}
              >
                sign-in
              </button>
        );
    }
}
