import { TextField, Button } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import CheckpointInfo from './CheckpointInfo';
import CreateCheckpointForm from './CreateCheckpointForm';
import CheckpointTypeForm from '../../components/CheckpointTypeForm'



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
        var data = await fetch(url).then(res => res.json());
        console.log(data[0]);

        setCheckpointList(data.map((checkpointElement, index) =>
            <CheckpointInfo key={checkpointElement.id + "-" + index} checkpointInfo={checkpointElement}>
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
            <CreateCheckpointForm updateCheckpointList={loadCheckpoints} trackId={trackInfo.Id}></CreateCheckpointForm>

            <Button onClick={showQrcodes}>Vis QR-koder</Button>

            <h3>Her er en oversikt over alle orienteringsløypene dine</h3>
            <div>{checkpointList}</div>
        </>);
    };
}

