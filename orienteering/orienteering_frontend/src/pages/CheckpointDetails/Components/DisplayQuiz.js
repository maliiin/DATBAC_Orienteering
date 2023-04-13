import React, { useState } from "react";
import { useEffect } from "react";
import DisplayQuestion from "./DisplayQuestion";

export default function DisplayQuiz(props) {
    const [quizQuestions, setQuizQuestions] = useState("");

    const fetchQuiz = async () => {
        const response = await fetch("/api/quiz/getQuiz?quizId=" + props.quizId);
        //fix dobbelsjekk-denne er ikke sikret siden taquiz bruker denne også

        const Quiz = await response.json();
        setQuizQuestions(Quiz.quizQuestions);
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
        return <p>Here comes the quiestions you make</p>;
    }
}
