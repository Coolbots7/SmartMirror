import React, { Component } from 'react';
import './Clock.css';
var weekdays = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
const months = ["January", "February", "March", "April", "May", "June",
  "July", "August", "September", "October", "November", "December"
];

export default class Clock extends Component {
    constructor(props) {
        super(props);
        
        this.state = {date: new Date()};
    }
    
  componentDidMount() {
    this.timerID = setInterval(
      () => this.tick(),
      1000
    );
  }

  componentWillUnmount() {
    clearInterval(this.timerID);
  }

  
  tick() {
    this.setState({
      date: new Date()
    });
  }

    render() {
        return (
            <div className="row">
                <div className="col p-0">
                    <div className="row mx-auto ClockTime">
                        {this.state.date.getHours().toString().padStart(2, '0')}:{this.state.date.getMinutes().toString().padStart(2, '0')}
                    </div>
                    <div className="row mx-auto ClockDay">
                        {weekdays[this.state.date.getDay()]}
                    </div>
                    <div className="row mx-auto ClockDate">
                        {months[this.state.date.getMonth()]} {this.state.date.getDate().toString().padStart(2, '0')}
                    </div>
                </div>
            </div>
        );
    }
}
