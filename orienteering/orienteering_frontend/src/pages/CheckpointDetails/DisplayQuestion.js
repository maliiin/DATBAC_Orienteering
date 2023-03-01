
import { TextField, Button, Box } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import AccessAlarmIcon from '@mui/icons-material/AccessAlarm';
import DeleteIcon from '@mui/icons-material/Delete';


export default function DisplayQuestion(props) {

    const deleteQuestion = async() => {
        console.log("g");
        await fetch('/api/quiz/deleteQuestion?questionId=' + props.questionInfo.quizQuestionId+'&quizId='+props.quizId, { method: 'DELETE' });

    }

    return (
        <Box border="1px solid lightblue;" margin="2px;">

            <p>Sporsmal: {props.questionInfo.question}</p>
            <p>Svaralternativer:</p>
            
            {
                props.questionInfo.alternative.map((alternative, index) =>

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