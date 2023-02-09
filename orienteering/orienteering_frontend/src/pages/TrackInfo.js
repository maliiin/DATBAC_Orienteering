import { React, props } from "react";

export default function TrackInfo(props) {
    return <h6>id: {props.trackInfo.id} userId: {props.trackInfo.userId}</h6>;
    //return <p> info track</p>
}

//export default TrackInfo;