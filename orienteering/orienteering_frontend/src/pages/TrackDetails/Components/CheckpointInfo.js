import { React, useState } from "react";
import { Button, Box } from '@mui/material';
import { useNavigate } from 'react-router-dom';

export default function CheckpointInfo(props) {

    const navigate = useNavigate();
    const [editing, setEditing] = useState(false);
    //fix endre navn på dette
    const [oldTitle, setOldTitle] = useState(props.checkpointInfo.title);

    //display spesific track
    const showCheckpoint = () => {
        //fix- skal det være track/id/checkpoint/id??

        const url = "/checkpointdetails/" + props.checkpointInfo.id;
        console.log(url);
        navigate(url);

    }

    const showNavigation = () => {
        //fix- skal det være track/id/checkpoint/id??

        const url = "/navigationEdit/" + props.checkpointInfo.id;
        navigate(url);

    }

    const deleteCheckpoint = async () => {
        const url = '/api/checkpoint/removeCheckpoint?';
        const parameter = 'checkpointId=' + props.checkpointInfo.id;
        const response = await fetch(url + parameter, { method: 'DELETE' });
        //401 => not signed in
        if (response.status == 401) { navigate("/login"); }
        //404 => dont exist or not your checkpoint
        if (response.status == 404) {navigate("/errorpage") }

        props.updateCheckpointList()
    }

    const shouldEdit = () => {
        setEditing(true)
    }

    const stopEdit = async () => {
        setEditing(false)
        //fix put/patch metord

        const url = '/api/checkpoint/editCheckpointTitle?';
        const parameter = 'checkpointTitle=' + oldTitle + "&checkpointId=" + props.checkpointInfo.id;
        const response = await fetch(url + parameter, { method: 'PUT' });
        //401 => not signed in
        if (response.status == 401) { navigate("/login"); }
        //404 => dont exist or not your checkpoint
        if (response.status == 404) { navigate("/errorpage") }

        props.updateCheckpointList()
    }

    const handleChange = (e) => {
        setOldTitle(e.target.value);
    }

    return (<>
        <Box border="1px solid lightblue;" margin="2px;" style={{ width: '80%' }}>

            <p style={{ display: "inline" }}>Title:</p>
            {editing ?
                (
                    <input
                        style={{ display: "inline" }}
                        type="text"
                        value={oldTitle}
                        onChange={handleChange}
                        onBlur={stopEdit}
                    >
                    </input>
                ) : (

                    <span
                        onDoubleClick={shouldEdit}
                    > {props.checkpointInfo.title}</span>)
            }

            <p>Type: {props.checkpointInfo.quizId == null ? "Game" : "Quiz"}

            </p>

            <Button onClick={showCheckpoint}>Show details</Button>
            <Button onClick={deleteCheckpoint}>Delete checkpoint</Button>
            <Button onClick={showNavigation}>Show navigation</Button>

        </Box>
    </>);
}
