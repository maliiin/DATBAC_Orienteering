import Grid from '@mui/material/Grid';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import AddQuizQuestion from "./Components/AddQuizQuestion";
import DisplayQuiz from "./Components/DisplayQuiz";

export default function CheckpointDetails() {

    const navigate = useNavigate();
    //const [render, setRender] = useState(false);
    const [hasQuiz, setHasQuiz] = useState(false);
    const [QuizId, setQuizId] = useState("");
    const [quizChanged, setQuizChanged] = useState(1);
    const [gameType, setGameType] = useState("");
    const games = { 1: "Fallingboxes", 2: "Chemistry", 3: "LogicGates" };
    const params = useParams();
    const checkpointId = params.checkpointId;

    const loadCheckpoint = async () => {
        const response = await fetch("/api/checkpoint/getCheckpoint?checkpointId=" + checkpointId);
        //fiks naviger- naviger om feil status
        //https://auth0.com/blog/forbidden-unauthorized-http-status-codes/ 

        const checkpoint = await response.json();

        if (checkpoint.gameId == 0) {

            //this checkpoint has quiz
            setHasQuiz(true);
            setQuizId(checkpoint.quizId);

        } else {

            setGameType(games[checkpoint.gameId]);
        }
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
                direction={{ xs: "column-reverse", md: "row" }}>

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
            <p> The game you have chosen for this checkpoint: {gameType}</p>
            <p></p>
        </>);
    };
};
