import { TextField, Button } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import CheckpointInfo from './CheckpointInfo';
import CreateCheckpointForm from './CreateCheckpointForm';



//all details of single track, list of the checkpoints

export default function TrackDetails() {
    const navigate = useNavigate();
    const params = useParams();

    const [render, setRender] = useState(false);
    const [checkpointList, setCheckpointList] = useState("");

    const [trackInfo, setTrackInfo] = useState({
        Id: params.trackId
    });

    //kanksje flytt alt dette til en hook?
    //hook har use effect, blir kalt på som funskjoen her blir kalt.
    useEffect(() => {
        //is authenticated and correct track?
        const isAuthenticated = async () => {

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

            const getTrackUrl = "https://localhost:3000/api/track/getTrack?trackId=" + trackId;

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
        const url = "/api/track/getCheckpoints?trackId=" + params.trackId;
        var data = await fetch(url).then(res => res.json());

        setCheckpointList(data.map((checkpointElement, index) =>
            <CheckpointInfo key={checkpointElement.id + "-" + index} checkpointInfo={checkpointElement}>
            </CheckpointInfo>

        ));
    }

    useEffect(() => {
        setTrackInfo(prevState => { return { ...prevState, Id: params.trackId } });
        loadCheckpoints();
    }, []);


    if (render == true) {
        return (<>
            <CreateCheckpointForm trackId={trackInfo.Id}></CreateCheckpointForm>
            <p>single track {params.trackId}</p>
            <div>{checkpointList}</div>
        </>);
    };
}

