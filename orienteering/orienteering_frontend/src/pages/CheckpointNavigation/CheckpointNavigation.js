import { TextField, Button, FormControl, FormLabel, RadioGroup, FormControlLabel, Radio } from '@mui/material';
import React, { useState, useRef } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
export default function CheckpointNavigation() {
    //kan være greit å få inn checkpointid slik at det senere blir mulig å registrere at noen har vært på checkpointet, lagre score osv...

    const params = useParams();


    const [quizInfo, setQuizInfo] = useState({
        Id: params.checkpointId
    });


    useEffect(() => {
        
        // fetchQuizQuestion();
    }, []);


    





    return (<>
        <p>CheckpointNavigation</p>

    </>
    );

}
