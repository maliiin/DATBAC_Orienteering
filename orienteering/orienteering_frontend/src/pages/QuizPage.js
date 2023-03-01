import { TextField, Button } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import QuizQuestionItem from '../components/QuizQuestionItem';
export default function QuizPage() {
    //kan være greit å få inn checkpointid slik at det senere blir mulig å registrere at noen har vært på checkpointet, lagre score osv...

    const params = useParams();


    const [quizQuestionList, setQuizQuestionList] = useState("");
    const [currentQuizQuestion, setCurrentQuizQuestion] = useState("");

    const [trackInfo, setTrackInfo] = useState({
        Id: params.quizId
    });

    const currentQuizQuestionIndex = 0;

    useEffect(() => {
        setTrackInfo(prevState => { return { ...prevState, Id: params.quizId } });
        fetchQuiz();
    }, []);

    useEffect(() => {
        if (quizQuestionList != "") {
            showNextQuizQuestion();
        }
    }, [currentQuizQuestionIndex, quizQuestionList]);

    function updateAnswer() {
        console.log("svar oppdatert");
    }

    async function fetchQuiz() {
        //var url = "/api/checkpoint/getCheckpoint?checkpointId=" + params.checkpointId;
        //var checkpoint = await fetch(url).then(res => res.json());
        //var quizId = checkpoint.quizId;
        //if (quizId == null) {
        //    // fiks: få feil og send til errorpage
        //    console.log("error");
        //}
        var url = "/api/quiz/getQuiz?quizId=" + trackInfo.Id;
        var quiz = await fetch(url).then(res => res.json());
        var test1 = "test1";
        setQuizQuestionList(quiz.quizQuestions);
        console.log(quiz.quizQuestions.alternative);

        //setQuizQuestionList(quizQuestions.map((quizQuestion, index) =>
        //    <QuizQuestionItem onChange={updateAnswer} key={quizQuestion.QuizQuestionId + "-" + index} alternativeList={quizQuestion.Alternative}>
        //    </QuizQuestionItem>

        //));
    }

    function showNextQuizQuestion() {
        var quizQuestion = quizQuestionList[currentQuizQuestionIndex];
        setCurrentQuizQuestion(
            <QuizQuestionItem onChange={updateAnswer} alternativeList={quizQuestion.alternative}>
            </QuizQuestionItem>)
    }


    return <>
        {currentQuizQuestion}
    </>

}
