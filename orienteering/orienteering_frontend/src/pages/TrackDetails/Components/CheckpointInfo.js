import { React, useState } from "react";
import { Button, Box, Grid } from '@mui/material';
import { Link, redirect, useNavigate } from 'react-router-dom';


//overview of singe checkpoint, not details
export default function CheckpointInfo(props) {

    const navigate = useNavigate();
    const [editing, setEditing] = useState(false);
    //fix endre navn på dette
    const [oldTitle, setOldTitle] = useState(props.checkpointInfo.title);

    //display spesific track
    const showCheckpoint = (event) => {
        //fix- skal det være track/id/checkpoint/id??

        const url = "/checkpointdetails/" + props.checkpointInfo.id;
        console.log(url);
        navigate(url);

    }

    const showNavigation = (event) => {
        //fix- skal det være track/id/checkpoint/id??

        const url = "/navigationEdit/" + props.checkpointInfo.id;
        console.log(url);
        navigate(url);

    }

    const deleteCheckpoint = async () => {
        const url = '/api/checkpoint/removeCheckpoint?';
        const parameter = 'checkpointId=' + props.checkpointInfo.id;
        const response = await fetch(url + parameter, { method: 'DELETE' });

        props.updateCheckpointList()

        console.log("delete")
    }

    const shouldEdit = () => {
        setEditing(true)
    }

    const stopEdit = async () => {
        setEditing(false)
        console.log("djjdj stop focus")

        //post/put metode

        const url = '/api/checkpoint/editCheckpointTitle?';
        const parameter = 'checkpointTitle=' + oldTitle + "&checkpointId=" + props.checkpointInfo.id;
        const response = await fetch(url + parameter, { method: 'PUT' });
        //fiks sjekk respons i error handling

        props.updateCheckpointList()

        console.log("delete")
    }

    const handleChange = (e) => {
        console.log("endre");
        setOldTitle(e.target.value);

    }

    //{
    //    props.questionInfo.alternatives.map((alternative, index) =>

    //        <p
    //            key={index + "-" + alternative.text}
    //            style={{ backgroundColor: props.questionInfo.correctAlternative - 1 == index ? "lightGreen" : "pink" }}
    //        >

    //            {alternative.text}

    //        </p>
    //    )
    //}

    //return <Button onClick={showCheckpoint}><h6>id: {props.CheckpointInfo.id} userId: {props.CheckpointInfo.userId} trackId: {props.CheckpointInfo.trackId}</h6></Button>;
    return (<>
        <Box border="1px solid lightblue;" margin="2px;" style={{ width: '80%' }}>

            <p style={{ display: "inline" }}>Title:</p>
            {editing ?
                <input
                    style={{ display: "inline" }}
                    type="text"
                    value={oldTitle}
                    onChange={handleChange}
                    onBlur={stopEdit}
                >
                </input>

                :

                <span
                    onDoubleClick={shouldEdit}
                > {props.checkpointInfo.title}</span>
            }

            <p>Type: {props.checkpointInfo.quizId == null ? "Game" : "Quiz"}

            </p>


            <Button onClick={showCheckpoint}>Show details</Button>

            <Button onClick={deleteCheckpoint}>Delete checkpoint</Button>

            <Button onClick={showNavigation}>Show navigation</Button>


        </Box>


    </>);

}
