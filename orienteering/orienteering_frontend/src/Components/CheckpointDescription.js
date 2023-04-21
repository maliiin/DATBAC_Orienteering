import { React, useState, useEffect } from "react";
import { Grid, Button} from '@mui/material';


export default function CheckpointDescription(props) {
    const [description, setDescription] = useState("");

    const getDescription = async () => {
        const response = await fetch("/api/checkpoint/getDescription?checkpointId=" + props.checkpointId);
        console.log(response);
        const fetchedDescription = await response.json();
        console.log(fetchedDescription);
        setDescription(fetchedDescription.description);
    }

    useEffect(() => {
        getDescription();
    }, []);

    return (<>
        <Grid
            container
            spacing={0}
            direction="column"
            alignItems="center"
            justifyContent="center"
            style={{ minHeight: '50vh' }}
        >
            <Grid item>
                <div>{description}</div>

            </Grid>
        </Grid>

        <Grid container spacing={2}
            justifyContent="space-around"
            style={{
                bottom: "10px",
                position: "absolute"
            }}>
            <Grid item>
                <Button onClick={props.hideDescription}>Continue</Button>

            </Grid>

        </Grid>
    </>);
}
