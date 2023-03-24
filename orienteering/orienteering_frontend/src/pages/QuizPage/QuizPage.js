import { TextField,Box, Button, FormControl, FormLabel, RadioGroup, FormControlLabel, Radio } from '@mui/material';
import React, { useState, useRef } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
export default function QuizPage() {

    const navigate = useNavigate();
    const params = useParams();

    const [quizQuestionRender, setQuizQuestionRender] = useState("");

    //the current quiz question
    const [currentQuizQuestion, setCurrentQuizQuestion] = useState("");

    const chosenAlternative = useRef("");
    const [quizQuestionIndex, setQuizQuestionIndex] = useState(0);
    const [quizStatus, setQuizStatus] = useState("");

    const [endOfQuiz, setEndOfQuiz] = useState(false);

    //answer
    const [activity, setActivity] = useState("");



    const [quizInfo, setQuizInfo] = useState({
        Id: ""
    });


    useEffect(() => {
        setQuizId();
        // fetchQuizQuestion();
    }, []);


    useEffect(() => {
        if (quizInfo.Id != "") {
            fetchQuizQuestion();
        }
    }, [quizQuestionIndex, quizInfo]);

    useEffect(() => {
        showQuizQuestion();
    }, [currentQuizQuestion]);



    //gets the quizId for this checkpoint
    async function setQuizId() {
        var url = "/api/checkpoint/getCheckpoint?checkpointId=" + params.checkpointId;
        var checkpointDto = await fetch(url).then(res => res.json());
        setQuizInfo(prevState => { return { ...prevState, Id: checkpointDto.quizId } });
    }

    //gets the current quiz Question
    async function fetchQuizQuestion() {
        var url = "/api/quiz/getNextQuizQuestion?quizId=" + quizInfo.Id + "&quizQuestionIndex=" + quizQuestionIndex.toString();
        var quizQuestion = await fetch(url).then(res => res.json());
        setCurrentQuizQuestion(quizQuestion);
    };


    function handleChange(event) {
        console.log("endret radiovalg");
        console.log(event.target.value);
        chosenAlternative.current = event.target.value;
    };


    const changeActivity = (event) => {

        setActivity(event.target.value);
    };

    function navigateToNextCheckpoint() {
        navigate('/checkpointnavigation/' + params.checkpointId);
    }

    async function handleSubmit(event) {
        event.preventDefault();

        //get correct answer from backend
        var url = "/api/quiz/getSolution?quizId=" + quizInfo.Id + "&quizQuestionId=" + currentQuizQuestion.quizQuestionId;
        var solution = await fetch(url).then(res => res.text());

        //if (chosenAlternative.current == solution) {
        //    setQuizStatus(<p>Riktig svar</p>)
        //}
        //else {
        //    setQuizStatus(<p>Feil svar. Riktig svar var: {solution}</p>)
        //};


        //check if end of quiz or not
        if (currentQuizQuestion.endOfQuiz == true) {
            console.log("her")
            setEndOfQuiz(true);
            //slett det under
            //setQuizQuestionRender(
            //    <>
            //        <p>Quiz ferdig</p>
            //        <button onClick={navigateToNextCheckpoint}>Navhghghghigate to next checkpoint</button>
            //    </>
            //)

        }
        else {
            var newIndex = quizQuestionIndex + 1;
            setQuizQuestionIndex(newIndex);
        };
    };

    function showQuizQuestion() {
        if (typeof currentQuizQuestion.alternatives != 'undefined') {
            var currentAlternatives = currentQuizQuestion.alternatives;
            var radioButtons = currentQuizQuestion.alternatives.map((alternative, index) =>
                <FormControlLabel
                    value={alternative.text}
                    key={alternative.id + "-" + index}
                    control={<Radio />}
                    label={alternative.text}
                    checked={activity == alternative.text}

                />
            );
        };
        setQuizQuestionRender(
            <Box onSubmit={handleSubmit} component="form">
                <FormLabel id="question">{currentQuizQuestion.question}</FormLabel>
 



                <RadioGroup
                    value={activity}
                    aria-labelledby="radio-buttons-group"
                    name="radio-buttons-group"
                    row
                    onChange={(e) => changeActivity(e)}
                >

                    <FormControlLabel
                        value="spill"
                        label="Spill"
                        control={<Radio required={true} />}
                        checked={activity == "spill"}
                    />

                    <FormControlLabel
                        value="quiz"
                        control={<Radio required={true} />}
                        label="Quiz"
                        checked={activity == "quiz"}
                    />

                </RadioGroup>
                <Button type="submit" variant="contained">Besvar spørsmål</Button>
            </Box>
        );
    };





    return (<>
        {quizQuestionRender}
        {quizStatus}

        {activity }
        <button
            onClick={navigateToNextCheckpoint}
            style={{
                display: endOfQuiz ? "block" : "none"

                ,
            }}
        >
            Navigate to next checkpoint

        </button>


    </>
    );

}
