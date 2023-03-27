
import { React, useState } from "react";
import { Button, Box, Grid } from '@mui/material';
import { Link, redirect, useNavigate } from 'react-router-dom';


export default function QRContainer(props) {
    return (
        <>
            <Grid item>
                <h2
                    style={{
                        'margin-left': '30px'
            } }

                >{props.title}</h2>
            <img
                width='300'
                height='300'
                src={`data:image/jpeg;base64,${props.data}`}
            />

        </Grid>

        </>);
}

