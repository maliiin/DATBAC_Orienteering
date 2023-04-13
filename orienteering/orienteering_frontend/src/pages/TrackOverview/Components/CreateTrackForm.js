import { React, useState } from "react";
import { Button, TextField } from '@mui/material';
import { useNavigate } from 'react-router-dom';

export default function CreateTrackForm(props) {
    const [trackInfo, setTrackInfo] = useState({
        TrackName: ""
    });

    const navigate = useNavigate();

    const handleSubmit = async (event) => {
        event.preventDefault();

        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            },
            body: JSON.stringify(trackInfo)
        };

        const response = await fetch('/api/track/createTrack', requestOptions);
        if (!response.ok) {
            navigate("login");
        }

        props.updateTracks();

        //clear form
        setTrackInfo({
            TrackName: ""
        });

        return response;
    };

    const handleChange = (event) => {
        //update state
        setTrackInfo({ TrackName: event.target.value });
    };

    return (<>
        <h4>Add new track</h4>
        <form onSubmit={handleSubmit}>

            <TextField
                required
                onChange={(e) => handleChange(e)}
                id="standard-basic" label="Title"
                name="TrackName"
                variant="standard"
                value={trackInfo.TrackName}
            />
            <br></br>
            <Button type="submit">Create track</Button>
        </form>
    </>);
}
