import { TextField, Button, Grid } from '@mui/material';
import React, { useState, useEffect } from "react";
import { Link, redirect, useNavigate, useParams } from 'react-router-dom';
import AddImage  from "./Components/AddImage";
import DisplayImagesAdmin from './Components/DisplayImagesAdmin';


export default function NavigationEditPage() {

    const navigate = useNavigate();
    const params = useParams();
    const [image, setImage] = useState("");

    const cId = params.checkpointId;
    console.log(cId);

    const [render, setRender] = useState(false);

    const loadImages = async ()=>{
        //var img = await fetch("/api/navigation/GetNavigation?checkpointId=" + props.checkpointId).then(r => r.json());

        var img = await fetch("/api/navigation/GetNavigation?checkpointId=" + "fix denne tekst").then(r => r.json());
        //setTestBlob(await img.blob());
        //console.log(img);
        //setTestImg(img.ImageTest);

        console.log(img.imageTest)
        console.log(img)



        setImage(<DisplayImagesAdmin image={img.imageTest}></DisplayImagesAdmin>
);


    }


    useEffect(() => {
        //is authenticated and correct track?
        const isAuthenticated = async () => {

            const checkUserUrl = "/api/user/getSignedInUserId";
            const response = await fetch(checkUserUrl);

            if (!response.ok) {
                //not signed in, redirect to login
                navigate("/login");
                return false;
            };

            const user = await response.json();
            const userId = user.id;

            //load checkpoint
            const checkpoint = await fetch("/api/checkpoint/getCheckpoint?checkpointId=" + params.checkpointId).then(r => r.json());

            //check that the signed in user owns the track
            const trackId = checkpoint.trackId;
            const getTrackUrl = "/api/track/getTrack?trackId=" + trackId;

            const result = await fetch(getTrackUrl);
            const track = await result.json();

            if (userId != track.userId) {
                navigate("/unauthorized");
                return false;
            }
            return true;

        };

        isAuthenticated().then(result => { setRender(result) });
        loadImages();

    }, []);







    if (render == true) {
        return (<>

            <Grid container spacing={3} margin="10px">

                <Grid item xs={6}>
                    <h4>Navigation overview </h4>
                    {image }
                </Grid>

                <Grid item xs={6}>
                    <h4>Add more images</h4>
                    <AddImage checkpointId={cId}></AddImage>

                </Grid>
            </Grid>

        </>);

    };
}

