import { TextField, Button, Grid, Box } from '@mui/material';
import React, { useState, useEffect } from "react";
import { Link, redirect, useNavigate, useParams } from 'react-router-dom';

export default function AddImage() {

    const handleSubmit = function () {
        console.log("not done yet")
    }

    return (<>

        <Box onSubmit={handleSubmit} component="form">
            <input type="file" id="image" required accept="image/*" />
            <Button type="submit">Add Image</Button>
        </Box>

    </>);
}