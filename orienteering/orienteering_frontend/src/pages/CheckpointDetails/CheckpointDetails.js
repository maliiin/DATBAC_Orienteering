import useAuthentication from "../../hooks/useAuthentication";
import { TextField, Button } from '@mui/material';
import Grid from '@mui/material/Grid';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import AddQuizQuestion from "./AddQuizQuestion";
import DisplayQuiz from "./DisplayQuiz";
import { AccessAlarm, ThreeDRotation } from '@mui/icons-material';
import DeleteIcon from '@mui/icons-material/Delete';


//
//import DropdownMenu from '../../components/DropdownMenu';


//page
//display all info of a single checkpoint

export default function CheckpointDetails() {
    //useAuthentication();
    const navigate = useNavigate();
    const [render, setRender] = useState(false);
    const [hasQuiz, setHasQuiz] = useState(false);
    const [QuizId, setQuizId] = useState("");



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
            const checkpoint = await fetch("/api/checkpoint/getCheckpoint?checkpointId=" + checkpointId).then(r => r.json());

            console.log(checkpoint.quizId);
            console.log(typeof (checkpoint.quizId));
            console.log(checkpoint.gameId);


            if (checkpoint.gameId == 0) {
                //this checkpoint has quiz
                setHasQuiz(true);
                console.log(checkpoint.quizId);
                setQuizId(checkpoint.quizId);

            };

            const trackId = checkpoint.trackId;
            console.log(trackId);

            //få trackid fra dette checkpointet
            //const trackId = params.trackId;

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


    console.log(hasQuiz);
    if (render && hasQuiz) {

        return (<>
            <Grid container spacing={3} margin="10px">
                
                <Grid item xs={6}>
                    <h4>Oversikt over spørsmål til quiz</h4>
                    <DisplayQuiz quizId={QuizId}></DisplayQuiz>
                </Grid>
                <Grid item xs={6}>
                    <h4>Legg til flere spørsmål her</h4>
                    <AddQuizQuestion></AddQuizQuestion>
                </Grid>

            </Grid>
        </>);

    } else if(render) {
        return (<>
            <h3>post id {params.checkpointId}</h3>
            <p> du har valgt spill</p>
        </>);
    };
};


