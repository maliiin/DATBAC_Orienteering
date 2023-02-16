import { useState, useEffect } from "react";
import { useNavigate } from "react-router";
//fiks, not in use???

//hook to ensure that the user is signed in,
//redirect to login if not

export default function useAuthentication() {
    const navigate = useNavigate();

    useEffect(() => {

        const isAuthenticated = async () => {
            //check if user is signed in, redirect if not
            const checkUserUrl = "/api/user/getSignedInUserId";
            const response = await fetch(checkUserUrl);
            if (!response.ok) {
                navigate("/login");
                return false;
            } else {
                console.log("user is signed in");
                return true;
            };

        };


        isAuthenticated()
            .then(result => {
                return result
            });



    }, []);
}
//    const navigate = useNavigate();
//    const checkUserUrl = "/api/user/getSignedInUserId";

//    console.log("inni hook");

//    const checkAuthentication = async () => {

//        //fiks skal denne være async egentlig?? er det nødvendig
//        const response = await fetch(checkUserUrl);
//        if (!response.ok)
//        {
//            //should not render--navigate to login
//            //navigate("/login");
//            console.log("return false");

//            return false;

//        }
//        else
//        {
//            console.log("return true");
//            //should render, is signed in
//            return true;
//        }
//    }
//    checkAuthentication();
//};




//import { useState, useEffect } from "react";
//import { useNavigate } from "react-router";

////hook to ensure that the user is signed in,
////redirect to login if not

//export default function useAuthentication() {
//    const navigate = useNavigate();
//    const checkUserUrl = "/api/user/getSignedInUserId";

//    console.log("inni hook");

//    useEffect(() => {

//        const checkAuthentication = async () => {

//            //fiks skal denne være async egentlig?? er det nødvendig
//            const response = await fetch(checkUserUrl);
//            if (!response.ok) {
//                navigate("/login");

//            }
//        }
//        checkAuthentication();
    
//    }, []);
//    return true;
//};

