
import { TextField, Box, Button, FormControl, FormLabel, RadioGroup, FormControlLabel, Radio } from '@mui/material';
import React, { useState, useRef } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";


export default function QuizPage2() {

    const navigate = useNavigate();
    const params = useParams();

    //what the user answers
    const [guess, setGuess] = useState("");

    const changeGuess = (event) => {

        setGuess(event.target.value);
    };

    async function handleSubmit(event) {
        console.log("not implemented")
    }

    return (<>
        <Box onSubmit={handleSubmit} component="form">
            <RadioGroup
                value={guess}
                aria-labelledby="radio-buttons-group"
                name="radio-buttons-group"
                row
                onChange={(e) => changeGuess(e)}
            >

                <FormControlLabel
                    value="spill"
                    label="Spill"
                    control={<Radio required={true} />}
                    checked={guess == "spill"}
                />

                <FormControlLabel
                    value="quiz"
                    control={<Radio required={true} />}
                    label="Quiz"
                    checked={guess == "quiz"}
                />

            </RadioGroup>
            <Button type="submit" variant="contained">Besvar spørsmål</Button>
        </Box>

    </>);
}