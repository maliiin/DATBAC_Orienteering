import { TextField, Button, Grid, Box } from '@mui/material';
import React, { useState, useEffect, props } from "react";
import { Link, redirect, useNavigate, useParams } from 'react-router-dom';


export default function DisplayImagesAdmin(props) {
    const [editing, setEditing] = useState(false);
    const [oldText, setOldText] = useState(props.imageInfo.textDescription);


    const deleteImage = () => {
        console.log("not implemented yet");
    }

    const shouldEdit = () => {
        setEditing(true)
    }

    const stopEdit = async () => {
        setEditing(false)

        const url = '/api/navigation/editNavigationText?';
        const parameter = 'navigationId=' + props.navId
            + "&newText=" + oldText
            + "&navigationImageId=" + props.imageInfo.id;
        const response = await fetch(url + parameter, { method: 'PUT' });
        //fiks sjekk respons i error handling

        //update images in parent
        props.updateImages();
    }

    const handleChange = (e) => {
        console.log("endre");
        setOldText(e.target.value);

    }


    return (
        <>
            <Box border="1px solid lightblue;" margin="2px;">
                <img
                    width={200}
                    src={"data:image/" + props.imageInfo.fileType + ";base64," + props.imageInfo.imageData}
                >
                </img>
                <br></br>
                <br></br>
                <p style={{ display: "inline" }}>Description:</p>
                {editing ?
                    <input
                        style={{ display: "inline" }}
                        type="text"
                        value={oldText}
                        onChange={handleChange}
                        onBlur={stopEdit}
                    >
                    </input>

                    :

                    <span
                        onDoubleClick={shouldEdit}
                    > {props.imageInfo.textDescription}</span>
                }


                <Button onClick={deleteImage}>Slett ikke implementert</Button>

            </Box>

        </>);


}