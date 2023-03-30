import { TextField, Button, Grid, Box } from '@mui/material';
import React, { useState, useEffect, props } from "react";
import { Link, redirect, useNavigate, useParams } from 'react-router-dom';


export default function DisplayImagesUser(props) {
    //fix- nå kan bildet skaleres rart om det er for høyt
    return (
        <>

            <img
                width={'100%'}
                style={{
                    maxHeight: window.screen.availHeight - 50 + 'px',
                }}
                src={"data:image/" + props.imageInfo.fileType + ";base64," + props.imageInfo.imageData}

            />
            <p
                style={{
                    margin:"20px"

                }}
            >{props.imageInfo.textDescription} </p>
        </>);


}
