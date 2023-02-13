import { React } from "react";
import { Button } from '@mui/material';
import { Link, redirect, useNavigate } from 'react-router-dom';


//some info of track

export default function TrackInfo(props) {
    //props is props.Trackinfo (id, name, userid ...)

    const navigate = useNavigate();


    //display spesific track
    const showTrack = (event) => {
        const url = "/track/" + props.trackInfo.id;
        //console.log(url);
        navigate(url);

        //console.log(event);
    }





    return <Button onClick={showTrack}><h6>id: {props.trackInfo.id} userId: {props.trackInfo.userId}</h6></Button>;
}

//videre:
//lag onclik som redirecter til enTrack overview
//lag side som displayer hver enkelt post
//flytt ting i mappe