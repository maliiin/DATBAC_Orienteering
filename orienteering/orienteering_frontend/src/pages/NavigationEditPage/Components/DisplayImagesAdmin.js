import { TextField, Button, Grid, Box } from '@mui/material';
import React, { useState, useEffect, props } from "react";
import { Link, redirect, useNavigate, useParams } from 'react-router-dom';


export default function DisplayImagesAdmin(props) {

    const deleteImage = () => {
        console.log("not implemented yet");
    }


    return (
        <>
            <Box border="1px solid lightblue;" margin="2px;">
                <img width={300} src={"data:image/png;base64," + props.image}></img>
                
                <Button onClick={deleteImage}>Slett ikke implementert</Button>

            </Box>

        </>);


}