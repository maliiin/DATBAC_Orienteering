import { TextField, Grid, Box, Button, FormLabel, RadioGroup, FormControlLabel, Radio } from '@mui/material';
import React, { useState, useRef } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";

export default function QuizPage2() {

    const navigate = useNavigate();
    const params = useParams();

    //what the user answers
    const [guess, setGuess] = useState("");

    //alternatives to guess
    const [radios, setRadios] = useState("");

    const [quizId, setQuizId] = useState("");
    const [quizQuestionIndex, setQuizQuestionIndex] = useState(0);

    //this is one question
    const [currentQuizQuestion, setCurrentQuizQuestion] = useState("");

    const [endOfQuiz, setEndOfQuiz] = useState(false);
    const [quizStatus, setQuizStatus] = useState(false);

    useEffect(() => {
        getQuizId();
    }, []);

    useEffect(() => {
        displayRadio();
    }, [currentQuizQuestion]);

    useEffect(() => {
        if (quizId != "") {
            getQuizQuestion();
        }
    }, [quizQuestionIndex, quizId]);

    //gets the quizId for this checkpoint
    async function getQuizId() {
        var url = "/api/checkpoint/getCheckpoint?checkpointId=" + params.checkpointId;
        var checkpoint = await fetch(url).then(res => res.json());
        setQuizId(checkpoint.quizId);
    }

    //gets the current quiz Question
    async function getQuizQuestion() {
        var url = "/api/quiz/getNextQuizQuestion?quizId=" + quizId + "&quizQuestionIndex=" + quizQuestionIndex.toString();
        var quizQuestion = await fetch(url).then(res => res.json());
        setCurrentQuizQuestion(quizQuestion);
    };

    async function handleSubmit(event) {
        event.preventDefault();

        //get correct answer from backend
        var url = "/api/quiz/getSolution?quizId=" + quizId + "&quizQuestionId=" + currentQuizQuestion.quizQuestionId;
        var solution = await fetch(url).then(res => res.text());

        //check if answer is correct
        if (solution == guess) {
            setQuizStatus(<p>Correct answer</p>)
        } else {
            setQuizStatus(<p>Wrong answer. Riktig svar var: {solution}</p>)
        }

        //check if end of quiz or not
        if (currentQuizQuestion.endOfQuiz == true) {
            setEndOfQuiz(true);
        }
        else {
            //increase index
            var newIndex = quizQuestionIndex + 1;
            setQuizQuestionIndex(newIndex);
        };

        //reset answer
        setGuess("")
    };

    async function displayRadio() {
        var t = currentQuizQuestion.alternatives.map((alternative, index) => {
            return (
                <FormControlLabel
                    key={alternative.text + "-" + index}
                    value={alternative.text}
                    label={alternative.text}
                    control={<Radio required={true} />}
                    defaultChecked={guess == alternative.text}
                //style={{backgroundColor:"blue"} }
                //checked={guess == alternative.text}

                >
                </FormControlLabel>
            )
        })
        setRadios(t)
    }

    //sends user to navigation page
    function navigateToNextCheckpoint() {
        navigate('/checkpointnavigation/' + params.checkpointId);
    }


    const changeGuess = (event) => {
        setGuess(event.target.value);
    };



    return (<>

        <Grid
            container
            spacing={0}
            direction="column"
            alignItems="center"
            justifyContent="center"

            style={{
                minHeight: '50vh',
            }}
        >
            <Grid
                item
                sx={10}
                style={{
                    width: '80%'
                }}
            >


                <Box
                    onSubmit={handleSubmit}
                    component="form"
                    style={{
                        display: endOfQuiz ? "none" : "block"
                    }}

                >
                    <FormLabel
                        id="question"
                    >
                        {currentQuizQuestion.question}
                    </FormLabel>

                    <RadioGroup

                        value={guess}
                        aria-labelledby="radio-buttons-group"
                        name="radio-buttons-group"
                        label="hei"
                        onChange={(e) => changeGuess(e)}
                    >
                        {radios}

                    </RadioGroup>

                    <Button
                        type="submit"
                        variant="contained"
                    >
                        Check answer
                    </Button>






                </Box>
                {quizStatus}

                <div style={{
                    display: endOfQuiz ? "block" : "none"
                }}>

                    <p>End of quiz</p>

                    <Button onClick={navigateToNextCheckpoint}>
                        Navigate to next checkpoint
                    </Button>
                </div>
            </Grid>
        </Grid>
    </>);
}