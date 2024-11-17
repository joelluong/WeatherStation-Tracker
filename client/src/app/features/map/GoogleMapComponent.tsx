import React, { useState, useEffect } from 'react';
import { GoogleMap, Marker, InfoWindow, useLoadScript } from '@react-google-maps/api';
import { Button } from "@mui/material";
import WeatherStationDialog from './WeatherStationDialog';
import {weatherStation} from "../../models/weatherStation.ts"; // Import Dialog component

const mapContainerStyle = {
  width: '100%',
  height: '100vh',
};

const center = {
  lat: -25.2744, // Center of Australia
  lng: 133.7751,
};

const GoogleMapComponent: React.FC = () => {
  const { isLoaded, loadError } = useLoadScript({
    googleMapsApiKey: import.meta.env.VITE_GOOGLE_MAPS_API_KEY, // Access API Key
  });

  const [weatherStations, setWeatherStations] = useState<weatherStation[]>([]);
  const [selectedStation, setSelectedStation] = useState<weatherStation | null>(null);
  const [modalOpen, setModalOpen] = useState(false); // State for modal visibility

  useEffect(() => {
    const fetchWeatherStations = async () => {
      try {
        const response = await fetch('http://localhost:5000/api/weather-stations');
        const data: weatherStation[] = await response.json();
        setWeatherStations(data);
      } catch (error) {
        console.error('Error fetching weather stations:', error);
      }
    };

    fetchWeatherStations();
  }, []);

  if (loadError) return <div>Error loading map</div>;
  if (!isLoaded) return <div>Loading map...</div>;

  const handleModalOpen = () => {
    setModalOpen(true);
  };

  const handleModalClose = () => {
    setModalOpen(false);
  };

  return (
    <>
      <GoogleMap
        mapContainerStyle={mapContainerStyle}
        zoom={5.5}
        center={center}
      >
        {weatherStations.map((station) => (
          <Marker
            key={station.id}
            position={{
              lat: station.latitude,
              lng: station.longitude,
            }}
            onClick={() => setSelectedStation(station)}
          />
        ))}

        {selectedStation && (
          <InfoWindow
            position={{
              lat: selectedStation.latitude,
              lng: selectedStation.longitude,
            }}
            onCloseClick={() => setSelectedStation(null)}
          >
            <div>
              <h2>{selectedStation.name}</h2>
              <p><b>Site:</b> {selectedStation.site}</p>
              <p><b>Portfolio:</b> {selectedStation.portfolio}</p>
              <p><b>State:</b> {selectedStation.state}</p>
              <Button onClick={handleModalOpen}>More details</Button>
            </div>
          </InfoWindow>
        )}
      </GoogleMap>

      <WeatherStationDialog
        open={modalOpen}
        onClose={handleModalClose}
        station={selectedStation}
      />
    </>
  );
};

export default GoogleMapComponent;
