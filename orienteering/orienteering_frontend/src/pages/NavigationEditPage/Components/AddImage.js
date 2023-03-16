import { TextField, Button, Grid, Box } from '@mui/material';
import React, { useState, useEffect } from "react";
import { Link, redirect, useNavigate, useParams } from 'react-router-dom';

export default function AddImage() {
    const [uploadedImage, setUploadedImage] = useState({
        FormFile: ""
    });

    const handleSubmit = async (event) => {
        event.preventDefault();

        const formData = new FormData();
        formData.append("file", uploadedImage);

        const requestAlternatives = {
            method: 'POST',
            //mode: 'cors',
            //headers: {
            //'Content-Type': 'application/json',
            //'Accept': 'application/json',

            //fix linje under -sikkerhet
            //'Access-Control-Allow-Origin': '*',
            // },
            //body: JSON.stringify(testBilde)
            body: formData

        };

        var response = await fetch('/api/checkpoint/AddImage', requestAlternatives);
    }

    const handleChange = function (event) {
        event.preventDefault();
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
                onChange={handleChange}
            />

            <Button
                type="submit"
            >
                Add Image
            </Button>
        </Box>

    </>);
}
