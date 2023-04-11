import { React, useState } from "react";
import { Button, Box } from '@mui/material';
import { useNavigate } from 'react-router-dom';

export default function TrackInfo(props) {
    //props is props.Trackinfo (id, name, userid ...)

    const navigate = useNavigate();
    const [editing, setEditing] = useState(false);
    const [oldTitle, setOldTitle] = useState(props.trackInfo.trackName);


    //display spesific track
    const showTrack = (event) => {
        const url = "/track/" + props.trackInfo.trackId;
        navigate(url);
    }

    const shouldEdit = () => {
        setEditing(true)
    }

    const stopEdit = async () => {
        setEditing(false)

        const url = '/api/track/updateTrackTitle?';
        const parameter = "trackId=" + props.trackInfo.trackId+'&newTitle=' + oldTitle;
        const response = await fetch(url + parameter, { method: 'PUT' });
        //fiks sjekk respons i error handling

        //update list of parent
        props.updateTrackList()
    }

    const handleChange = (e) => {
        setOldTitle(e.target.value);
    }

    const deleteTrack = async() => {
        //fix implementer
        const url = '/api/track/deleteTrack?';
        const parameter = "trackId=" + props.trackInfo.trackId;
        const response = await fetch(url + parameter, { method: 'DELETE' });
        //fiks sjekk respons i error handling

        //update list of parent
        props.updateTrackList()
    }

    return (<>
        <Box border="1px solid lightblue;" margin="2px;" style={{ width:'80%'}}>
            <p style={{ display: "inline" }}>Title:</p>
            {editing ?
                <input
                    style={{ display: "inline" }}
                    type="text"
                    value={oldTitle}
                    onChange={handleChange}
                    onBlur={stopEdit}
                >
                </input>
                :
                <span
                    onDoubleClick={shouldEdit}
                > {props.trackInfo.trackName}</span>
            }
            <br></br>

            <Button onClick={showTrack}>Show details</Button>
            <Button onClick={deleteTrack}>Delete track</Button>
        </Box>

    </>);
}
