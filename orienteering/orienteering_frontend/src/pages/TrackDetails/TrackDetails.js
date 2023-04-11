import { Button } from '@mui/material';
import React, { useState } from "react";
import { useNavigate } from 'react-router-dom';
import { useParams } from 'react-router-dom';
import { useEffect } from "react";
import CheckpointInfo from './Components/CheckpointInfo';
import CreateCheckpointForm from './Components/CreateCheckpointForm';
import Grid from '@mui/material/Grid';

export default function TrackDetails() {
    const navigate = useNavigate();
    const params = useParams();

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
        const response = await fetch(url);

        //fix-obs se p�-n� navigeres bruker til error b�de om ikke logget inn og dersom
        //det er en annen brukers ting man ser p�
        if (!response.ok) {
            navigate("/errorpage")
        }
        const data = await response.json();

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

