import { React, useState } from "react";
import { Button, Box } from '@mui/material';
import { useNavigate } from 'react-router-dom';

export default function TrackInfo(props) {
    //props is props.Trackinfo (id, name, userid ...)

    const navigate = useNavigate();
    const [editing, setEditing] = useState(false);
    const [oldTitle, setOldTitle] = useState(props.trackInfo.trackName);


    //display spesific track
    const showTrack = () => {
        const url = "/track/" + props.trackInfo.trackId;
        navigate(url);
    }

    const shouldEdit = () => {
        setEditing(true)
    }

    const stopEdit = async () => {
        setEditing(false)

        const url = '/api/track/updateTrackTitle';
        await fetch(url, {
            method: "PATCH",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                TrackId: props.trackInfo.trackId,
                NewTitle: oldTitle
            })
        });

        //update list of parent
        props.updateTrackList()
    }

    const handleChange = (e) => {
        setOldTitle(e.target.value);
    }

    const deleteTrack = async() => {
        const url = '/api/track/deleteTrack?';
        const parameter = "trackId=" + props.trackInfo.trackId;
        await fetch(url + parameter, { method: 'DELETE' });

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
