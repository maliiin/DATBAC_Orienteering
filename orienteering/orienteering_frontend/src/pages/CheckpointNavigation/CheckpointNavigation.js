import { TextField, Button, Grid, FormControl, FormLabel, RadioGroup, FormControlLabel, Radio } from '@mui/material';
import React, { useState, useRef } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';
import { createSearchParams, useParams } from 'react-router-dom';
import { useEffect } from "react";
import DisplayImagesUser from './Components/DisplayImagesUser';
export default function CheckpointNavigation() {
    //kan være greit å få inn checkpointid slik at det senere blir mulig å registrere at noen har vært på checkpointet, lagre score osv...
    const [current, setCurrent] = useState(0);
    const [trackFinished, setTrackFinished] = useState(false);
    const navigate = useNavigate();

    const nextImage = () => {
        setCurrent(current + 1)
        console.log(current)
    }

    const prevImage = () => {
        setCurrent(current - 1);
    }
    const params = useParams();

    const [imagesList, setImagesList] = useState(["", ""]);
    const [totalTime, setTotalTime] = useState("");

    const currentCheckpointId = params.checkpointId;

    const getNavigation = async () => {
        const sessionInfo = await fetch("/api/session/checkTrackFinished?currentCheckpoint=" + currentCheckpointId).then(res => res.json());
        if (sessionInfo.timeUsed != null) {
            setTrackFinished(true);
            setTotalTime(sessionInfo.timeUsed);
        }
        const navUrl = "/api/Navigation/GetNextNavigation?currentCheckpointId=" + currentCheckpointId;
        const response = await fetch(navUrl);
        if (!response.ok) {
            navigate("/errorpage")
        }
        const nav = await response.json();
            //fix-dersom endre rekkefølge på objekter, enten endre rekkefølge her eller display med rett order
        

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




    useEffect(() => {
        getNavigation();
    }, []);

    //<DisplayImagesUser imageInfo={imagesList[current]}></DisplayImagesUser>
    if (!trackFinished) {
        return (<>


            {imagesList[current]}




            <Grid container spacing={2}
                justifyContent="space-around"
                style={{
                    bottom: "10px",
                    position: "absolute"
                }}

            >
                <Grid item xs={5}>
                    <Button
                        onClick={prevImage}
                        style={{
                            //visibility: current <= 0 ? "hidden" : "inline"
                            display: current <= 0 ? "none" : "inline"
                        }}
                    >
                        Previous image
                    </Button>
                </Grid>
                <Grid item xs={5}>
                    <Button
                        onClick={nextImage}
                        style={{
                            display: current >= imagesList.length - 1 ? "none" : "inline"
                            //visibility: current >= imagesList.length - 1 ? "hidden" : "inline"
                        }}
                    >
                        Next image
                    </Button>
                    <Button
                        disabled
                        style={{
                            display: current >= imagesList.length - 1 ?   "inline":"none"
                            //visibility: current >= imagesList.length - 1 ? "hidden" : "inline"
                        }}
                    >
                        Scan QR code
                    </Button>
                </Grid>

            </Grid>
            <br></br>


        </>
        );
    }


    if (trackFinished) {
        return (
            <>
                <p>Track finished</p>
                <p>Total time used: {totalTime} minutes</p>
            </>
            );
    }
    

}
