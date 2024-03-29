import { Grid, Box, Button, FormLabel, RadioGroup, FormControlLabel, Radio } from '@mui/material';
import React, { useState } from "react";
import { useNavigate, useParams } from 'react-router-dom';
import { useEffect } from "react";
import CheckpointDescription from '../../Components/CheckpointDescription';

export default function QuizPage() {

    const navigate = useNavigate();
    const params = useParams();
    const [score, setScore] = useState(0);

    //what the user answers
    const [guess, setGuess] = useState("");

    //alternatives to guess
    const [radios, setRadios] = useState("");

    const [quiz, setQuiz] = useState("");
    const [quizQuestionIndex, setQuizQuestionIndex] = useState(0);

    //this is one question
    const [currentQuizQuestion, setCurrentQuizQuestion] = useState("");

    const [endOfQuiz, setEndOfQuiz] = useState(false);
    const [quizStatus, setQuizStatus] = useState(false);
    const [checkpointDescriptionDisplayed, setCheckpointDescriptionDisplayed] = useState(false);

    useEffect(() => {
        getQuiz();
        checkSession();
    }, []);

    useEffect(() => {
        if (currentQuizQuestion != "") {
            displayRadio();
        }
    }, [currentQuizQuestion]);

    useEffect(() => {
        if (quiz != "") {
            nextQuizQuestion();
        }
    }, [quizQuestionIndex, quiz]);

    async function checkSession() {
        const url = "/api/session/setStartCheckpoint";
        await fetch(url, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                CheckpointId: params.checkpointId
            })
        });
    }

    async function getQuiz() {
        const quizUrl = "/api/quiz/getQuizByCheckpoint?checkpointId=" + params.checkpointId;
        const quizResponse = await fetch(quizUrl);
        if (!quizResponse.ok) {
            navigate("/errorpage");
        }
        const fetchedQuiz = await quizResponse.json();
        setQuiz(fetchedQuiz);
    }

    async function nextQuizQuestion() {
        const quizQuestion = quiz.quizQuestions[quizQuestionIndex];
        setCurrentQuizQuestion(quizQuestion);
    };

    async function handleSubmit(event) {
        event.preventDefault();

        var solution = currentQuizQuestion.alternatives[currentQuizQuestion.correctAlternative - 1].text;
        //check if end of quiz or not
        if (quiz.quizQuestions.length == quizQuestionIndex + 1) {
            setEndOfQuiz(true);
            //score+1 because additional score not is added yet
            addScore(score + 1);
        }
        else {
            const newIndex = quizQuestionIndex + 1;
            setQuizQuestionIndex(newIndex);
        };

        //check if answer is correct
        if (solution == guess) {
            setQuizStatus(<>
                <p>Correct answer</p>
            </>)
            setScore(score + 1);
        } else {
            setQuizStatus(<p>Wrong answer. Correct answer is: {solution}</p>)
        }



        //reset answer
        setGuess("")
    };

    async function addScore(points) {

        const requestAlternatives = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({ Score: "" + points })
        };
        await fetch("/api/session/addScore", requestAlternatives)
    }

    async function displayRadio() {
        var t = currentQuizQuestion.alternatives.map((alternative, index) => {
            return (
                <FormControlLabel
                    key={alternative.text + "-" + index}
                    value={alternative.text}
                    label={alternative.text}
                    control={<Radio required={true} />}
                    defaultChecked={guess == alternative.text}>
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

    async function displayRadio() {
        var t = currentQuizQuestion.alternatives.map((alternative, index) => {
            return (
                <FormControlLabel
                    key={alternative.text + "-" + index}
                    value={alternative.text}
                    label={alternative.text}
                    control={<Radio required={true} />}
                    defaultChecked={guess == alternative.text}>
                </FormControlLabel>
            )
        })
        setRadios(t)
    }

    const hideCheckpointDescription = async () => {
        setCheckpointDescriptionDisplayed(true);
    }

    if (checkpointDescriptionDisplayed) {
        return (<>

            <Grid
                container
                spacing={0}
                direction="column"
                alignItems="center"
                style={{
                    minHeight: '50vh',
                }}
            >
                <Grid
                    item
                    sx={10}
                    style={{
                        width: '70%',
                        top: "20%",
                        position: "absolute"
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
                        <p>You got {score} points.</p>

                        <Button onClick={navigateToNextCheckpoint}>
                            Navigate to next checkpoint
                        </Button>
                    </div>
                </Grid>
            </Grid>
        </>);
    }

    if (!checkpointDescriptionDisplayed) {
        return (
            <>
                <CheckpointDescription
                    checkpointId={params.checkpointId}
                    hideDescription={hideCheckpointDescription}>
                </CheckpointDescription>
            </>);
    }
}