import { React } from "react";
import { Button } from '@mui/material';
import { Link, redirect, useNavigate } from 'react-router-dom';


//overview of singe checkpoint 
export default function CheckpointInfo(props) {

    //const navigate = useNavigate();


    //display spesific track
    //const showCheckpoint = (event) => {
    //    const url = "/track/" + props.trackInfo.id;
    //    console.log(url);
    //    navigate(url);

    //}





    //return <Button onClick={showCheckpoint}><h6>id: {props.CheckpointInfo.id} userId: {props.CheckpointInfo.userId} trackId: {props.CheckpointInfo.trackId}</h6></Button>;
    return (<>
        <Button>
            <h6>
                id: {props.checkpointInfo.id}
            </h6>
        </Button>
    </>);

}
