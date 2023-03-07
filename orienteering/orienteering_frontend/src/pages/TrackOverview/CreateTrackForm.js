import { React, useState, useEffect } from "react";
import { Button, TextField } from '@mui/material';
import { Link, redirect, useNavigate } from 'react-router-dom';


//some info of track, not details

export default function CreateTrackForm(props) {
    //props is props.Trackinfo (id, name, userid ...)
    const [trackInfo, setTrackInfo] = useState({
        UserId: "",
        TrackName: ""
    });

    const navigate = useNavigate();

    //set userId from props
    useEffect(() => {
        setTrackInfo(prevState => { return { ...prevState, UserId: props.id } });
        console.log("gjort")
        console.log(props.id)
    }, [props.id]); 

    //display spesific track
    const handleSubmit = async (event) => {
        
        setTrackInfo(prevState => { return { ...prevState, UserId: props.id } });

        console.log(trackInfo.id)
        console.log(props.id)
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

        props.updateTracks();
        //if (response.status.su

        //clear form
        setTrackInfo({
            UserId: "",
            TrackName: ""
        });
        return response;


    };

    const handleChange = (event) => {
        //update state
        setTrackInfo({ ...trackInfo, [event.target.name]: event.target.value });
    };






    return (<>
        <h4>Legg til ny loype</h4>
        <form onSubmit={handleSubmit}>

            <TextField
                required
                onChange={(e) => handleChange(e)}
                id="standard-basic" label="Tittel"
                name="TrackName"
                variant="standard"
                value={trackInfo.TrackName}
            />
            <br></br>
            <Button type="submit">Lag løype</Button>
        </form>
    </>);
}
//videre:
//lag onclik som redirecter til enTrack overview
//lag side som displayer hver enkelt post
//flytt ting i mappe


