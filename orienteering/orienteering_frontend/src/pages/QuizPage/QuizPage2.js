
import { TextField, Box, Button, FormLabel, RadioGroup, FormControlLabel, Radio } from '@mui/material';
import React, { useState, useRef } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";


export default function QuizPage2() {

    const navigate = useNavigate();
    const params = useParams();

    //what the user answers
    const [guess, setGuess] = useState("");
    const [radios, setRadios] = useState("");

    const [quizId, setQuizId] = useState("");
    const [quizQuestionIndex, setQuizQuestionIndex] = useState(0);

    //this is one question
    const [currentQuizQuestion, setCurrentQuizQuestion] = useState({
        alternatives: [0, 1]
    });

    const [endOfQuiz, setEndOfQuiz] = useState(false);

    useEffect(() => {
        getQuizId();
        // fetchQuizQuestion();
    }, []);

    useEffect(() => {
        displayRadio();
        // fetchQuizQuestion();
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

    async function displayRadio() {
        var t = currentQuizQuestion.alternatives.map((alternative, index) => {
            return (
                <FormControlLabel
                    key={alternative.text + "-" + index}
                    value={alternative.text}
                    label={alternative.text}
                    control={<Radio required={true} />}
                    defaultChecked={guess == alternative.text}
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
        console.log(event.target.value)
        console.log(event)
        console.log(event.target)
        event.target.checked = 'true'
        setGuess(event.target.value);
    };

    async function handleSubmit(event) {
        console.log("not implemented")
    }

    return (<>
        <Box onSubmit={handleSubmit} component="form">
            <RadioGroup
                value={guess}
                aria-labelledby="radio-buttons-group"
                name="radio-buttons-group"

                onChange={(e) => changeGuess(e)}
            >





                {radios}

            </RadioGroup>
            <Button type="submit" variant="contained">Besvar spørsmål</Button>


            {guess}
        </Box>

    </>);
}