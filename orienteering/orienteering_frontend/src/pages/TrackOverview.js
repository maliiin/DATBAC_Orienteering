import { useState, useEffect } from "react";

function TrackOverview() {
    const [trackList, setTrackList] = useState("")
    const fetchTracks = async () => {
        //Fiks: fjerne hardkoding av userid
        const UserId = "bb12cfff-cba1-4e09-9178-b9f5d0508538"
        const url = "track/gettracks?UserId=" + UserId;
        const response = await fetch(url);
        const data = await response.json();
        setTrackList(data);
    }
    useEffect(() => {

        fetchTracks();
    }, []);
    //Kilder: https://reactjs.org/docs/lists-and-keys.html (02.02.2023)
    const listItems = trackList.map((track) =>
        <li>{track}</li>
    );
    return <>
        <h1> Tracks</h1>
        <h2>List of tracks</h2>
        <ul>{listItems}</ul>
    </>


}

export default TrackOverview;
