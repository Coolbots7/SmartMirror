import React, { Component } from 'react';
import './Clock.css';


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
                    <div className="row mx-auto">
                        <h1>{this.state.date.toLocaleTimeString()}</h1>
                    </div>
                    <div className="row mx-auto">
                        <h2>{this.state.date.toDateString()}</h2>
                    </div>
                </div>
            </div>
        );
    }
}
