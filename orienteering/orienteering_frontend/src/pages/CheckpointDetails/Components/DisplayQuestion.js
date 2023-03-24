
import { TextField, Button, Box } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';



export default function DisplayQuestion(props) {
   
    const deleteQuestion = async () => {
        const url = '/api/quiz/deleteQuestion?';
        const parameter= 'questionId=' + props.questionInfo.quizQuestionId + '&quizId=' + props.quizId;
        await fetch(url + parameter, { method: 'DELETE' });

        //update value to render quizquestions
        props.setQuizChanged(props.quizChanged * -1);
        

    }

    return (
        <Box border="1px solid lightblue;" margin="2px;" style={{width:'80%'} }>

            <p>Sporsmal: {props.questionInfo.question}</p>
            <p>Svaralternativer:</p>
            
            {
                props.questionInfo.alternatives.map((alternative, index) =>

                    <p
                        key={index + "-" + alternative.text}
                        style={{ backgroundColor: props.questionInfo.correctAlternative - 1 == index ? "lightGreen" : "pink" }}
                    >  
                        
                        {alternative.text}

                    </p>                
                )
            }
            <Button onClick={deleteQuestion }>Slett spørsmål</Button>
            
        </Box>
    );
}