import { TextField, Button } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import {  useEffect } from "react";
import CheckpointInfo from './CheckpointInfo';

//all details of single track, list of the checkpoints

export default function TrackDetails() {
    const params = useParams();

    const [trackInfo, setTrackInfo] = useState({
        Id: params.trackId
    });

    const [checkpointList, setCheckpointList] = useState("");

    const navigate = useNavigate();


    //get all checkpoints for this id
    const loadCheckpoints = async () => {
       const url = "/api/track/getCheckpoints?trackId=" + params.trackId;
        var data = await fetch(url).then(res => res.json());
        //setCheckpointList(data);
        setCheckpointList(data.map((checkpointElement, index) =>
            <CheckpointInfo key={checkpointElement.id + "-" + index} checkpointInfo={checkpointElement}>
            </CheckpointInfo>
            
        ));
        console.log("her kommer alle checkpoint")
        console.log(data);    
    }

    useEffect(() => {
        setTrackInfo(prevState => { return { ...prevState, Id: params.trackId } });

        //console.log("før checkpoints!!!!");
        loadCheckpoints();
    }, []); 


    const createNewCheckpoint = async () => {
        //console.log(trackInfo);
        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            },
            body: JSON.stringify(trackInfo)
        };
        
        const response = await fetch('/api/track/createCheckpoint', requestOptions);
        //if (response.status.su
        return response;
    }


    const showQrcodes = async () => {
        const trackid = params.trackId;
        //const link = "/qrcodepage/" + trackid;
        //
        navigate('/qrcodepage', { state: { trackid: trackid } })

    }



    //console.log(params);
    return (<>

        <p>single track {params.trackId}</p>
        <Button onClick={createNewCheckpoint}>lag checkpoint</Button>
        <Button onClick={showQrcodes}>Vis QR-koder</Button>
        <div>{checkpointList}</div>
    </>);
}
