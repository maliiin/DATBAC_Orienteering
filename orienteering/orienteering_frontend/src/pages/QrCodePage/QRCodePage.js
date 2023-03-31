import { useState, useEffect } from "react";
import QRContainer from './Components/QRContainer';
import { Button, Box, Grid } from '@mui/material';

import { useLocation, useNavigate } from 'react-router-dom';

function QRCodePage() {
    const [ListeItems, setListItems] = useState("");
    const location = useLocation();
    const navigate = useNavigate();

    const fetchCheckpoints = async () => {
        // Kilder: til location.state.trackid: linje 13 under https://stackoverflow.com/questions/64566405/react-router-dom-v6-usenavigate-passing-value-to-another-component (13.02.2023)
        const TrackId = location.state.trackid;
        const url = "api/qrcode/getqrcodes?TrackId=" + TrackId;
        //sjekk mulige responser
        const response = await fetch(url);
        if (!response.ok) {
            navigate("/errorpage");
        }
        const data = await response.json();

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
    return (<>

        <Grid
            container
            direction="column"
            alignItems="center"
            justifyContent="center"
        >
            {ListeItems}
        </Grid>

    </>);
}

export default QRCodePage;