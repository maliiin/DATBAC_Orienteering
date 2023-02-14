import {  Button } from '@mui/material';
import { useState, useEffect } from "react";
import  TrackInfo  from "./TrackInfo";
import React from "react";
import CreateTrackForm from "./CreateTrackForm";
import { useNavigate } from 'react-router-dom';
import { redirect } from "react-router-dom";
import useAuthentication from "../../hooks/useAuthentication";


//displays all the tracks of a user. not details of the tracks

function TrackOverview() {
    //ensure that user is authenticated
    useAuthentication();


   // const navigate = useNavigate();
    //console.log(useAuthentication().id);
    const [userInfo, setUserInfo] = useState ({
       Id:""// useAuthentication()
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

    const loadTrack = async () => {
        //console.log("user id før hente ut tracks " + userInfo.Id);
        const data = await fetch("api/track/getTracks?userId=" + userInfo.Id).then(res => res.json());
        //console.log(data);

        setTrackList(data);

        //console.log("data og liste");
        //console.log(trackList);
        console.log(data);
        setList(data.map((trackElement, index) =>
            /*<Button key={trackElement.id + "-button-" + index}>*/
                <TrackInfo key={trackElement.id + "-" + index} trackInfo={trackElement}>
                </TrackInfo>
            /*</Button>*/
        ));
    }

    //const createTrack = async () => {
    //    //userInfo.UserId = userId;
    //    const requestOptions = {
    //        method: 'POST',
    //        headers: {
    //            'Content-Type': 'application/json',
    //            'Accept': 'application/json'
    //        },
    //        body: JSON.stringify(userInfo)
    //    };
    //    const response = await fetch('/api/track/createTrack', requestOptions);
    //    //console.log("lag track userinfo er " + userInfo.Id);
    //   // console.log(response.json());
    //}


    //dette gjør at brukerid lastes
    useEffect(() => {
        loadUserId();
        
        
    }, []); 

    useEffect(() => {
        if (userInfo.Id != "") {
            //console.log("ved load track, user id er" + userInfo.Id);
            loadTrack();
        }
       
    }, [userInfo.Id]);

    return (
        <>
            <p>id til brukeren {userInfo.Id}</p>

            <CreateTrackForm id={userInfo.Id }></CreateTrackForm>
            <div>{list}</div>
            
        </>);
}

export default TrackOverview;
//<Button onClick={createTrack}> lag track</Button>