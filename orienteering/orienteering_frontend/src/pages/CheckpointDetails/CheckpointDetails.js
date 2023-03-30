import Grid from '@mui/material/Grid';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import AddQuizQuestion from "./Components/AddQuizQuestion";
import DisplayQuiz from "./Components/DisplayQuiz";

//page
//display all info of a single checkpoint

export default function CheckpointDetails() {

    const navigate = useNavigate();
    //endre navn? fix. til autenticated ?
    const [render, setRender] = useState(false);
    const [hasQuiz, setHasQuiz] = useState(false);
    const [QuizId, setQuizId] = useState("");
    const [quizChanged, setQuizChanged] = useState(1);
    const [gameType, setGameType] = useState("");

    const params = useParams();
    const checkpointId = params.checkpointId;

    const games = { 1: "Fallingboxes", 2: "Chemistry", 3: "LogicGates" };


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

            //load checkpoint
            const checkpoint = await fetch("/api/checkpoint/getCheckpoint?checkpointId=" + checkpointId).then(res => res.json());
            


            if (checkpoint.gameId == 0) {

                //this checkpoint has quiz
                setHasQuiz(true);
                setQuizId(checkpoint.quizId);
            }
            else {
                setGameType(games[checkpoint.gameId]);
            }

            //check that the signed in user owns the track
            const trackId = checkpoint.trackId;
            const getTrackUrl = "/api/track/getTrack?trackId=" + trackId;

            const track = await fetch(getTrackUrl).then(res => res.json());

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
            <Grid
                container
                spacing={3}
                margin="10px"
                direction={{ xs: "column-reverse", md: "row" }}

            >
                
                <Grid item xs={10} md={6 }>
                    <h4>Questions</h4>
                    <DisplayQuiz quizChanged={quizChanged} setQuizChanged={setQuizChanged} quizId={QuizId}></DisplayQuiz>
                </Grid>

                <Grid item xs={10} md={6 }>
                    <h4>Add more questions here</h4>
                    <AddQuizQuestion quizChanged={quizChanged} setQuizChanged={setQuizChanged} ></AddQuizQuestion>
                </Grid>
            </Grid>
        </>);

    } else if(render) {
        return (<>
            <p> The game you have chosen for this checkpoint: {gameType}</p>
            <p></p>
        </>);
    };
};


