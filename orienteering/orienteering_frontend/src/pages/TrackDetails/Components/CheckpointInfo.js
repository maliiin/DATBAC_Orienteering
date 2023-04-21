import { React, useState } from "react";
import { Button, Box } from '@mui/material';
import { useNavigate } from 'react-router-dom';

export default function CheckpointInfo(props) {

    const navigate = useNavigate();
    const [editingTitle, setEditingTitle] = useState(false);
    const [editingDescription, setEditingDescription] = useState(false);
    const [oldTitle, setOldTitle] = useState(props.checkpointInfo.title);
    const [oldDescription, setOldDescription] = useState(props.checkpointInfo.title);

    //display spesific track
    const showCheckpoint = () => {
        const url = "/checkpointdetails/" + props.checkpointInfo.id;
        navigate(url);
    }

    const showNavigation = () => {
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
        if (response.status == 404) { navigate("/errorpage") }

        props.updateCheckpointList()
    }

    const shouldEditTitle = () => {
        setEditingTitle(true)
    }
    const shouldEditDescription = () => {
        setEditingDescription(true)
    }

    const stopEditTitle = async () => {
        setEditingTitle(false)

        const url = '/api/checkpoint/editCheckpointTitle';
        const response = await fetch(url, {
            method: "PATCH",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                CheckpointId: props.checkpointInfo.id,
                Title: oldTitle
            })
        });
        //401 => not signed in
        if (response.status == 401) { navigate("/login"); }
        //404 => dont exist or not your checkpoint
        if (response.status == 404) { navigate("/errorpage") }

        props.updateCheckpointList()
    }

    const stopEditDescription = async () => {
        setEditingTitle(false)

        const url = '/api/checkpoint/editCheckpointDescription';
        const response = await fetch(url, {
            method: "PATCH",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                CheckpointId: props.checkpointInfo.id,
                Description: oldDescription
            })
        });
        //401 => not signed in
        if (response.status == 401) { navigate("/login"); }
        //404 => dont exist or not your checkpoint
        if (response.status == 404) { navigate("/errorpage") }

        props.updateCheckpointList()
    }

    const handleChangeTitle = (e) => {
        setOldTitle(e.target.value);
    }

    const handleChangeDescription = (e) => {
        setOldDescription(e.target.value);
    }
    

    return (<>
        <Box border="1px solid lightblue;" margin="2px;" style={{ width: '80%' }}>

            <p style={{ display: "inline" }}>Title:</p>
            {editingTitle ?
                (<input
                    style={{ display: "inline" }}
                    type="text"
                    value={oldTitle}
                    onChange={handleChangeTitle}
                    onBlur={stopEditTitle}>
                </input>
                ) : (
                    <span onDoubleClick={shouldEditTitle}>
                        {props.checkpointInfo.title}
                    </span>)
            }

            <p>Type: {props.checkpointInfo.quizId == null ? "Game" : "Quiz"}
            </p>

            <p style={{ display: "inline" }}>
                Description: 
            </p>
            {editingDescription ?
                (<input
                    style={{ display: "inline" }}
                    type="text"
                    value={oldDescription}
                    onChange={handleChangeDescription}
                    onBlur={stopEditDescription}>
                </input>
                ) : (
                    <span onDoubleClick={shouldEditDescription}>
                        {props.checkpointInfo.checkpointDescription}
                    </span>)
            }

            <Button onClick={showCheckpoint}>Show details</Button>
            <Button onClick={deleteCheckpoint}>Delete checkpoint</Button>
            <Button onClick={showNavigation}>Show navigation</Button>

        </Box>
    </>);
}
