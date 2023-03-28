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

    //fiks kanksje flytt alt dette til en hook?
    //hook har use effect, blir kalt på som funskjoen her blir kalt.
    useEffect(() => {
        //is authenticated and correct track?
        const isAuthenticated = async () => {

            //check that user is signed in
            const checkUserUrl = "/api/user/getSignedInUserId";
            const response = await fetch(checkUserUrl);

            if (!response.ok) {
                //not signed in, redirect to login
                navigate("/login");
                return false;
            };


            const user = await response.json();
            const userId = user.id;
            const trackId = params.trackId;
            setTrackInfo(prevState => { return { ...prevState, Id: params.trackId } });



            const getTrackUrl = "/api/track/getTrack?trackId=" + trackId;

            const result = await fetch(getTrackUrl);
            if (!result.ok) {
                navigate("/errorpage");
            }
            const track = await result.json();

            if (userId != track.userId) {
                navigate("/unauthorized");
                return false;
            }
            return true;


        };

        isAuthenticated().then(result => { setRender(result) });

    }, []);

    //get all checkpoints for this id
    const loadCheckpoints = async () => {
        const url = "/api/checkpoint/getCheckpoints?trackId=" + params.trackId;
        const data = await fetch(url).then(res => res.json());
        console.log(data[0]);

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

    const showQrcodes = async () => {
        const trackid = params.trackId;
        //const link = "/qrcodepage/" + trackid;
        //
        navigate('/qrcodepage', { state: { trackid: trackid } })
    }

    if (render == true) {
        return (<>
            <Button onClick={showQrcodes}>Show QR-codes</Button>

            <Grid
                container
                spacing={3}
                margin="10px"
                direction={{ xs: "column-reverse", md: "row" }}
            >
                
                <Grid item xs={10} md={6 }>
                    <h4>Checkpoints</h4>
                    <div>{checkpointList}</div>
                </Grid>

                <Grid item xs={10} md={4 }>
                    <CreateCheckpointForm updateCheckpointList={loadCheckpoints} trackId={trackInfo.Id}></CreateCheckpointForm>
                </Grid>

            </Grid>
        </>);
    };
}

