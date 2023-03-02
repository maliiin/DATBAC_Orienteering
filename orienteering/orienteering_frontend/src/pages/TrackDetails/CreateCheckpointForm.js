import { React, useState, useEffect } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { useRadioGroup } from '@mui/material/RadioGroup';
import { Radio, FormLabel, Box, RadioGroup, FormControlLabel, Select, MenuItem, Button, TextField } from '@mui/material';
import FormControl from '@mui/material/FormControl';
import FormHelperText from '@mui/material/FormHelperText';

//some info of track, not details

export default function CreateCheckpointForm(props) {
    //props is props.Trackinfo (id, name, userid ...)
    const [showForm, setShowForm] = useState(false);

    const [checkpointInfo, setCheckpointInfo] = useState({
        Title: "",
        TrackId: "",
        GameId: 1//endret fra 0
    });


    //set userId from props
    useEffect(() => {
        setCheckpointInfo(prevState => { return { ...prevState, TrackId: props.trackId } });
        
    }, [props.TrackId]);


    const handleSubmit = async (event) => {
        //convert GameId to 0 if quiz is selected.

        //it is quiz, set gameId to 0
        
        let copiedCheckpoint = JSON.parse(JSON.stringify(checkpointInfo));

        if (showForm == false) {
            copiedCheckpoint.GameId = 0;
        }


        event.preventDefault();

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

        return response;


    };


    const changeTitle = (event) => {
        //update state
        setCheckpointInfo({ ...checkpointInfo, [event.target.name]: event.target.value });
    };

    const changeGame = (event) => {
        //update state
        setCheckpointInfo({ ...checkpointInfo, [event.target.name]: event.target.value });
    };

    const changeActivity = (event) => {

        if (event.target.value === "spill") {
            setShowForm(true);
        }
        else {
            setShowForm(false);
        }
    };




    return (
        <>
            <Box onSubmit={handleSubmit} component="form">
                <TextField
                    required
                    onChange={(e) => changeTitle(e)}
                    id="standard-basic" label="Tittel"
                    name="Title"
                    variant="standard"
                    value={checkpointInfo.Title} />

                <br></br>

                <FormLabel id="radio-buttons-group">Velg aktivitet</FormLabel>

                <RadioGroup
                    
                    aria-labelledby="radio-buttons-group"
                    name="radio-buttons-group"
                    row
                    onChange={(e) => changeActivity(e)}
                >

                    <FormControlLabel
                        value="spill"
                        label="Spill"
                        control={<Radio required={true} />}
                    />

                    <FormControlLabel
                        value="quiz"
                        control={<Radio required={true} />}
                        label="Quiz"
                    />

                </RadioGroup>


                <FormControl required={showForm ? true :false}>
                    <FormLabel style={showForm ? {} : { display: 'none' }} id="radio-buttons-group">Velg spill</FormLabel>

                    <Select
                        style={showForm ? {} : { display: 'none' }}
                        sx={{ m: 1, minWidth: 120 }}
                        size="small"
                        labelId="select-label"
                        id="select"
                        onChange={changeGame}
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


