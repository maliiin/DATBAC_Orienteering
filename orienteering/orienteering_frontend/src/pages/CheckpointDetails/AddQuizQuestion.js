import { TextField, FormGroup, Box, Button } from "@mui/material";
import React, { useState } from "react";
//<TextField
//    required
//    //onChange={(e) => handleChange(e)}
//    //id="standard-basic" label="Username"
//    //name="username"
//    variant="standard"
//    value={questionInfo.username}
///>

function AddQuizQuestion() {
    const [questionInfo, setQuestionInfo] = useState({
        Question: "",
        Options: [],
        CorrectOption: 2

    });

    const [count, setCount] = useState(2);

    const handleAddOption = (event) => {
        //event.PreventDefault();
        setCount(count + 1);

    }

    const handleChange = (event) => {
        console.log("change");
        //update state
        setQuestionInfo({ ...questionInfo, [event.target.name]: event.target.value });
    };

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
                name="question"
                variant="standard"
                value={questionInfo.question}
            />

            <TextField
                required
                type="number"
                onChange={(e) => handleChange(e)}
                id="standard-basic" label="CorrectOption"
                name="CorrectOption"
                variant="standard"
                value={questionInfo.CorrectOption}
            />
            <TextField
                required
                onChange={(e) => handleChange(e)}
                id="standard-basic" label="Spørsmål"
                name="Options"
                variant="standard"
                value={questionInfo.Options}
            />
            <>
                {[...Array(count)].map((element, index) => <p key={index }>heiheihei</p>) }
            </>

                <Button variant="contained" onClick={handleAddOption}>Legg til alternativ</Button>

        </Box>

        <p>her kan du legge til spørsmål</p>her kan du legge til spørsmål
    </>);
}

export default AddQuizQuestion;
