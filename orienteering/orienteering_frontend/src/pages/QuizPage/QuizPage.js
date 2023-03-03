import { TextField, Button, FormControl, FormLabel, RadioGroup, FormControlLabel, Radio } from '@mui/material';
import React, { useState, useRef } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import QuizQuestionItem from './QuizQuestionItem';
export default function QuizPage() {
    //kan være greit å få inn checkpointid slik at det senere blir mulig å registrere at noen har vært på checkpointet, lagre score osv...

    const params = useParams();

    const [quizQuestionRender, setQuizQuestionRender] = useState("");
    const [currentQuizQuestion, setCurrentQuizQuestion] = useState("");

    const chosenAlternative = useRef("");
    const [quizQuestionIndex, setQuizQuestionIndex] = useState(0);
    const [quizStatus, setQuizStatus] = useState("");

    const [quizInfo, setQuizInfo] = useState({
        Id: params.quizId
    });


    useEffect(() => {
        setQuizInfo(prevState => { return { ...prevState, Id: params.quizId } });
       // fetchQuizQuestion();
    }, []);


    useEffect(() => {
        fetchQuizQuestion();
    }, [quizQuestionIndex]);

    useEffect(() => {
        showQuizQuestion();
    }, [currentQuizQuestion, quizStatus]);


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

    async function handleSubmit(event) {
        event.preventDefault();
        var url = "/api/quiz/getSolution?quizId=" + quizInfo.Id + "&quizQuestionId=" + currentQuizQuestion.quizQuestionId;
        var solution = await fetch(url).then(res => res.text());
        if (chosenAlternative.current == solution) {
            setQuizStatus(<p>Riktig svar</p>)
        }
        else {
            setQuizStatus(<p>Feil svar. Riktig svar var: {solution}</p>)
        };

        
        if (currentQuizQuestion.endOfQuiz == true) {
            setQuizQuestionRender(<p>Quiz ferdig</p>)
        }
        else {
            var newIndex = quizQuestionIndex + 1;
            setQuizQuestionIndex(newIndex);
        };
    };

    function showQuizQuestion() {
        if (typeof currentQuizQuestion.alternative != 'undefined') {
            var currentAlternatives = currentQuizQuestion.alternative;
            var radioButtons = currentAlternatives.map((alternative, index) =>
                <FormControlLabel value={alternative.text} key={alternative.id + "-" + index} control={<Radio />} label={alternative.text} />
            );
        };
            setQuizQuestionRender(
                <form onSubmit={handleSubmit}>
                    <FormLabel id="question">{currentQuizQuestion.question}</FormLabel>
                    <RadioGroup
                        aria-labelledby="question"
                        name="radio-buttons-group"
                        onChange={handleChange}
                    >

                        {radioButtons}

                    </RadioGroup>
                    <Button type="submit" variant="contained">Besvar spørsmål</Button>
                </form>
            );
    };





return (<>
    {quizQuestionRender}
        {quizStatus}
        
        
    </>
    );

}
