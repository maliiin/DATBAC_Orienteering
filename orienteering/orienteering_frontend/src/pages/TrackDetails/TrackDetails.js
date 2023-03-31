import { TextField, Button } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import CheckpointInfo from './Components/CheckpointInfo';
import CreateCheckpointForm from './Components/CreateCheckpointForm';
import Grid from '@mui/material/Grid';

//all details of single track, list of the checkpoints

export default function TrackDetails() {
    const navigate = useNavigate();
    const params = useParams();

    const [render, setRender] = useState(false);
    const [checkpointList, setCheckpointList] = useState("");

    const [trackInfo, setTrackInfo] = useState({
        Id: params.trackId
    });

    const showQrcodes = async () => {
        const trackid = params.trackId;
        navigate('/qrcodepage', { state: { trackid: trackid } })
    }

    //get all checkpoints for this id
    const loadCheckpoints = async () => {
        const url = "/api/checkpoint/getCheckpoints?trackId=" + params.trackId;
        const data = await fetch(url).then(res => res.json());

        setCheckpointList(data.map((checkpointElement, index) =>
            <CheckpointInfo
                key={checkpointElement.id + "-" + index}
                checkpointInfo={checkpointElement}
                updateCheckpointList={loadCheckpoints}
            >
            </CheckpointInfo>
        ));
    }

    useEffect(() => {
        loadCheckpoints();
    }, []);

    return (<>
        <Button onClick={showQrcodes}>Show QR-codes</Button>

        <Grid
            container
            spacing={3}
            margin="10px"
            direction={{ xs: "column-reverse", md: "row" }}
        >

            <Grid item xs={10} md={6}>
                <h4>Checkpoints</h4>
                <div>{checkpointList}</div>
            </Grid>

            <Grid item xs={10} md={4}>
                <CreateCheckpointForm
                    updateCheckpointList={loadCheckpoints}
                    trackId={trackInfo.Id}>
                </CreateCheckpointForm>
            </Grid>

        </Grid>
    </>);
};

