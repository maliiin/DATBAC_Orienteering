import { TextField, FormGroup, Box, Button } from "@mui/material";
import React, { useState, useEffect } from "react";
import {useParams} from "react-router-dom"; 


export default function AddQuizQuestion() {
    const params = useParams();
    //const [quizId, setQuizId] = useState("";)
    //const CheckpointId = params.checkpointId;
    console.log(params.checkpointId);
    //fiks hardkodet quizid
    const [questionInfo, setQuestionInfo] = useState({
        Question: "",
        QuizId: "",//"08db1019-cac8-4445-8a85-7195fce75e20",
        Alternatives: [{
            Id: 1,
            Text: ""
        }, {
            Id: 2,
            Text: ""
            }],
        CorrectAlternative: 2

    });

    const [count, setCount] = useState(2);

    //adds one more field for option
    const handleAddAlternative = (event) => {
        //event.PreventDefault();

        //update alternativeslist to contain one more element
        let newItems = questionInfo.Alternatives.slice();
        newItems.push({
            Id: count + 1,
            Text: ""
        });
        setQuestionInfo({ ...questionInfo, Alternatives: newItems });
        setCount(count + 1);


    }

    //adds question to the quiz
    const handleSubmit = async (event) => {
        event.preventDefault();

        
        //var test = await fetch('/api/track/test?k=heihei');//.then(res=>res.json());

        //console.log(test);
        //var q = await test.json();
        //console.log(q);


        const requestAlternatives = {

            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(questionInfo)

        };

        var response = await fetch('/api/quiz/addQuizQuestion', requestAlternatives);
        console.log(response);
        //return false;

    }

    const handleAlternativeChange = (event, i) => {
        //make copy
        const newItems = questionInfo.Alternatives.slice();

        //utdate value
        newItems[i].Text = event.target.value;

        //update original list to include the updated value
        setQuestionInfo({ ...questionInfo, Alternatives: newItems });

    }


    const handleChange = (event) => {
        console.log("change");
        //update state
        setQuestionInfo({ ...questionInfo, [event.target.name]: event.target.value });
    };


    useEffect(() => {

        //load quiz id
        const GetQuizId = async () => {
            const checkpoint = await fetch("/api/checkpoint/getCheckpoint?checkpointId=" + params.checkpointId).then(res => res.json());
            setQuestionInfo({ ...questionInfo, QuizId: checkpoint.QuizId });
            console.log(checkpoint);

        }

        GetQuizId();

    },[]);



    return (<>
        <Box
            //kilde. akuratt sx= er fra https://mui.com/material-ui/react-text-field/ 17.02
            component="form"
            sx={{
                '& .MuiTextField-root': { m: 1, width: '25ch' },
            }}>

            <TextField
                required
                onChange={(e) => handleChange(e)}
                id="standard-basic" label="Spørsmål"
                name="Question"
                variant="standard"
                value={questionInfo.Question}
            />

            <TextField
                required
                type="number"
                onChange={(e) => handleChange(e)}
                id="standard-basic" label="CorrectOption"
                name="CorrectOption"
                variant="standard"
                value={questionInfo.CorrectAlternative}
            />


            <>
                {[...Array(count)].map((element, index) => (
                    <TextField
                        key={index +"-"+ element}
                        required
                        onChange={newVal => handleAlternativeChange(newVal, index)}
                        //onChange={(e) => handleOptionChange(e)}
                        id="standard-basic" label="Svaralternativ"
                        name="Options"
                        variant="standard"
                        value={questionInfo.Alternatives[index].Text}
                    //value={questionInfo.Options}
                    />

                    ))
                    }
                    
            </>


            <Button variant="contained" onClick={handleAddAlternative}>Legg til alternativ</Button>
            <Button type="submit" variant="contained" onClick={handleSubmit}>Legg til spørsmål</Button>

        </Box>

    </>);
}


//<>
//    {[...Array(count)].map((element, index) => <p key={index}>heiheihei</p>)}
//</>
