import React from "react";

export default function DisplayImagesUser(props) {
    //fix- nå kan bildet skaleres rart om det er for høyt
    return (
        <>
            <img
                width={'100%'}
                style={{
                    maxHeight: window.screen.availHeight - 50 + 'px',
                }}
                src={"data:image/" + props.imageInfo.fileType + ";base64," + props.imageInfo.imageData}
            />
            <p style={{
                margin: "20px"
            }}>
                {props.imageInfo.textDescription}
            </p>
        </>);
}
