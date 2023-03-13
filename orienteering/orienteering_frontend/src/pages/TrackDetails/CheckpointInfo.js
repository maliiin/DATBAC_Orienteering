import { React } from "react";
import { Button, Box, Grid } from '@mui/material';
import { Link, redirect, useNavigate } from 'react-router-dom';


//overview of singe checkpoint, not details
export default function CheckpointInfo(props) {

    const navigate = useNavigate();



    //display spesific track
    const showCheckpoint = (event) => {
        //fix- skal det være track/id/checkpoint/id??

        const url = "/checkpointdetails/" + props.checkpointInfo.id;
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
        <Box border="1px solid lightblue;" margin="2px;">

            <p>Tittel: {props.checkpointInfo.title}</p>
            <p>Type: {props.checkpointInfo.quizId == null ? "Spill" : "Quiz"}

            </p>


            <Button onClick={showCheckpoint}>vis detaljer</Button>

            <Button onClick={deleteCheckpoint}>Slett post</Button>

        </Box>


    </>);

}
