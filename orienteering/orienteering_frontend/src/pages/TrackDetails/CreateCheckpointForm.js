import { React, useState, useEffect } from "react";
import { Radio, FormLabel, Box, RadioGroup, FormControlLabel, Select, MenuItem, Button, TextField } from '@mui/material';
import FormControl from '@mui/material/FormControl';


export default function CreateCheckpointForm(props) {
    //the chosen activity
    const [activity, setActivity] = useState("");

    const [checkpointInfo, setCheckpointInfo] = useState({
        Title: "",
        TrackId: "",
        GameId: 1
    });


    //set userId from props
    useEffect(() => {
        setCheckpointInfo(prevState => { return { ...prevState, TrackId: props.trackId } });

    }, [props.TrackId]);


    const handleSubmit = async (event) => {
        event.preventDefault();
        let copiedCheckpoint = JSON.parse(JSON.stringify(checkpointInfo));

        if (activity == "quiz") {
            //quiz, so gameId is 0
            copiedCheckpoint.GameId = 0;
        }

        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            },
            body: JSON.stringify(copiedCheckpoint)
        };

        const response = await fetch('/api/checkpoint/createCheckpoint', requestOptions);

        //update checkpointlist of parent
        props.updateCheckpointList();

        //clear text field and game
        setCheckpointInfo(
            {
                ...checkpointInfo,
                Title: "",
                GameId: 1
            });

        //clear radio
        setActivity("");

        return response;
    };

    const handleChange = (event) => {
        //update state
        setCheckpointInfo({ ...checkpointInfo, [event.target.name]: event.target.value });
    };

    const changeActivity = (event) => {

        setActivity(event.target.value);
    };

    return (
        <>
            <h4>Legg til ny post</h4>

            <Box onSubmit={handleSubmit} component="form">
                <TextField
                    required
                    onChange={(e) => handleChange(e)}
                    id="standard-basic" label="Tittel"
                    name="Title"
                    variant="standard"
                    value={checkpointInfo.Title} />

                <br></br>
                <br></br>


                <FormLabel id="radio-buttons-group">Velg aktivitet</FormLabel>

                <RadioGroup
                    value={activity}
                    aria-labelledby="radio-buttons-group"
                    name="radio-buttons-group"
                    row
                    onChange={(e) => changeActivity(e)}
                >

                    <FormControlLabel
                        value="spill"
                        label="Spill"
                        control={<Radio required={true} />}
                        checked={activity == "spill"}
                    />

                    <FormControlLabel
                        value="quiz"
                        control={<Radio required={true} />}
                        label="Quiz"
                        checked={activity == "quiz"}
                    />

                </RadioGroup>

                <br></br>

                <FormControl //required={showForm ? true : false}
                >
                    <FormLabel
                        style={activity == "spill" ? {} : { display: 'none' }}
                        id="radio-buttons-group"
                    >
                        Velg spill
                    </FormLabel>

                    <Select
                        style={activity == "spill" ? {} : { display: 'none' }}
                        sx={{ m: 1, minWidth: 120 }}
                        size="small"
                        labelId="select-label"
                        id="select"
                        onChange={(e) => handleChange(e)}
                        name="GameId"
                        value={checkpointInfo.GameId}
                        defaultValue={1}
                    >

                        <MenuItem value={1}>
                            Spill11
                        </MenuItem>

                        <MenuItem value={2} >
                            Spill2
                        </MenuItem>

                        <MenuItem value={3} >
                            Spill3
                        </MenuItem>

                    </Select>
                </FormControl>

                <br></br>

                <Button type="submit">Lag post</Button>
            </Box>
        </>);
}


