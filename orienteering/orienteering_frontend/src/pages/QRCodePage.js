import { useState, useEffect } from "react";
import QRContainer from '../components/QRContainer';

function QRCodePage() {
    const [CheckpointList, setCheckpointList] = useState("");
    const [ListeItems, setListItems] = useState("");

    const fetchCheckpoints = async () => {
        console.log("start");

        const TrackId = "08db0a9a-91b6-47f0-89b2-d85e6971d57b";
        const UserId = "51eb9aee-1817-47fb-ac86-6e6a9dc7f3b5";
        //const userResponse = await fetch("api/user/GetSignedInUserId").then(response => response.json());
        //const user = await userResponse.json();
       // const UserId = user.Id;
        const url = "api/qrcode/getqrcodes?UserId=" + UserId + "&TrackId=" + TrackId;
        //console.log("hei");
        const data = await fetch(url).then(response => response.json());
        //const data = await response.json();
        //console.log(data);
        setCheckpointList(data);
        setListItems(CheckpointList.map((checkpoint, index) =>
            <QRContainer key={checkpoint.Id + "-" + index} data={checkpoint.QRCode}></QRContainer>
        ));
    }
    useEffect(() => {

        fetchCheckpoints();
    }, []);
    ////Kilder: https://reactjs.org/docs/lists-and-keys.html (02.02.2023)
    //console.log(CheckpointList);
    return (<>
        <h1> Tracks</h1>
        <h2>List of tracks</h2>
        <div>{ListeItems}</div>
        
    </>);


}

export default QRCodePage;