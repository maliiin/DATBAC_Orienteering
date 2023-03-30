import { React, useState, useEffect } from "react";
import { Button, TextField } from '@mui/material';
import { Link, redirect, useNavigate } from 'react-router-dom';


//some info of track, not details

export default function CreateTrackForm(props) {
    //props is props.Trackinfo (id, name, userid ...)
    const [trackInfo, setTrackInfo] = useState({
        //UserId: "",
        TrackName: ""
    });

    const navigate = useNavigate();

    //set userId from props
    //fiks tror ikke dette trengs
    //trenger heller ikke sende inn userId??
    //useEffect(() => {
    //    //setTrackInfo(prevState => { return { ...prevState, UserId: props.id } });
    //    console.log("gjort")
    //    console.log(props.id)
    //}, [props.id]); 

    //display spesific track
    const handleSubmit = async (event) => {
        
        //setTrackInfo(prevState => { return { ...prevState, UserId: props.id } });

        //console.log(trackInfo.id)
       // console.log(props.id)
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



