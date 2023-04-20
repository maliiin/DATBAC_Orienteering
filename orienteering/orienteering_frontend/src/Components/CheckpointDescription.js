import { React, useState } from "react";

export default function CheckpointDescription(props) {
    const [description, setDescription] = useState("");


    const getDescription = async () => {
        const fetchedDescription = await fetch("/api/checkpoint/getDescription?checkpointId=" + props.checkpointId).then(res => res.json());
        setDescription(fetchedDescription);
    }

    useEffect(() => {
        getDescription();
    }, []);

    return (<>
        <div>{description}</div>
    </>);
}
