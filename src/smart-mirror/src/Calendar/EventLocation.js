import React, { Component } from 'react';
import './EventLocation.css';
const axios = require('axios');

let AppID = "e5ccd03e";
let ApiKey= "8e733637ef909bd25f22f34858e9919a";

export default class EventLocation extends Component {
    constructor(props) {
        super(props);

        this.state = {
            travel_time: null,
            distance: null
        };
    }

    componentDidMount() {
        this.getTravelTime(this.props.location);
      }

    getTravelTime = async(destination) => {
        let now = new Date();

        let routeRequestData = {
            "locations": [
                {
                    "id": "Home",
                    "coords": {
                        "lat": 34.127265,
                        "lng": -118.003603
                    }
                }
            ],
            "departure_searches": [
                {
                  "id": "search",
                  "departure_location_id": "Home",
                  "arrival_location_ids": [
                    "Destination"
                  ],
                  "transportation": {
                    "type": "driving"
                  },
                  "departure_time": now.toISOString().toString(),
                  "properties": ["travel_time", "distance"]
                }
              ]
        };

        const geocoding = await axios.get(`https://api.traveltimeapp.com/v4/geocoding/search?query=${encodeURIComponent(destination)}`, {headers: {"Accept": "application/json", "X-Application-Id": AppID, "X-Api-Key": ApiKey}});

        let coords = geocoding.data.features[0].geometry.coordinates;

        routeRequestData.locations.push({"id": "Destination", "coords":{"lat": coords[1], "lng": coords[0]}});

        const route = await axios.post("https://api.traveltimeapp.com/v4/routes", routeRequestData, {headers: {"Content-Type": "application/json", "Accept": "application/json", "X-Application-Id": AppID, "X-Api-Key": ApiKey}});

        const travel_time = route.data.results[0].locations[0].properties[0].travel_time;
        const distance = route.data.results[0].locations[0].properties[0].distance;
        this.setState({
            travel_time,
            distance
        });

        
    }

    render() {
        if(this.state.travel_time) {
            let travel_time = Math.ceil(this.state.travel_time / 60);
            let leave_time = new Date(this.props.startDate);
            leave_time.setMinutes(leave_time.getMinutes() - travel_time);
            return (
                <div className="EventLocation"><i className="fas fa-car pr-1"></i> {travel_time} min away, leave at {leave_time.toLocaleTimeString()}</div>
            );
        }

        return(
            <div className="EventLocation spinner-grow spinner-grow-sm" role="status">
                <span className="sr-only">Loading...</span>
            </div>
        );
    }
}