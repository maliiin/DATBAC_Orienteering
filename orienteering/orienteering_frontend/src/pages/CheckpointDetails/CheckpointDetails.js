import useAuthentication from "../../hooks/useAuthentication";
import { TextField, Button } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";


//page
//display all info of a single checkpoint

export default function CheckpointDetails() {
    //useAuthentication();
    const navigate = useNavigate();
    const [render, setRender] = useState(false);


    const params = useParams();
    const checkpointId = params.checkpointId;
    console.log(checkpointId);

    //dette fungerer litt, men getCheckpoint returnerer checkpoint uten id

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
            console.log(checkpointId);
            const checkpoint = await fetch("/api/track/getCheckpoint?checkpointId=" + checkpointId).then(r => r.json());
            
            const trackId = checkpoint.trackId;
            console.log(trackId);

            //få trackid fra dette checkpointet
            //const trackId = params.trackId;

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

    if (render) {

        return <h1>post id {params.checkpointId}</h1>;

    };
};


