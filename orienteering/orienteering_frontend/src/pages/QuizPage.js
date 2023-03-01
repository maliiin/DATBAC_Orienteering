import { TextField, Button, FormControl, FormLabel, RadioGroup, FormControlLabel, Radio } from '@mui/material';
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
    const [chosenAlternative, setChosenAlternative] = useState("");
    const [currentQuizQuestionIndex, setCurrentQuizQuestionIndex] = useState(0);

    const [trackInfo, setTrackInfo] = useState({
        Id: params.quizId
    });


    useEffect(() => {
        setTrackInfo(prevState => { return { ...prevState, Id: params.quizId } });
        fetchQuiz();
    }, []);

    useEffect(() => {
        showQuizQuestion();
    }, [currentQuizQuestionIndex, quizQuestionList]);


    async function fetchQuiz() {
        var url = "/api/quiz/getQuiz?quizId=" + trackInfo.Id;
        var quiz = await fetch(url).then(res => res.json());
        setQuizQuestionList(quiz.quizQuestions);
    }

    function handleChange(event) {
        console.log("endret radiovalg");
        console.log(event.target.value);
        setChosenAlternative(event.target.value);
    }

    function handleSubmit(event) {
        event.preventDefault();
        if (currentQuizQuestionIndex + 1 < quizQuestionList.length) {
            var newIndex = currentQuizQuestionIndex + 1;
            setCurrentQuizQuestionIndex(newIndex);
        }
        else {
            console.log("Alle spørsmål besvart");
        }
        console.log(currentQuizQuestionIndex);
    }

    function showQuizQuestion() {
        if (quizQuestionList != "") {
            if (typeof quizQuestionList[currentQuizQuestionIndex].alternative != 'undefined') {
                var currentAlternatives = quizQuestionList[currentQuizQuestionIndex].alternative;
                setCurrentQuizQuestion(
                    <form onSubmit={handleSubmit}>
                        <FormLabel id="question">{quizQuestionList[currentQuizQuestionIndex].question}</FormLabel>
                        <RadioGroup
                            aria-labelledby="question"
                            name="radio-buttons-group"
                            onChange={handleChange}
                        >
                            
                            <QuizQuestionItem alternativeList={currentAlternatives}>
                            </QuizQuestionItem>

                        </RadioGroup>
                        <Button type="submit" variant="contained">Besvar spørsmål</Button>
                    </form>
                );
            }
        }
    }



    return (<>
        {currentQuizQuestion}
        
        
    </>
    );

}
