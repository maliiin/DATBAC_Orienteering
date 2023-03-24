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
    const navigate = useNavigate();

    const fetchQuiz = async () => {
        const response = await fetch("/api/quiz/getQuiz?quizId=" + props.quizId);
        if (!response.status.ok) {
            navigate("/errorpage");
        }
        const Quiz = await response.json();
        setQuizQuestions(Quiz.quizQuestions);
        console.log(Quiz.quizQuestions);
    }

    useEffect(() => {
        fetchQuiz();
    }, [props.quizChanged])

    if (quizQuestions.length > 0) {
        return (<>
            {
                quizQuestions.map((quizQuestion, index) =>
                    <DisplayQuestion
                        quizChanged={props.quizChanged}
                        setQuizChanged={props.setQuizChanged}
                        key={index + "-" + quizQuestion.quizQuestionId}
                        questionInfo={quizQuestion}
                        quizId={props.quizId}>
                    </DisplayQuestion>
                )
            }

        </>
        );
    } else {
        return <p>Her kommer spørsmålene du lager.</p>;
    }
}

//fiks reaktiv ved legging til av spørsmål.
//som i de andre to filene.