import { TextField, Button, FormControl, FormLabel, RadioGroup } from '@mui/material';
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

    function updateAnswer() {
        console.log("svar oppdatert");
    }

    async function fetchQuiz() {
        var url = "/api/quiz/getQuiz?quizId=" + trackInfo.Id;
        var quiz = await fetch(url).then(res => res.json());
        setQuizQuestionList(quiz.quizQuestions);
        console.log(quiz.quizQuestions.alternative);
    }



    return (<>
        <FormControl>
            <FormLabel id="question">Gender</FormLabel>
            <RadioGroup
                aria-labelledby="question"
                name="radio-buttons-group"
            >
                <QuizQuestionItem onChange={updateAnswer} alternativeList={quizQuestionList[currentQuizQuestionIndex].alternative}>
                </QuizQuestionItem>
            </RadioGroup>
        </FormControl>
    </>
    );

}
