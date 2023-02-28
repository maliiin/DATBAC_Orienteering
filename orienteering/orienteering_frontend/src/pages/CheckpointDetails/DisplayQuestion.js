
import useAuthentication from "../../hooks/useAuthentication";
import { TextField, Button, Box } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import AccessAlarmIcon from '@mui/icons-material/AccessAlarm';
import DeleteIcon from '@mui/icons-material/Delete';


export default function DisplayQuestion(props) {

    return (
        <div>

            <p>Sporsmal: {props.questionInfo.question}</p>
            
            {
            props.questionInfo.alternative.map((alternative, index) =>
                
                    
                <Box key={index + "-" + alternative.text}>
                    
                        <p>{alternative.text}</p>
                    </Box>

                
                )
            }
            
        </div>
    );
}