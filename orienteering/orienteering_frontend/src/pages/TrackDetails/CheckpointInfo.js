import { React } from "react";
import { Button } from '@mui/material';
import { Link, redirect, useNavigate } from 'react-router-dom';


//overview of singe checkpoint, not details
export default function CheckpointInfo(props) {

    const navigate = useNavigate();


    //display spesific track
    const showCheckpoint = (event) => {
        //fix- skal det være track/id/checkpoint/id??

        const url ="/checkpoint/" + props.checkpointInfo.id;
        console.log(url);
        navigate(url);

    }





    //return <Button onClick={showCheckpoint}><h6>id: {props.CheckpointInfo.id} userId: {props.CheckpointInfo.userId} trackId: {props.CheckpointInfo.trackId}</h6></Button>;
    return (<>
        <Button onClick={showCheckpoint}>
            <h6>
                id: {props.checkpointInfo.id}
            </h6>
        </Button>
    </>);

}
