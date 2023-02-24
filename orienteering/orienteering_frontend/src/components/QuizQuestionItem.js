import { useRadioGroup } from '@mui/material/RadioGroup';
import { Radio, FormControl, FormLabel, RadioGroup, FormControlLabel, Select, MenuItem } from '@mui/material';
import React, { useEffect, useState } from 'react';
import QuizQuestionAlternative from './QuizQuestionAlternative'
//Kilder: https://mui.com/material-ui/react-radio-button/ (17.02.2023)


export default function QuizQuestionItem(props) {

    const [alternatives, setAlternatives] = useState(false);
    const handleChange = (event) => {
        //setCheckpointInfo({ ...checkpointInfo, [event.target.name]: event.target.value });

    };
    const alternativeList = props.alternativeList;

    useEffect(() => {
        //console.log("gjort")
        //console.log(props.id)
        setAlternatives(alternativeList.map((alternative, index) =>
            <QuizQuestionAlternative data={alternative}></QuizQuestionAlternative>


        ));
    }, []);

    return <>
         
        <div> {alternatives} </div>

    </>

}