import { TextField, Button, Grid, FormControl, FormLabel, RadioGroup, FormControlLabel, Radio } from '@mui/material';
import React, { useState, useRef } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import DisplayImagesUser from './Components/DisplayImagesUser';
export default function CheckpointNavigation() {
    //kan være greit å få inn checkpointid slik at det senere blir mulig å registrere at noen har vært på checkpointet, lagre score osv...
    const [current, setCurrent] = useState(0);

    const nextImage = () => {
        setCurrent(current + 1)
        console.log(current)
    }

    const prevImage = () => {
        setCurrent(current - 1);
    }
    const params = useParams();

    const [imagesList, setImagesList] = useState(["h", "hh"]);

    const currentCheckpointId = params.checkpointId;

    const getNavigation = async () => {

        const navUrl = "/api/Navigation/GetNextNavigation?currentCheckpointId=" + currentCheckpointId;
        const res = await fetch(navUrl);
        if (res.ok) {
            //fix-dersom endre rekkefølge på objekter, enten endre rekkefølge her eller display med rett order
            const nav = await res.json();
            setImagesList(nav.images.map((imageInfo, index) =>
                <>
                    <DisplayImagesUser
                        imageInfo={imageInfo}
                        key={index + "-" + imageInfo.Order}

                        
                    >
                    </DisplayImagesUser>
                </>

            ));
        }
        //fix-naviger til errorside hvis ikke?
    }

    useEffect(() => {
        console.log(33)
        getNavigation();
    }, []);

    //<DisplayImagesUser imageInfo={imagesList[current]}></DisplayImagesUser>

    return (<>

   
                {imagesList[current]}




        <Grid container spacing={2}
            justifyContent="space-around"
            style={{
                bottom: "10px",
                position: "absolute"
            }}

        >
                <Grid item xs={2}>
                    <Button
                        onClick={prevImage}
                        style={{
                            //visibility: current <= 0 ? "hidden" : "inline"
                            display: current <= 0 ? "none" : "inline"
                        }}
                    >
                        forrige
                    </Button>
                </Grid>
                <Grid item xs={2}>
                    <Button
                        onClick={nextImage}
                        style={{
                            display: current >= imagesList.length - 1 ? "none" : "inline"
                            //visibility: current >= imagesList.length - 1 ? "hidden" : "inline"
                        }}
                    >
                        neste
                    </Button>
                </Grid>



        </Grid>
        <br></br>
        <input accept='image/*' id='icon-button-file' type='file' capture='environment'/>
        <input type="file" id="cap" name="personalPhoto" accept="image/*" capture="camera" id="camera"/>


    </>
    );

}
