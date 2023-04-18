import { useState, useEffect } from "react";
import QRContainer from './Components/QRContainer';
import { Grid } from '@mui/material';
import { useLocation, useNavigate } from 'react-router-dom';

function QRCodePage() {
    const [ListeItems, setListItems] = useState("");
    const location = useLocation();
    const navigate = useNavigate();

    const fetchCheckpoints = async () => {
        const TrackId = location.state.trackid;
        const url = "api/qrcode/getqrcodes?TrackId=" + TrackId;
        const response = await fetch(url);
        //401 => not signed in
        if (response.status == 401) { navigate("/login"); }
        //other errors
        if (!response.ok) { navigate("/errorpage"); }

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