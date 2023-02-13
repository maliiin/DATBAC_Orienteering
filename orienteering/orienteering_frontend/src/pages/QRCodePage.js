import { useState, useEffect } from "react";
import QRContainer from '../components/QRContainer';

function QRCodePage() {
    const [CheckpointList, setCheckpointList] = useState("");
    const [ListeItems, setListItems] = useState("");

    const fetchCheckpoints = async () => {
        console.log("start");

        const TrackId = "df085e87-91c8-4c84-ac33-44f1ba680a80";
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