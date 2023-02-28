import { TextField, Button } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";



//all details of single track, list of the checkpoints

export default function CheckpointRedirection() {
    const navigate = useNavigate();
    const params = useParams();

    const [render, setRender] = useState(false);

    const [trackInfo, setTrackInfo] = useState({
        Id: params.checkpointId
    });

    useEffect(() => {
        setTrackInfo(prevState => { return { ...prevState, Id: params.checkpointId } });
        redirectToPage()
    }, []);



    const redirectToPage = async () => {
        var url = "/api/checkpoint/getCheckpoint?checkpointId=" + trackInfo.Id;
        var checkpoint = await fetch(url).then(res => res.json());
        if (checkpoint.quizId == null) {
            console.log("spill");
        }
        else {
            navigate('/checkpoint/quiz/' + checkpoint.quizId);
        }
        

    }

    if (render == true) {
        return null
    };
}

