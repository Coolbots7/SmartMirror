import React from 'react';
import './App.css';
import Clock from "./Clock/Clock"
import Weather from "./Weather/Weather"
import Calendar from "./Calendar/Calendar"

function App() {
  return (
    <div className="App">
      <div className="Calendar">
        <Calendar />
      </div>
      <div className="Weather">
        <Weather />
      </div>
      <div className="Clock">
        <Clock />
      </div>
    </div>
  );
}

export default App;
