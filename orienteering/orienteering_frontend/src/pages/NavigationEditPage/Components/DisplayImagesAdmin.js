import { Button, Box } from '@mui/material';
import React, { useState } from "react";

export default function DisplayImagesAdmin(props) {
    const [editing, setEditing] = useState(false);
    const [oldText, setOldText] = useState(props.imageInfo.textDescription);

    const deleteImage = async () => {
        const url = "/api/navigation/DeleteImage?navigationId=" + props.navId + "&imageId=" + props.imageInfo.id;
        await fetch(url, { method: 'DELETE' });

        //update images in parent
        props.updateImages();
    }

    const shouldEdit = () => {
        setEditing(true)
    }

    const stopEdit = async () => {
        setEditing(false)

        const url = '/api/navigation/editNavigationText';
        await fetch(url, {
            method: "PATCH",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                NavigationId: props.navId,
                NewText: oldText,
                NavigationImageId: props.imageInfo.id
            })
        });

        //update images in parent
        props.updateImages();
    }

    const handleChange = (e) => {
        setOldText(e.target.value);
    }

    return (
        <>
            <Box
                border="1px solid lightblue;"
                margin="2px;"
                style={{
                    width: '80%',
                    maxWidth: '400px'
                }}
            >
                <img
                    width={200}
                    style={{ width: '100%' }}
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
                        maxLength={80}
                    >
                    </input>

                    :

                    <span
                        onDoubleClick={shouldEdit}
                    > {props.imageInfo.textDescription}</span>
                }
                <br></br>
                <br></br>

                <Button onClick={deleteImage}>Delete image</Button>
            </Box>
        </>);
}