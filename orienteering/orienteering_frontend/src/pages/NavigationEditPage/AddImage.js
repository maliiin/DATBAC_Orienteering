import { TextField, Button, Grid, Box } from '@mui/material';
import React, { useState, useEffect } from "react";
import { Link, redirect, useNavigate, useParams } from 'react-router-dom';

export default function AddImage() {
    const [uploadedImage, setUploadedImage] = useState("");

    const handleSubmit = function (event) {
        event.preventDefault();
        console.log("not done yet")
    }

    const handleChange = function (event) {
        event.preventDefault();
        console.log(event.target.files[0])
        console.log(event.target.files[0])
        var t=URL.createObjectURL(event.target.files[0])
        console.log(t);
        setUploadedImage(event.target.files[0]);
    }

    return (<>

        <Box onSubmit={handleSubmit} component="form">

            <input
                //multiple
                type="file"
                id="image"
                required
                accept="image/*"
                onChange={handleChange }
            />

            <Button
                type="submit"
            >
                Add Image
            </Button>
        </Box>

    </>);
}