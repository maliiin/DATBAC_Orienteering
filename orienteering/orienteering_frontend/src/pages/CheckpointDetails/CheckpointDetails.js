import { createSearchParams, Link, useParams } from 'react-router-dom'
import useAuthentication from "../../hooks/useAuthentication";


//page
//display all info of a single checkpoint

export default function CheckpointDetails(props) {
    useAuthentication();

    const params = useParams();


    return <h1>post id {params.checkpointId}</h1>;
};


