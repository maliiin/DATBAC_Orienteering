import { TextField, Button, FormControl, FormLabel, RadioGroup, FormControlLabel, Radio } from '@mui/material';
import React, { useState, useRef } from "react";
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
export default function QuizPage() {
    //kan være greit å få inn checkpointid slik at det senere blir mulig å registrere at noen har vært på checkpointet, lagre score osv...
    const chosenAlternative = useRef("");
    const params = useParams();
    const [quizQuestionRender, setQuizQuestionRender] = useState("");
    const [currentQuizQuestion, setCurrentQuizQuestion] = useState("");
    //console.log("start av side");
    const [quizQuestionIndex, setQuizQuestionIndex] = useState(0);
    const [quizStatus, setQuizStatus] = useState("");
    const [currentAnswer, setCurrentAnswer] = useState("hei");

    const [quizInfo, setQuizInfo] = useState({
        Id: params.quizId
    });

    var testAnswer = "";


    //useEffect(() => {
    //    setQuizInfo(prevState => { return { ...prevState, Id: params.quizId } });
    //    // fetchQuizQuestion();
    //}, []);


    useEffect(() => {
        fetchQuizQuestion();
    }, [quizQuestionIndex]);

    useEffect(() => {
        showQuizQuestion();
    }, [currentQuizQuestion]);


    async function fetchQuizQuestion() {
        var url = "/api/quiz/getNextQuizQuestion?quizId=" + quizInfo.Id + "&quizQuestionIndex=" + quizQuestionIndex.toString();
        var quizQuestion = await fetch(url).then(res => res.json());
        setCurrentQuizQuestion(quizQuestion);
    };

    function handleChange(event) {
        //event.preventDefault();
        console.log("endret radiovalg");
        //console.log(event.target.value);
        console.log(currentAnswer);
        chosenAlternative.current = event.target.value;
        
        //setCurrentAnswer("hahhahaha");
        const test = event.target.value;
        console.log(""+test);

        setCurrentAnswer(test);
        testAnswer = test;

        //console.log(chosenAlternative.current);

    };

    async function handleSubmit(event) {
        //console.log("her");

        event.preventDefault();
        var url = "/api/quiz/getSolution?quizId=" + quizInfo.Id + "&quizQuestionId=" + currentQuizQuestion.quizQuestionId;
        var solution = await fetch(url).then(res => res.text());

        //if (chosenAlternative.current == solution) {
        if (currentAnswer == solution) {

            setQuizStatus(<p>Riktig svar</p>)
        }
        else {
            setQuizStatus(<p>Feil svar. Riktig svar var: {solution}</p>)
        };

        //restart chosenAlternative

        //setCurrentAnswer("");

        //chosenAlternative.current = "";
        // console.log("her");



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
                <FormControlLabel
                    //checked={chosenAlternative.current == alternative.text}
                    value={alternative.text}
                    key={alternative.id + "-" + index}
                    control={<Radio required={true} />}
                    label={testAnswer == alternative.text ? 'lik' : 'ulik'}
                    checked={testAnswer == alternative.text}
                />
            );
        };


        setQuizQuestionRender(
            <form onSubmit={handleSubmit}>
                <FormLabel id="question"><h3>{currentQuizQuestion.question}</h3></FormLabel>
                <RadioGroup
                    aria-labelledby="question"
                    name="radio-buttons-group"
                    onChange={handleChange}
                    //value={chosenAlternative.current}
                    value={testAnswer}
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
