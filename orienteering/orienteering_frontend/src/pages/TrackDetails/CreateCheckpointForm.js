import { React, useState, useEffect } from "react";
import { Button, TextField } from '@mui/material';
import { Link, redirect, useNavigate } from 'react-router-dom';


//some info of track, not details

export default function CreateCheckpointForm(props) {
    //props is props.Trackinfo (id, name, userid ...)
    const [checkpointInfo, setCheckpointInfo] = useState({
        Title: "",
        TrackId: ""

    });


    //set userId from props
    useEffect(() => {
        setCheckpointInfo(prevState => { return { ...prevState, TrackId: props.trackId } });
        //console.log("gjort")
        //console.log(props.id)
    }, [props.TrackId]);


    const handleSubmit = async (event) => {
        //setCheckpointInfo(prevState => { return { ...prevState, UserId: props.trackId } });

        console.log(checkpointInfo.Title)
        console.log(props.trackId)
        event.preventDefault();

        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            },
            body: JSON.stringify(checkpointInfo)
        };

        const response = await fetch('/api/track/createCheckpoint', requestOptions);
        return response;


    };

    const handleChange = (event) => {
        //update state
        setCheckpointInfo({ ...checkpointInfo, [event.target.name]: event.target.value });
    };






    return (<>
        <form onSubmit={handleSubmit}>

            <TextField
                required
                onChange={(e) => handleChange(e)}
                id="standard-basic" label="Tittel"
                name="Title"
                variant="standard"
                value={checkpointInfo.Title}
            />
            <br></br>
            <Button type="submit">Lag post</Button>
        </form>
    </>);
}


