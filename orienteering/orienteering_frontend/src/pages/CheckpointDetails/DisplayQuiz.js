import useAuthentication from "../../hooks/useAuthentication";
import { TextField, Button } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import AddQuizQuestion from "./AddQuizQuestion";
import DisplayQuestion from "./DisplayQuestion";
//

export default function DisplayQuiz(props) {
    const [quizQuestions, setQuizQuestions] = useState("");

    useEffect(() => {
        const fetchQuiz = async () => {
            const Quiz = await fetch("/api/quiz/getQuiz?quizId=" + props.quizId).then(res => res.json());
            setQuizQuestions(Quiz.quizQuestions);
            console.log(Quiz.quizQuestions);
        }

        fetchQuiz();
    },[])


    if (quizQuestions.length > 0) {
        return (<>
            {

                //fix- må ha sjekk som sjekker om det finnes spørsmål, hvis
                //ikke klikker den

                quizQuestions.map((quizQuestion, index) =>
                    <DisplayQuestion key={index + "-" + quizQuestion.quizQuestionId} questionInfo={quizQuestion}></DisplayQuestion>
                )
            }

        </>
        );
    } else {
        return <p>hehhehe</p>;
    }
}