import { useState, useEffect } from "react";
import { useNavigate } from "react-router";

//hook to ensure that the user is signed in,
//redirect to login if not

//reload tar lang tid
//fiks sikring av checkpoint
//fiks ikke i bruk??

export default function useAuthorizeTrack(trackId) {
    const navigate = useNavigate();
    //const [data, setData] = useState(null);
    const checkUserUrl = "https://localhost:3000/api/user/getSignedInUserId";
    //var data;
    let lovlig = false;

    console.log("inni hook annen");

    useEffect(() => {

        const test = async () => {
            //fiks skal denne være async egentlig??
            const data = await fetch(checkUserUrl).then(res=>res.json());
            var userId = data.id;

            const getTrackUrl = "https://localhost:3000/api/track/getTrack?trackId=" + trackId;
            const result = await fetch(getTrackUrl);
            console.log(result);
            const track = await result.json();
            //console.log("track auth");

            console.log(userId);
            console.log(track.userId);


            if (userId == track.userId) {
                console.log("DE ER LIKE------------");
                lovlig = true;
            } else {
                console.log("DE ER ULIKE-----------");
                navigate("/unauthorized");
            }
            return

        }

        test();

    }, []);
    return lovlig;
};

