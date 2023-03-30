import { Button } from '@mui/material';
import { useState, useEffect } from "react";
import TrackInfo from "./Components/TrackInfo";
import React from "react";
import CreateTrackForm from "./Components/CreateTrackForm";
import { useNavigate } from 'react-router-dom';
import { redirect } from "react-router-dom";
import Grid from '@mui/material/Grid';


//få returtype fra de ulike hooksene, ikke i use state men i const. så blir det
//if den og den er true vis dette, pass på at det kun blir en navigate--> kanskje slå sammen to hooks

//displays all the tracks of a user. not details of the tracks

export default function TrackOverview() {
    //const [shouldRender, setShouldRender] = useState(false);
    const [render, setRender] = useState(false);

    const navigate = useNavigate();
    //gir undefined
    //const test = useAuthentication();

    const [userInfo, setUserInfo] = useState({
        Id: ""
    });

    const [trackList, setTrackList] = useState("");
    const [list, setList] = useState("");

    //laster userid og sjekker at det stemmer
    const loadUserId = async () => {
        const response = await fetch("api/user/getsignedinuserid");

        ////not signed in
        //if (!response.ok) {
        //    console.log("ikke innlogget!!!");
        //    navigate("/login");
        //}

        const data = await response.json();

        setUserInfo(prevState => { return { ...prevState, Id: data.id } });
    };
    //fiks-trenger vi brukerid i det helet tatt
    const loadTrack = async () => {
        const response = await fetch("api/track/getTracks");
        if (!response.ok) {
            navigate("/errorpage");
        }
        const data = await response.json();

        setList(data.map((trackElement, index) =>
            <TrackInfo
                key={trackElement.id + "-" + index}
                trackInfo={trackElement}
                updateTrackList={ loadTrack}
            >

            </TrackInfo>
        ));
    }
    //
    //dette gjør at brukerid lastes
    useEffect(() => {

        const isAuthenticated = async () => {
            //check if user is signed in, redirect if not
            const checkUserUrl = "/api/user/getSignedInUserId";
            const response = await fetch(checkUserUrl);
            if (!response.ok) {
                navigate("/login");
                return false;
            } else {
                //console.log("user is signed in");
                return true;
            };

        };


        isAuthenticated().then(result => { setRender(result) });

        loadUserId();


    }, []);

    useEffect(() => {
        if (userInfo.Id != "" && typeof (userInfo.Id) !== "undefined") {
            loadTrack();
        }

    }, [userInfo.Id]);

    
    //console.log(render);
    if (render == true) {

        return (
            <>
                <Grid container
                    spacing={3}
                    margin="10px"
                    direction={{ xs: "column-reverse" , md:"row"}}
                    
                >
                    <Grid item xs={10} md={6 }>
                        <h4>List of all your tracks</h4>
                        <p>Double-click title to edit</p>
                        <div>{list}</div>
                    </Grid>

                    <Grid item xs={10} md={6 }>

                        <CreateTrackForm updateTracks={loadTrack} id={userInfo.Id}></CreateTrackForm>
                    </Grid>
                </Grid>

            </>
        );
    };


}

