import { TextField, Button } from '@mui/material';
import { useState, useEffect } from "react";
import Drawer from '@mui/material/Drawer';

function TrackOverview() {

    const [userId, setUserId] = useState("");

    const loasUserId = async () => {
        const data = await fetch("api/user/getsignedinuserid").then(res => res.json());
        //const value = await data.json();
        setUserId(data.id);
    };

    useEffect(() => {
        
        loasUserId();
    }, []);




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


    //<Button onClick={getSignedInUser}> hent user id</Button>
        //<Button onClick={async () => { await getSignedInUser(); }}> hent user id</Button>
    //
    return <>

        <h1>hei {userId}</h1>
    </>

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
