import {  Button } from '@mui/material';
import { useState, useEffect } from "react";
import  TrackInfo  from "./TrackInfo";
import React from "react";

//displays all the tracks

function TrackOverview() {
    const [userInfo, setUserInfo] = useState ({
        Id: ""
    });

    const [trackList, setTrackList] = useState("");
    const [list, setList] = useState("");

    const loadUserId = async () => {
        const data = await fetch("api/user/getsignedinuserid").then(res => res.json());
        //console.log("the user id is " + data.id);
        //setUserId(data.id);
        setUserInfo(prevState => { return { ...prevState, Id: data.id } });
    };

    const loadTrack = async () => {
        //console.log("user id før hente ut tracks " + userInfo.Id);
        const data = await fetch("api/track/getTracks?userId=" + userInfo.Id).then(res => res.json());
        //console.log(data);

        setTrackList(data);

        //console.log("data og liste");
        //console.log(trackList);

        setList(data.map((trackElement, index) =>
            /*<Button key={trackElement.id + "-button-" + index}>*/
                <TrackInfo key={trackElement.id + "-" + index} trackInfo={trackElement}>
                </TrackInfo>
            /*</Button>*/
        ));
    }

    const createTrack = async () => {
        //userInfo.UserId = userId;
        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(userInfo)
        };
        const response = await fetch('/api/track/createTrack', requestOptions);
        //console.log("lag track userinfo er " + userInfo.Id);
       // console.log(response.json());
    }

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
            <p>id til bruker {userInfo.Id}</p>
            <div>{list}</div>
            <Button onClick={createTrack}> lag track</Button>
        </>);
}

export default TrackOverview;
