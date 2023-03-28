import { TextField, Input, Button, Grid, Box } from '@mui/material';
import React, { useState, useEffect, props, useRef } from "react";
import { Link, redirect, useNavigate, useParams } from 'react-router-dom';

export default function AddImage(props) {
    const [uploadedImage, setUploadedImage] = useState({
        FormFile: "",
    });
    const imageRef = useRef(null);
    const [textDescription, setTextDescription] = useState("");
    const [uploadStatus, setUploadStatus] = useState("");

    const handleSubmit = async (event) => {
        event.preventDefault();

        const formData = new FormData();
        formData.append("file", uploadedImage);
        //console.log(imageRef.current.value)
        //formData.append("file", {
        //    FormFile: imageRef.current.value
        //});

        formData.append("checkpointId", props.checkpointId);
        formData.append("textDescription", textDescription);

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
        if (!response.ok) {
            setUploadStatus("Image too large. Try to downscale image before uploading");
        }
        else {
            setUploadStatus("");
        }

        //update parent
        props.updateImages();

        //clear input 
        setTextDescription("")
        imageRef.current.value = null;
    }

    const handleChangeImage = function (event) {
        event.preventDefault();
        setUploadedImage(event.target.files[0]);

        //imageRef.current.value = event.target.value[0];
        //console.log(event.target.value)


    }

    const handleChangeText = function (event) {
        setTextDescription(event.target.value);

    }


    //fix remove selected file on submit-clear input



    return (
        <>

            <Box onSubmit={handleSubmit} component="form">

                <input
                    //multiple
                    type="file"
                    id="image"
                    required
                    accept="image/*"
                    onChange={handleChangeImage}
                    ref={imageRef}
                //value={uploadedImage.FormFile }
                />

                <br></br>
                <TextField
                    //fix-skal være required eller ikke
                    required
                    onChange={(e) => handleChangeText(e)}
                    label="Description"
                    name="TextDescription"
                    variant="standard"
                    value={textDescription}
                >
                </TextField>
                <br></br>
                <br></br>
                <Button
                    type="submit"
                >
                    Add Image
                </Button>
            </Box>

            {uploadStatus}


        </>);
}
