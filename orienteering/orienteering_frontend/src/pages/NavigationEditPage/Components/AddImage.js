import { TextField, Button, Grid, Box } from '@mui/material';
import React, { useState, useEffect, props } from "react";
import { Link, redirect, useNavigate, useParams } from 'react-router-dom';

export default function AddImage(props) {
    const [uploadedImage, setUploadedImage] = useState({
        FormFile: "",
    });

    const [testImg, setTestImg] = useState(null);
    const [testBlob, setTestBlob] = useState("");
    const [testBytes, setTestBytes] = useState(null);



    const handleSubmit = async (event) => {
        event.preventDefault();

        const formData = new FormData();
        formData.append("file", uploadedImage);
        formData.append("checkpointId", props.checkpointId);

        console.log(props.checkpointId);
        const send = {
            FormFile: formData,
            CheckpointId: props.CheckpointId
        }

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
            //body: JSON.stringify(send)

        };

        var response = await fetch('/api/navigation/AddImage', requestAlternatives);

        //update parent
        props.updateImages();

    }

    const handleChange = function (event) {
        event.preventDefault();
        setUploadedImage(event.target.files[0]);
    }


    //fix remove selected file on submit-clear input



    return (
        <>

        <img src={testImg}></img>
        <Box onSubmit={handleSubmit} component="form">

            <input
                //multiple
                type="file"
                id="image"
                required
                accept="image/*"
                onChange={handleChange}
            />
                <br></br>
                <br></br>

            <Button
                type="submit"
            >
                Add Image
            </Button>
        </Box>


    </>);
}
