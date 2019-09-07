import React from 'react';
import './App.css';
import Clock from "./Clock/Clock"
import Weather from "./Weather/Weather"

function App() {
  return (
    <div className="App">
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
