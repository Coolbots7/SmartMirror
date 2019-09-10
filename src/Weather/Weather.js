import React, { Component } from 'react';
import './Weather.css';
import axios from 'axios';
import Icon from '@mdi/react'
import {mdiUmbrella, mdiWeatherWindy, mdiWeatherSunny } from '@mdi/js'

let apiKey = 'b37bbdb794c3f7c244931da31c637860';
let zip = '91016,us';
let url = `http://api.openweathermap.org/data/2.5/weather?zip=${zip}&appid=${apiKey}&units=imperial`

export default class Weather extends Component {
    constructor(props) {
        super(props);
        
        this.state = {weather: null};
    }
    
  componentDidMount() {
    this.tick();
    this.timerID = setInterval(
      () => this.tick(),
      10*60*1000
    );
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

        let precipitation = weather.hasOwnProperty("precipitation") ? weather.precipitation.value : 0;

        return (
            <div className="row">
                <div className="col p-0">
                    {/** 24H Temperature Graph */}                    
                    <div className="row">
                      <div className="CurrentWeather">
                        {Math.floor(weather.main.temp).toString()}&#176; <Icon path={mdiWeatherSunny} size='6rem' color='#FFFFFF'/>
                      </div>
                    </div>
                    <div className="row">
                      <div className="WeatherDescription text-capitalize">
                        {weather.weather[0].description}
                      </div>
                    </div>
                    <div className="row">
                        {/** Precipitation */}
                        <div className="col-auto WeatherRow">
                          <div className="WeatherText">{precipitation.toString()}% <Icon path={mdiUmbrella} size='1.8rem' color='#FFFFFF'/></div>
                        </div>                        
                    </div>
                    <div className="row">
                        {/** Wind */}
                        <div className="col-auto WeatherRow">                          
                          <div className="WeatherText">{Math.ceil(weather.wind.speed).toString()} MPH <Icon path={mdiWeatherWindy} size='1.8rem' color='#FFFFFF'/></div>
                        </div>                    
                    </div>
                    {/* <div className="row">
                        <div className="col-auto WeatherColumn">
                          <i className="fas fa-2x fa-cloud-meatball WeatherIcon"></i>
                          <div className="WeatherText">{weather.main.humidity} %</div>                        
                        </div>                    
                    </div>
                    <div className="row">
                        <div className="col-auto WeatherColumn">
                          <i className="fas fa-2x fa-sun WeatherIcon"></i>
                        <div className="WeatherText">{sunrise.toLocaleTimeString()}</div> 
                        </div>                    
                    </div>
                    <div className="row">
                        <div className="col-auto WeatherColumn">
                          <i className="fas fa-2x fa-moon WeatherIcon"></i>
                        <div className="WeatherText">{sunset.toLocaleTimeString()}</div> 
                        </div>                    
                    </div> */}
                </div>
            </div>
        );
      }
      
      return (
        <span></span>
      );
    }
}
