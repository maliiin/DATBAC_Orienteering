import { TextField, Box, Button } from "@mui/material";
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import Grid from '@mui/material/Grid';

export default function AddQuizQuestion(props) {
    const navigate = useNavigate();

    const [questionInfo, setQuestionInfo] = useState({
        Question: "",
        QuizId: props.quizId,
        Alternatives: [{
            Id: 1,
            Text: ""
        }, {
            Id: 2,
            Text: ""
        }, {
            Id: 3,
            Text: ""
        }],
        CorrectAlternative: ""
    });

    //amount of alternatives
    const [count, setCount] = useState(3);

    //adds one more field for alternative
    const handleAddAlternative = () => {

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
        //401 => not signed in
        if (response.status == 401) { navigate("/login"); }
        //404 => dont exist or not your checkpoint
        if (response.status == 404) { navigate("/errorpage") }

        //update value to render quizquestions
        props.setQuizChanged(props.quizChanged * -1);

        //clear input field
        setQuestionInfo({
            ...questionInfo,
            Question: "",
            Alternatives: [{
                Id: 1,
                Text: ""
            }, {
                Id: 2,
                Text: ""
            }, {
                Id: 3,
                Text: ""
            }],
            CorrectAlternative: ""
        });
        setCount(3);
    }

    const handleAlternativeChange = (event, i) => {
        //make copy of list
        const newItems = questionInfo.Alternatives.slice();

        //utdate value
        newItems[i].Text = event.target.value;

        //update original list to include the updated value
        setQuestionInfo({ ...questionInfo, Alternatives: newItems });
    }

    const handleRemoveAlternative = () => {
        //all exept last alternative
        let shorterList = questionInfo.Alternatives.slice(0, -1);

        setQuestionInfo({ ...questionInfo, Alternatives: shorterList });
        setCount(count - 1);
    }

    const handleChange = (event) => {
        //update state
        setQuestionInfo({ ...questionInfo, [event.target.name]: event.target.value });
    };

    return (<>
        <Box
            onSubmit={handleSubmit}
            //kilde. akuratt sx= er fra https://mui.com/material-ui/react-text-field/ 17.02
            component="form"
            sx={{
                '& .MuiTextField-root': { m: 1, width: '25ch' },
            }}>
            <p>Choose which alternative which should be correct in 'Correct Alternative'.</p>
            <p>Alternative (nr)</p>
            <Grid container spacing={2}>

                <Grid item sx={2}>
                    <Button
                        disabled={count <= 5 ? false : true}
                        variant="contained"
                        onClick={handleAddAlternative}>

                        Add alternative

                    </Button>
                </Grid>

                <Grid item sx={2}>
                    <Button
                        disabled={count >= 3 ? false : true}

                        variant="contained"
                        onClick={handleRemoveAlternative}>

                        Remove last alternative

                    </Button>
                </Grid>


                <Grid item sx={2}>
                    <Button
                        type="submit"
                        variant="contained">
                        add question
                    </Button>
                </Grid>

            </Grid>

            <Grid container spacing={3} >
                <Grid item sx={6}>
                    <TextField
                        required
                        onChange={(e) => handleChange(e)}
                        id="standard-basic" label="Question"
                        name="Question"
                        variant="standard"
                        value={questionInfo.Question} />
                </Grid>


                <Grid item sx={6}>

                    <TextField
                        required
                        type='number'
                        onChange={(e) => handleChange(e)}
                        id="standard-basic" label="Correct alternative (nr)"
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
                                id="standard-basic" label={"Alternative (" + (index + 1) + ")"}
                                name="Options"
                                variant="standard"
                                value={questionInfo.Alternatives[index].Text}
                            />
                        </Grid>
                    ))
                    }
                </>
            </Grid>
        </Box>
    </>);
}


