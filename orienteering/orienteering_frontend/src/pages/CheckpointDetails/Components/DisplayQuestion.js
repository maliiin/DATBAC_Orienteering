import { Button, Box } from '@mui/material';
import React from "react";
import { useNavigate } from 'react-router-dom';

export default function DisplayQuestion(props) {
    const navigate = useNavigate();

    const deleteQuestion = async () => {
        const url = '/api/quiz/deleteQuestion?';
        const parameter = 'questionId=' + props.questionInfo.quizQuestionId + '&quizId=' + props.quizId;
        const response = await fetch(url + parameter, { method: 'DELETE' });
        //401 => not signed in
        if (response.status == 401) { navigate("/login"); }
        //404 => dont exist or not your checkpoint
        if (response.status == 404) { navigate("/errorpage") }

        //update value to render quizquestions
        props.setQuizChanged(props.quizChanged * -1);
    }

    return (
        <Box border="1px solid lightblue;" margin="2px;" style={{ width: '80%' }}>

            <p>Question: {props.questionInfo.question}</p>
            <p>Alternatives:</p>

            {props.questionInfo.alternatives.map((alternative, index) =>

                <p
                    key={index + "-" + alternative.text}
                    style={{
                        backgroundColor: props.questionInfo.correctAlternative - 1 == index ? "lightGreen" : "pink"
                    }}>
                    {alternative.text}
                </p>
            )}

            <Button onClick={deleteQuestion}>Delete question</Button>

        </Box>
    );
}