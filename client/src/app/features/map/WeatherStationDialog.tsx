import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Typography, Divider } from "@mui/material";
import {weatherStation} from "../../models/weatherStation.ts";

interface WeatherStationDialogProps {
  open: boolean;
  onClose: () => void;
  station: weatherStation | null;
}

const WeatherStationDialog: React.FC<WeatherStationDialogProps> = ({ open, onClose, station }) => {
  return (
    <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
      <DialogTitle>Weather Station Details</DialogTitle>
      <DialogContent>
        {station ? (
          <>
            <Typography variant="h6">{station.name}</Typography>
            <Typography><b>Site:</b> {station.site}</Typography>
            <Typography><b>Portfolio:</b> {station.portfolio}</Typography>
            <Typography><b>State:</b> {station.state}</Typography>
            <Divider sx={{ my: 2 }} />

            {station.variables.map((variable) => (
              <div key={variable.id}>
                <Typography variant="subtitle1">
                  <b>{variable.longName} ({variable.name})</b> [{variable.unit}]
                </Typography>
                <ul>
                  {variable.data.map((dataPoint) => (
                    <li key={dataPoint.id}>
                      <Typography>
                        <b>Time:</b> {new Date(dataPoint.timestamp).toLocaleString()} -
                        <b> Value:</b> {dataPoint.value} {variable.unit}
                      </Typography>
                    </li>
                  ))}
                </ul>
                <Divider sx={{ my: 2 }} />
              </div>
            ))}
          </>
        ) : (
          <Typography>No station selected.</Typography>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Close</Button>
      </DialogActions>
    </Dialog>
  );
};

export default WeatherStationDialog;
