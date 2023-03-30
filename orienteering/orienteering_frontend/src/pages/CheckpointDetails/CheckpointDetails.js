import Grid from '@mui/material/Grid';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import AddQuizQuestion from "./Components/AddQuizQuestion";
import DisplayQuiz from "./Components/DisplayQuiz";
import { Check } from '../../../../node_modules/@mui/icons-material/index';

//page
//display all info of a single checkpoint

export default function CheckpointDetails() {

    const navigate = useNavigate();
    //const [render, setRender] = useState(false);
    const [hasQuiz, setHasQuiz] = useState(false);
    const [QuizId, setQuizId] = useState("");
    const [quizChanged, setQuizChanged] = useState(1);

    const params = useParams();
    const checkpointId = params.checkpointId;

    const loadCheckpoint = async () => {
        const response = await fetch("/api/checkpoint/getCheckpoint?checkpointId=" + checkpointId);
        //fiks naviger- naviger om feil status
        //https://auth0.com/blog/forbidden-unauthorized-http-status-codes/ 
        

        console.log(response.status);
        const checkpoint = await response.json();

        if (checkpoint.gameId == 0) {

            //this checkpoint has quiz
            setHasQuiz(true);
            setQuizId(checkpoint.quizId);
        };
    }

    

    useEffect(() => {
        loadCheckpoint();
    })


    if (hasQuiz) {

        return (<>
            <Grid
                container
                spacing={3}
                margin="10px"
                direction={{ xs: "column-reverse", md: "row" }}

            >

                <Grid item xs={10} md={6}>
                    <h4>Questions</h4>
                    <DisplayQuiz quizChanged={quizChanged} setQuizChanged={setQuizChanged} quizId={QuizId}></DisplayQuiz>
                </Grid>

                <Grid item xs={10} md={6}>
                    <h4>Add more questions here</h4>
                    <AddQuizQuestion quizChanged={quizChanged} setQuizChanged={setQuizChanged} ></AddQuizQuestion>
                </Grid>
            </Grid>
        </>);

    } else {
        return (<>
            <p> You have chosen game</p>
            <p></p>
        </>);
    };
};

//fix slett dette
//useEffect(() => {
    //    //is authenticated and correct track?
    //    const isAuthenticated = async () => {

    //        //const checkUserUrl = "/api/user/getSignedInUserId";
    //        //const response = await fetch(checkUserUrl);

    //        //if (!response.ok) {
    //        //    //not signed in, redirect to login
    //        //    navigate("/login");
    //        //    return false;
    //        //};

    //        //const user = await response.json();
    //        //const userId = user.id;

    //        //load checkpoint
    //        const checkpoint = await fetch("/api/checkpoint/getCheckpoint?checkpointId=" + checkpointId).then(res => res.json());
    //        //console.log(checkpoint.quizId);
    //        //console.log(typeof (checkpoint.quizId));
    //        //console.log(checkpoint.gameId);


    //        if (checkpoint.gameId == 0) {

    //            //this checkpoint has quiz
    //            setHasQuiz(true);
    //            setQuizId(checkpoint.quizId);
    //        };


    //        ////check that the signed in user owns the track
    //        //const trackId = checkpoint.trackId;
    //        //const getTrackUrl = "/api/track/getTrack?trackId=" + trackId;

    //        //const track = await fetch(getTrackUrl).then(res => res.json());

    //        //if (userId != track.userId) {
    //        //    navigate("/unauthorized");
    //        //    return false;
    //        //}
    //        //return true;

    //    };

    //    //isAuthenticated().then(result => { setRender(result) });
    //    loadCheckpoint();

    //}, []);