import { TextField, Button } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import {  useEffect } from "react";
import CheckpointInfo from './CheckpointInfo';
import CreateCheckpointForm from './CreateCheckpointForm';
import useAuthentication from "../../hooks/useAuthentication";
import useAuthorizeTrack from "../../hooks/useAuthorizeTrack";



//all details of single track, list of the checkpoints

export default function TrackDetails() {
    const [render, setRender] = useState(false);
    const navigate = useNavigate();


    const params = useParams();
    const [shouldRender, setShouldRender] = useState(false);



    //useAuthentication();
    useEffect(() => {
        //is authenticated and allowed?
        const isAuthenticated = async () => {
            //check if user is signed in, redirect if not
            const checkUserUrl = "/api/user/getSignedInUserId";
            const response = await fetch(checkUserUrl);
            if (!response.ok) {
                navigate("/login");
                return false;
            } else {
                console.log("user is signed in");
                const userId = await response.json();
                console.log(userId);

                const trackId = params.trackId;
                const getTrackUrl = "https://localhost:3000/api/track/getTrack?trackId=" + trackId;
                const result = await fetch(getTrackUrl);
                const track = await result.json();


                if (userId == track.userId) {
                    console.log("DE ER LIKE------------");
                    //lovlig = true;
                    return true;
                } else {
                    console.log("DE ER ULIKE-----------");
                    navigate("/unauthorized");
                    return false;
                }


                return true;
            };

        };


        isAuthenticated().then(result => { setRender(result) });

    }, []);


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


        //setShouldRender(useAuthorizeTrack(params.trackId));
        //setShouldRender(true);

    }, []); 

    console.log(shouldRender);
    if (render==true) { 
        return (<>
            <h1>{shouldRender} hehehe</h1>
            <CreateCheckpointForm trackId={trackInfo.Id}></CreateCheckpointForm>
            <p>single track {params.trackId}</p>
            <div>{checkpointList}</div>
        </>);
    };
}

