import React, { useState, useEffect } from 'react';
import ReactMapGL, { Marker, Popup, ViewState } from 'react-map-gl';

interface WeatherStation {
  id: number;
  name: string;
  site: string;
  portfolio: string;
  state: string;
  latitude: number;
  longitude: number;
}

const MapboxComponent: React.FC = () => {
  const [viewport, setViewport] = useState<ViewState>({
    latitude: -25.2744, // Center of Australia
    longitude: 133.7751,
    zoom: 7,
    bearing: 0,
    pitch: 0,
    padding: { top: 0, bottom: 0, left: 0, right: 0 },
  });

  const [weatherStations, setWeatherStations] = useState<WeatherStation[]>([]);
  const [selectedStation, setSelectedStation] = useState<WeatherStation | null>(null);

  useEffect(() => {
    const fetchWeatherStations = async () => {
      try {
        const response = await fetch('http://localhost:5000/api/weather-stations');
        const data: WeatherStation[] = await response.json();
        setWeatherStations(data);
        console.log(data);
      } catch (error) {
        console.error('Error fetching weather stations:', error);
      }
    };

    fetchWeatherStations();
  }, []);

  return (
    <ReactMapGL
      {...viewport}
      mapboxAccessToken="pk.eyJ1Ijoiam9lbGx1b25nIiwiYSI6ImNtM2hjeXYwazBkemUybHBvYTVpZTJ5YXcifQ.c6uZlAgaIKITSOqRWQRCbw"
      mapStyle="mapbox://styles/mapbox/streets-v11"
      style={{ width: '100%', height: '100vh' }}
      onMove={(evt) => setViewport(evt.viewState)}
    >
      {weatherStations.map((station) => (
        <Marker
          key={station.id}
          latitude={station.latitude}
          longitude={station.longitude}
        >
          <button
            className="marker-btn"
            onClick={(e) => {
              e.preventDefault();
              setSelectedStation(station); // Correctly sets the selected station for popups
            }}
          >
            <img src="/marker-icon.png" alt="Marker Icon" />
          </button>
        </Marker>
      ))}

      {selectedStation && (
        <Popup
          latitude={selectedStation.latitude}
          longitude={selectedStation.longitude}
          closeOnClick={false} // Keep popup open when clicking inside it
          closeButton={false}  // Remove default close button
          onClose={() => setSelectedStation(null)}
        >
          <div>
            <button
              className="custom-close-btn"
              onClick={() => setSelectedStation(null)}
              aria-label="Close popup"
            >
              &times; {/* Custom Close Icon */}
            </button>
            <h2>{selectedStation.name}</h2>
            <p>{selectedStation.site}</p>
            <p>{selectedStation.portfolio}</p>
            <p>{selectedStation.state}</p>
          </div>
        </Popup>
      )}
    </ReactMapGL>
  );
};

export default MapboxComponent;
