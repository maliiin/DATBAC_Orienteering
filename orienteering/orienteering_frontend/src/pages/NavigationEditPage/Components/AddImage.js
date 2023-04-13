import { TextField, Button, Box } from '@mui/material';
import React, { useState, useRef } from "react";

export default function AddImage(props) {
    const [uploadedImage, setUploadedImage] = useState({
        FormFile: "",
    });
    const imageRef = useRef(null);
    const [textDescription, setTextDescription] = useState("");

    const handleSubmit = async (event) => {
        event.preventDefault();

        const formData = new FormData();
        formData.append("file", uploadedImage);
        formData.append("checkpointId", props.checkpointId);
        formData.append("textDescription", textDescription);

        const requestAlternatives = {
            method: 'POST',
            body: formData
        };

        var response = await fetch('/api/navigation/AddImage', requestAlternatives);
        //fix-vet ikke om errorhandling trengs her?

        //update parent
        props.updateImages();

        //clear input 
        setTextDescription("")
        imageRef.current.value = null;
    }

    const handleChangeImage = function (event) {
        event.preventDefault();
        setUploadedImage(event.target.files[0]);
    }

    const handleChangeText = function (event) {
        setTextDescription(event.target.value);
    }

    return (
        <>
            <Box onSubmit={handleSubmit} component="form" style={{ width: "80%" }}>
                <p>Upload images showing the path from previous checkpoint to this checkpoint</p>
                <p>For the first checkpoint, the "previous" checkpoint will be the one created last</p>
                <p>The navigation will show the images in the order which they are added</p>

                <h4>Add more images</h4>
                <input
                    type="file"
                    id="image"
                    required
                    accept="image/*"
                    onChange={handleChangeImage}
                    ref={imageRef}
                />

                <br></br>
                <TextField
                    required
                    onChange={(e) => handleChangeText(e)}
                    label="Description"
                    name="TextDescription"
                    variant="standard"
                    value={textDescription}
                    inputProps={{ maxLength: 80 }}
                >
                </TextField>
                <br></br>
                <br></br>
                <Button type="submit">
                    Add Image
                </Button>
                <p>If the image won't be uploaded, try to downscale the image</p>
            </Box>
        </>);
}
