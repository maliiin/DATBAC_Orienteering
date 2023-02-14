import { useState, useEffect } from "react";
import { useNavigate } from "react-router";

//hook to ensure that the user is signed in,
//redirect to login if not

export default function useAuthentication() {
    const navigate = useNavigate();
    //const [data, setData] = useState(null);
    const checkUserUrl = "/api/user/getSignedInUserId";
    //var data;

    console.log("inni hook");

    useEffect(() => {

        const checkAuthentication = async () => {
            //fiks skal denne være async egentlig??
            const response = await fetch(checkUserUrl);
            if (!response.ok) {
                navigate("/login");
            }
            //if (response.ok) {
            //    setData(await response.json().id);
            //} else {
            //    //not signed in:
            //    navigate("/login");
            //}
            //return data;
        }

        checkAuthentication();

        //async () => {


        //const response = await fetch(checkUserUrl);
        //if (response.ok) {
        //   data=await response.json();
        //} else {
        //    //not signed in:
        //    navigate("/login");
        //} 
        //return [data];

    }, []);
    //console.log(data);
    //return { data } ;

};

//export default useAuthentication;
