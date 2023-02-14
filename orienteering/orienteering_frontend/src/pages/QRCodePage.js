import { useState, useEffect } from "react";
import QRContainer from '../components/QRContainer';
import { useLocation } from 'react-router-dom';

function QRCodePage() {
    const [CheckpointList, setCheckpointList] = useState("");
    const [ListeItems, setListItems] = useState("");
    const location = useLocation();

    const fetchCheckpoints = async () => {
        console.log("start");
        // Kilder: til location.state.trackid: linje 13 under https://stackoverflow.com/questions/64566405/react-router-dom-v6-usenavigate-passing-value-to-another-component (13.02.2023)
        const TrackId = location.state.trackid;
        //const TrackId = "08db0da6-3633-48ec-8b8f-0799ce2f9cbd";
        //const TrackId = params.TrackId;
        const UserId = "d492968b-1d81-46f5-b2ae-eacfd20b5b5d"
        //const userResponse = await fetch("api/user/GetSignedInUserId").then(response => response.json());
        //const user = await userResponse.json();
       // const UserId = user.Id;
        const url = "api/qrcode/getqrcodes?UserId=" + UserId + "&TrackId=" + TrackId;
        //console.log("hei");
        //const data = await fetch(url).then(response => response.json());
        const response = await fetch(url);
        const data = await response.json();

        //const data = await response.json();
        //console.log(data);
        //setCheckpointList(data);
        setListItems(data.map((checkpoint, index) =>
            <>
                <h2>Checkpoint: {checkpoint.id}</h2>
            <QRContainer key={checkpoint.id + "-" + index} data={checkpoint.qrCode}></QRContainer>
                <br></br>
            </>
        ));
    }
    useEffect(() => {

        fetchCheckpoints();
    }, []);
    ////Kilder: https://reactjs.org/docs/lists-and-keys.html (02.02.2023)
    //console.log(CheckpointList);
    return (<>
        <h1> Checkpoints:</h1>
        <div>{ListeItems}</div>
        
    </>);


}

export default QRCodePage;