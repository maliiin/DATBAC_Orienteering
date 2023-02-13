import { TextField, Button } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import {  useEffect } from "react";
import CheckpointInfo from './CheckpointInfo';
import CreateCheckpointForm from './CreateCheckpointForm';

//all details of single track, list of the checkpoints

export default function TrackDetails() {
    const params = useParams();

    const [trackInfo, setTrackInfo] = useState({
        Id: params.trackId
    });

    const [checkpointList, setCheckpointList] = useState("");

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

        loadCheckpoints();
    }, []); 

    return (<>
        <CreateCheckpointForm trackId={trackInfo.Id }></CreateCheckpointForm>
        <p>single track {params.trackId}</p>
        <div>{checkpointList}</div>
    </>);
}

