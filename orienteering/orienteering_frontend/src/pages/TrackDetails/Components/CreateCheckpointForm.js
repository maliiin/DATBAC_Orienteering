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
            <h4>Add new checkpoint</h4>

            <Box onSubmit={handleSubmit} component="form">
                <TextField
                    required
                    onChange={(e) => handleChange(e)}
                    id="standard-basic" label="Title"
                    name="Title"
                    variant="standard"
                    value={checkpointInfo.Title} />

                <br></br>
                <br></br>


                <FormLabel id="radio-buttons-group">Choose activity</FormLabel>

                <RadioGroup
                    value={activity}
                    aria-labelledby="radio-buttons-group"
                    name="radio-buttons-group"
                    row
                    onChange={(e) => changeActivity(e)}
                >

                    <FormControlLabel
                        value="spill"
                        label="Game"
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
                        Choose game
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
                            FallingBoxes
                        </MenuItem>

                        <MenuItem value={2} >
                            Chemistry
                        </MenuItem>

                        <MenuItem value={3} >
                            LogicGates
                        </MenuItem>

                    </Select>
                </FormControl>

                <br></br>

                <Button type="submit">Create checkpoint</Button>
            </Box>
        </>);
}


