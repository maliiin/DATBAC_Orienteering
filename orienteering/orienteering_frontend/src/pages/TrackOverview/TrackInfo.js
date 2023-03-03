import { React } from "react";
import { Button } from '@mui/material';
import { Link, redirect, useNavigate } from 'react-router-dom';


//some info of track, not details

export default function TrackInfo(props) {
    //props is props.Trackinfo (id, name, userid ...)

    const navigate = useNavigate();


    //display spesific track
    const showTrack = (event) => {
        const url = "/track/" + props.trackInfo.trackId;
        //console.log(url);
        navigate(url);

        //console.log(event);
    }





    return (<>
        <Button onClick={showTrack}>
            <h6>
                title {props.trackInfo.trackName} track-id: {props.trackInfo.trackId}
            </h6>
        </Button>
    </>);
}
