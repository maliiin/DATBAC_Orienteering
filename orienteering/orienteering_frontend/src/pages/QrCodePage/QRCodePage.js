import { useState, useEffect } from "react";
import QRContainer from './Components/QRContainer';
import { Button, Box, Grid } from '@mui/material';

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
        //const UserId = "536a339d-f3f6-43f1-a921-099bdeb9fb1b";
        //const userResponse = await fetch("api/user/GetSignedInUserId").then(response => response.json());
        //const user = await userResponse.json();
        // const UserId = user.Id;
        const url = "api/qrcode/getqrcodes?TrackId=" + TrackId;
        //console.log("hei");
        //const data = await fetch(url).then(response => response.json());
        const response = await fetch(url);
        const data = await response.json();

        //const data = await response.json();
        //console.log(data);
        //setCheckpointList(data);
        setListItems(data.map((checkpoint, index) =>
            <QRContainer
                title={checkpoint.title}
                key={checkpoint.id + "-" + index}
                data={checkpoint.qrCode}>
            </QRContainer>
        ));
    }
    useEffect(() => {

        fetchCheckpoints();
    }, []);
    ////Kilder: https://reactjs.org/docs/lists-and-keys.html (02.02.2023)
    //console.log(CheckpointList);
    return (<>

        <Grid
            container
            direction="column"
            alignItems="center"
            justifyContent="center"
        >
            {ListeItems }
        </Grid>

    </>);


}

export default QRCodePage;