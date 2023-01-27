import { createSearchParams, Link, useParams } from 'react-router-dom'
function Checkpoint(props) {
    const params = useParams();


    return <h1>post id {params.checkpointId}</h1>;
};

export default Checkpoint;
