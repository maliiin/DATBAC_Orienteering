import { TextField, Button, Grid, Box } from '@mui/material';
import React, { useState, useEffect, props } from "react";
import { Link, redirect, useNavigate, useParams } from 'react-router-dom';


export default function DisplayImagesUser(props) {

    return (
        <>
            <img
                width={'100%' }
                src={"data:image/" + props.imageInfo.fileType + ";base64," + props.imageInfo.imageData}
                
            >
            </img>
        </>);


}