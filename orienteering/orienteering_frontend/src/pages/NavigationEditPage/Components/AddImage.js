import { TextField, Input, Button, Grid, Box } from '@mui/material';
import React, { useState, useEffect, props } from "react";
import { Link, redirect, useNavigate, useParams } from 'react-router-dom';
import Form from '../../../../../node_modules/react-bootstrap/esm/Form';

export default function AddImage(props) {
    const [uploadedImage, setUploadedImage] = useState({
        FormFile: "",
    });
    const [textDescription, setTextDescription] = useState("");

    const handleSubmit = async (event) => {
        event.preventDefault();

        const formData = new FormData();
        formData.append("file", uploadedImage);
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

        //update parent
        props.updateImages();

    }

    const handleChangeImage = function (event) {
        event.preventDefault();
        setUploadedImage(event.target.files[0]);


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


        </>);
}
