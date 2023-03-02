import { TextField, FormGroup, Box, Button } from "@mui/material";
import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom"; 
import Grid from '@mui/material/Grid';



export default function AddQuizQuestion(props) {
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

        //update value to render quizquestions
        props.setQuizChanged(props.quizChanged * -1);

    }

    const handleAlternativeChange = (event, i) => {
        //make copy of list
        const newItems = questionInfo.Alternatives.slice();

        //utdate value
        newItems[i].Text = event.target.value;

        //update original list to include the updated value
        setQuestionInfo({ ...questionInfo, Alternatives: newItems });

    }


    const handleChange = (event) => {
        //update state
        setQuestionInfo({ ...questionInfo, [event.target.name]: event.target.value });
    };


    useEffect(() => {

        //load quiz id
        const GetQuizId = async () => {
            const checkpoint = await fetch("/api/checkpoint/getCheckpoint?checkpointId=" + params.checkpointId).then(res => res.json());
            setQuestionInfo({ ...questionInfo, QuizId: checkpoint.quizId });
            console.log(checkpoint);

        }

        GetQuizId();

    },[]);



    return (<>

        <Box
            onSubmit={handleSubmit }
            //kilde. akuratt sx= er fra https://mui.com/material-ui/react-text-field/ 17.02
            component="form"
            sx={{
                '& .MuiTextField-root': { m: 1, width: '25ch' },
            }}>

            <Grid container spacing={6}>

                <Grid item sx={4}>
                    <Button variant="contained" onClick={handleAddAlternative}>Legg til alternativ</Button>
                </Grid>

                <Grid item sx={4}>
                    <Button type="submit" variant="contained">Legg til spørsmål</Button>
                </Grid>

            </Grid>

            <Grid container spacing={3 } >
                <Grid item sx={6 }>
                    <TextField
                        required
                        onChange={(e) => handleChange(e)}
                        id="standard-basic" label="Spørsmål"
                        name="Question"
                        variant="standard"
                        value={questionInfo.Question}
                        />
                </Grid>


                <Grid item sx={6}>

                    <TextField
                        required
                        type='number'
                        onChange={(e) => handleChange(e)}
                        id="standard-basic" label="CorrectAlternative"
                        name="CorrectAlternative"
                        variant="standard"
                        value={questionInfo.CorrectAlternative}
                        InputProps={{ inputProps: { min: 1, max: questionInfo.Alternatives.length } }}
                    />

                </Grid>


            <>
                    {[...Array(count)].map((element, index) => (
                        <Grid item sx={6} key={index + "-" + element}>
                            <TextField
                                
                                required
                                onChange={newVal => handleAlternativeChange(newVal, index)}
                                //onChange={(e) => handleOptionChange(e)}
                                id="standard-basic" label={"Svaralternativ (" + (index+ 1) + ")"}
                                name="Options"
                                variant="standard"
                                value={questionInfo.Alternatives[index].Text}
                            //value={questionInfo.Options}
                            />
                        </Grid>

                    ))
                    }
                    
            </>

            </Grid>

            </Box>

    </>);
}


