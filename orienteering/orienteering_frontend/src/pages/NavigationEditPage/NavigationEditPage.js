import { TextField, Button, Grid } from '@mui/material';
import React, { useState, useEffect } from "react";
import { Link, redirect, useNavigate, useParams } from 'react-router-dom';
import AddImage  from "./Components/AddImage";
import DisplayImagesAdmin from './Components/DisplayImagesAdmin';


export default function NavigationEditPage() {

    const navigate = useNavigate();
    const params = useParams();

    const [imageList, setImageList] = useState("hei");

    const cId = params.checkpointId;

    const [render, setRender] = useState(false);

    const loadImages = async () => {
        console.log("loadImages")
        //var img = await fetch("/api/navigation/GetNavigation?checkpointId=" + props.checkpointId).then(r => r.json());

        var Navigation = await fetch("/api/navigation/GetNavigation?checkpointId=" + params.checkpointId).then(r => r.json());


        setImageList(Navigation.images.map((imageInfo, index) =>
            <>
                <DisplayImagesAdmin
                    imageInfo={imageInfo}
                    key={index+"-"+imageInfo.Order }
                >
                </DisplayImagesAdmin>
                
            </>
        ));

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
                    {imageList }
                </Grid>

                <Grid item xs={6}>
                    <h4>Add more images</h4>
                    <AddImage
                        checkpointId={params.checkpointId}
                        updateImages={loadImages }
                    >
                    </AddImage>

                </Grid>
            </Grid>

        </>);

    };
}

