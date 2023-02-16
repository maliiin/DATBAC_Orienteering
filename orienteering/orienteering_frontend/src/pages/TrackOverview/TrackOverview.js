import {  Button } from '@mui/material';
import { useState, useEffect } from "react";
import  TrackInfo  from "./TrackInfo";
import React from "react";
import CreateTrackForm from "./CreateTrackForm";
import { useNavigate } from 'react-router-dom';
import { redirect } from "react-router-dom";
import useAuthentication from "../../hooks/useAuthentication";

//f� returtype fra de ulike hooksene, ikke i use state men i const. s� blir det
//if den og den er true vis dette, pass p� at det kun blir en navigate--> kanskje sl� sammen to hooks

//displays all the tracks of a user. not details of the tracks

export default function TrackOverview() {
    //const [shouldRender, setShouldRender] = useState(false);
    const [render, setRender] = useState(false);
    var verdi = "hehehe";

    const navigate = useNavigate();


    const [userInfo, setUserInfo] = useState ({
       Id:""
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
        const data = await fetch("api/track/getTracks?userId=" + userInfo.Id).then(res => res.json());

        setList(data.map((trackElement, index) =>
                <TrackInfo key={trackElement.id + "-" + index} trackInfo={trackElement}>
                </TrackInfo>
        ));
    }
    //
    //dette gj�r at brukerid lastes
    useEffect(() => {

        const isAuthenticated = async () => {
            //check if user is signed in, redirect if not
            const checkUserUrl = "/api/user/getSignedInUserId";
            const response = await fetch(checkUserUrl);
            if (!response.ok) {
                navigate("/login");
                return false;
            } else {
                console.log("user is signed in");
                return true;
            };

        };


        isAuthenticated().then(result => { setRender(result) });

        loadUserId();
        
        
    }, []); 

    useEffect(() => {
        if (userInfo.Id != "" && typeof(userInfo.Id)!== "undefined" ) {
            console.log("load track--------------");
            console.log(userInfo.Id);
            //console.log("ved load track, user id er" + userInfo.Id);
            loadTrack();
        }
       
    }, [userInfo.Id]);


    console.log(render);
    if (render == true) {
        //if (verdi == true) {

        return (
            <>
                <p>id til brukeren {userInfo.Id}</p>

                <CreateTrackForm id={userInfo.Id}></CreateTrackForm>
                <div>{list}</div>

            </>
        );
    };


}
























//import { Button } from '@mui/material';
//import { useState, useEffect } from "react";
//import TrackInfo from "./TrackInfo";
//import React from "react";
//import CreateTrackForm from "./CreateTrackForm";
//import { useNavigate } from 'react-router-dom';
//import { redirect } from "react-router-dom";
//import useAuthentication from "../../hooks/useAuthentication";

////f� returtype fra de ulike hooksene, ikke i use state men i const. s� blir det
////if den og den er true vis dette, pass p� at det kun blir en navigate--> kanskje sl� sammen to hooks

////displays all the tracks of a user. not details of the tracks

//function TrackOverview() {
//    //const [shouldRender, setShouldRender] = useState(false);
//    const [render, setRender] = useState(false);

//    const navigate = useNavigate();

//    //ensure that user is authenticated

//    console.log(render);
//    setRender(useAuthentication());
//    console.log(render);

//    //useEffect(() => {
//    //    console.log("inni effect-----------------------------");
//    //    console.log(render);
//    //    if (render != null) {
//    //        if (!render) {
//    //            navigate("/login");
//    //        };
//    //    }
//    //},[render]);



//    const [userInfo, setUserInfo] = useState({
//        Id: ""
//    });

//    const [trackList, setTrackList] = useState("");
//    const [list, setList] = useState("");

//    //laster userid og sjekker at det stemmer
//    const loadUserId = async () => {
//        const response = await fetch("api/user/getsignedinuserid");

//        ////not signed in
//        //if (!response.ok) {
//        //    console.log("ikke innlogget!!!");
//        //    navigate("/login");
//        //}

//        const data = await response.json();

//        setUserInfo(prevState => { return { ...prevState, Id: data.id } });
//    };

//    const loadTrack = async () => {
//        const data = await fetch("api/track/getTracks?userId=" + userInfo.Id).then(res => res.json());

//        setList(data.map((trackElement, index) =>
//            <TrackInfo key={trackElement.id + "-" + index} trackInfo={trackElement}>
//            </TrackInfo>
//        ));
//    }

//    //dette gj�r at brukerid lastes
//    useEffect(() => {

//        loadUserId();


//    }, []);

//    useEffect(() => {
//        if (userInfo.Id != "" && typeof (userInfo.Id) !== "undefined") {
//            console.log("load track--------------");
//            console.log(userInfo.Id);
//            //console.log("ved load track, user id er" + userInfo.Id);
//            loadTrack();
//        }

//    }, [userInfo.Id]);


//    //if (!render)
//    //{
//    //    //render is false
//    //    //navigate("/login");
//    //    return <p> not signed in!</p>
//    //}
//    //else
//    //{
//    return (
//        <>
//            <p>id til brukeren {userInfo.Id}</p>

//            <CreateTrackForm id={userInfo.Id}></CreateTrackForm>
//            <div>{list}</div>

//        </>
//    );
//    //};
//}

//export default TrackOverview;
