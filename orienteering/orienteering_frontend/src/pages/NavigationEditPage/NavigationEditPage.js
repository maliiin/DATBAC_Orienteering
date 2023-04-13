import { Grid } from '@mui/material';
import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from 'react-router-dom';
import AddImage from "./Components/AddImage";
import DisplayImagesAdmin from './Components/DisplayImagesAdmin';

export default function NavigationEditPage() {

    const navigate = useNavigate();
    const params = useParams();

    const [imageList, setImageList] = useState("");

    const loadImages = async () => {
        const response = await fetch("/api/navigation/GetNavigation?checkpointId=" + params.checkpointId);
        //401 => not signed in
        if (response.status == 401) { navigate("/login"); }
        //404 => dont exist or not your checkpoint
        if (response.status == 404) { navigate("/errorpage") }

        const Navigation = await response.json();

        setImageList(Navigation.images.map((imageInfo, index) =>
            <>
                <DisplayImagesAdmin
                    imageInfo={imageInfo}
                    key={index + "-" + imageInfo.Order}
                    navId={Navigation.id}
                    updateImages={loadImages}
                >
                </DisplayImagesAdmin>

            </>
        ));

    }


    useEffect(() => {

            loadImages();

        }, []);

    return (<>

        <Grid
            container
            spacing={3}
            margin="10px"
            direction={{ xs: "column-reverse", md: "row" }}
        >

            <Grid item xs={10} md={6}>
                <h4>Navigation overview </h4>
                <p>Doubletap description text to edit.</p>
                {imageList}
            </Grid>

                <Grid item xs={10} md={6 }>
                    <AddImage
                        checkpointId={params.checkpointId}
                        updateImages={loadImages }
                    >
                    </AddImage>

            </Grid>
        </Grid>

    </>);

};


