import { createSearchParams, Link, useParams } from 'react-router-dom'

//page
//display all info of a single checkpoint

export default function CheckpointDetails(props) {
    const params = useParams();


    return <h1>post id {params.checkpointId}</h1>;
};


