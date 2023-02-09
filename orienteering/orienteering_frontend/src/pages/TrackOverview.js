import {  Button } from '@mui/material';
import { useState, useEffect } from "react";
import  TrackInfo  from "./TrackInfo";
import React from "react";

function TrackOverview() {
    const [userInfo, setUserInfo] = useState ({
        Id: ""
    });

    //const [userId, setUserId] = useState("");//("a7803486-9413-41c1-b85d-6772213ac551");
    const [trackList, setTrackList] = useState("");
    const [list, setList] = useState("");

    const loadUserId = async () => {
        const data = await fetch("api/user/getsignedinuserid").then(res => res.json());
        console.log("the user id is " + data.id);
        //setUserId(data.id);
        setUserInfo(prevState => { return { ...prevState, Id: data.id } });



        //userInfo.UserId = data.id;
        //console.log("userinfo id " + userInfo.UserId);
    };

    const loadTrack = async () => {
        console.log("user id før hente ut tracks " + userInfo.Id);
        const data = await fetch("api/track/getTracks?userId=" + userInfo.Id).then(res => res.json());
        console.log(data);

        setTrackList(data);

        console.log("data og liste");
        console.log(trackList);

        setList(data.map((trackElement, index) =>
            <Button key={trackElement.id + "-button-" + index}>
                <TrackInfo key={trackElement.id + "-" + index} trackInfo={trackElement}>
                </TrackInfo>
            </Button>
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
        console.log("lag track userinfo er " + userInfo.Id);
        console.log(response.json());
    }

 
    useEffect(() => {
        loadUserId();
        
        
    }, []); 

    useEffect(() => {
        if (userInfo.Id != "") {
            console.log("ved load track, user id er" + userInfo.Id);
            loadTrack();
        }
       
    }, [userInfo.Id]);









    //const getSignedInUser = async () => {
    //    const data = await fetch("api/user/getsignedinuserid").then(res => res.json());
    //    //sjekk ok repsons
    //    //const data = response.then(res => res.json());
    //    //.then(res => res.json());
    //    console.log(data);
    //    const userId = data;
    //    setTest(userId);
    //    return true;


    //};

    return (
        <>
            <p>id til bruker {userInfo.Id}</p>
            <div>{list}</div>
            <Button onClick={createTrack}> lag track</Button>
        </>);

}

    //const [trackList, setTrackList] = useState("")

    //const fetchTracks = async () => {
    //    //Fiks: fjerne hardkoding av userid
    //    //const requestOptions = {
    //    //    method: 'Get',
    //    //    headers: {
    //    //        'Content-Type': 'application/json',
    //    //        'Accept': 'application/json'
    //    //    },
    //    //    body: JSON.stringify(userInfo)
    //    //};
    //    //const response = await fetch('/api/user/signinuser', requestOptions);
    //    const response = await fetch("/api/user/GetSignedInUserId");
    //    console.log(response);
    //    var userId="";
    //    if (response.ok) {
    //    //    console.log("fik id");
    //        userId = response.json();
    //    //    const test = userId.userId;
    //    //    console.log(userId);
    //    //    console.log(test);
    //    }

    //    //const userId = "bb12cfff-cba1-4e09-9178-b9f5d0508538"
    //    //const url = "track/gettracks?UserId=" + UserId;
    //    //const response = await fetch(url);
    //    //const data = await response.json();
    //    //setTrackList(data);
    //    setTrackList(userId);

    //}
    //useEffect(() => {

    //    fetchTracks();
    //}, []);
    //Kilder: https://reactjs.org/docs/lists-and-keys.html (02.02.2023)
    //const listItems = trackList.map((track) =>
    //    <li>{track}</li>
    //);
    //return <>
    //    <h1> Tracks</h1>
    //    <h2>List of tracks</h2>
    //    <ul>{listItems}</ul>
    //</>
    //return <h1>userid:{trackList} </h1>


    //return (<>
    //    <h1>  tracks</h1>
    //    <Drawer sx={{
    //        width: '100px',
    //        flexShrink: 0,
    //        '& .MuiDrawer-paper': {
    //            width: '100px',
    //            boxSizing: 'border-box',
    //        },
    //    }}
    //        variant="permanent"
    //        anchor="left" >hei hallo</Drawer>
    //</>);

//}

export default TrackOverview;
