import { React } from "react";
import { Grid } from '@mui/material';

export default function QRContainer(props) {
    return (
        <>
            <Grid item>
                <h2
                    style={{
                        'margin-left': '30px',
                        'wordWrap': 'break-word',
                        'width': '300px'
                    }}>
                    {props.title}
                </h2>
                <img
                    width='300'
                    height='300'
                    src={`data:image/jpeg;base64,${props.data}`}
                />
            </Grid>
        </>);
}

