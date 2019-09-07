import React, { Component } from 'react';
import './Weather.css';
import axios from 'axios';

let apiKey = 'b37bbdb794c3f7c244931da31c637860';
let zip = '91016,us';
let url = `http://api.openweathermap.org/data/2.5/weather?zip=${zip}&appid=${apiKey}&units=imperial`

export default class Weather extends Component {
    constructor(props) {
        super(props);
        
        this.state = {weather: null};
    }
    
  componentDidMount() {
    // this.timerID = setInterval(
    //   () => this.tick(),
    //   5000
    // );

    this.tick();
  }

  componentWillUnmount() {
    clearInterval(this.timerID);
  }

  
  tick = async()  => {
    const res = await axios.get(url);
    const weather = JSON.stringify(await res.data);

    this.setState({
      weather
    })
  }

    render() {
      if(this.state.weather) {
        let weather = JSON.parse(this.state.weather);

        let sunrise = new Date(weather.sys.sunrise*1000);
        let sunset = new Date(weather.sys.sunset*1000);

        return (
            <div className="row">
                <div className="col p-0">
                    {/** 24H Temperature Graph */}                    
                    <div className="row">
                    </div>
                    {/** Current Weather */}
                    <div className="row">
                        {/** Percipitation */}
                        <div className="col-auto WeatherColumn">
                          <i className="fas fa-2x fa-umbrella WeatherIcon"></i>
                        </div>
                        {/** Wind */}
                        <div className="col-auto WeatherColumn">
                          <i className="fas fa-2x fa-wind WeatherIcon"></i>
                          <div className="WeatherText">{weather.wind.speed} MPH {weather.wind.deg}&#176;</div>
                        </div>
                        {/** Humidity */}
                        <div className="col-auto WeatherColumn">
                          <i className="fas fa-2x fa-cloud-meatball WeatherIcon"></i>
                          <div className="WeatherText">{weather.main.humidity} %</div>                        
                        </div>
                        {/** Sunrise */}
                        <div className="col-auto WeatherColumn">
                          <i className="fas fa-2x fa-sun WeatherIcon"></i>
                        <div className="WeatherText">{sunrise.toLocaleTimeString()}</div> 
                        </div>
                        {/** Sunset */}
                        <div className="col-auto WeatherColumn">
                          <i className="fas fa-2x fa-moon WeatherIcon"></i>
                        <div className="WeatherText">{sunset.toLocaleTimeString()}</div> 
                        </div>
                        {/** Sky */}
                        <div className="col-auto WeatherColumn">
                          <i className="fas fa-2x fa-cloud-sun WeatherIcon"></i>
                          <div className="WeatherText">{weather.weather[0].description}</div> 
                        </div>
                    </div>
                </div>
            </div>
        );
      }
      
      return (
        <span></span>
      );
    }
}
