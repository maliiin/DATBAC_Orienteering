import { React, useState, useEffect } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { useRadioGroup } from '@mui/material/RadioGroup';
import { Radio, FormLabel, RadioGroup, FormControlLabel, Select, MenuItem, Button, TextField } from '@mui/material';


//some info of track, not details

export default function CreateCheckpointForm(props) {
    //props is props.Trackinfo (id, name, userid ...)
    const [showForm, setShowForm] = useState(false);

    const [checkpointInfo, setCheckpointInfo] = useState({
        Title: "",
        TrackId: "",
        GameId: 0
    });


    //set userId from props
    useEffect(() => {
        setCheckpointInfo(prevState => { return { ...prevState, TrackId: props.trackId } });
        //console.log("gjort")
        //console.log(props.id)
    }, [props.TrackId]);


    const handleSubmit = async (event) => {
        //setCheckpointInfo(prevState => { return { ...prevState, UserId: props.trackId } })
        
        event.preventDefault();

        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            },
            body: JSON.stringify(checkpointInfo)
        };

        const response = await fetch('/api/checkpoint/createCheckpoint', requestOptions);
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
        //setCheckpointInfo({ ...checkpointInfo, [event.target.name]: event.target.value });
        if (event.target.value === "spill") {
            setShowForm(true);
        }
        else {
            setShowForm(false);
        }
    };




    return (<>
        <form onSubmit={handleSubmit}>
            <TextField
                required
                onChange={(e) => changeTitle(e)}
                id="standard-basic" label="Tittel"
                name="Title"
                variant="standard"
                value={checkpointInfo.Title}
            />
            <br></br>
            <FormLabel id="radio-buttons-group">Velg aktivitet</FormLabel>
            <RadioGroup
                aria-labelledby="radio-buttons-group"
                name="radio-buttons-group"
                row
                onChange={(e) => changeActivity(e)}
            >
                <FormControlLabel value="spill" control={<Radio />} label="Spill" />
                <FormControlLabel value="quiz" control={<Radio />} label="Quiz" />
            </RadioGroup>
            <FormLabel style={showForm ? {} : { display: 'none' }} id="radio-buttons-group">Velg spill</FormLabel>
            <Select style={showForm ? {} : { display: 'none' }} sx={{ m: 1, minWidth: 120 }} size="small"
                labelId="select-label"
                id="select"
                onChange={changeGame}
                name="GameId"
                value={checkpointInfo.GameId}
            >
                <MenuItem value={1}>Spill1</MenuItem>
                <MenuItem value={2}>Spill2</MenuItem>
                <MenuItem value={3}>Spill3</MenuItem>
            </Select>
            <br></br>
            <Button type="submit">Lag post</Button>
        </form>
    </>);
}


