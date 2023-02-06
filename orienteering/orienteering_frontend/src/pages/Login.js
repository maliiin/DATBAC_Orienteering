import { TextField, Button } from '@mui/material';
import React, { useState } from "react";
import { Link, redirect, useNavigate } from 'react-router-dom';



//dette er registrer, ikke login!!
function Login() {
    const navigate = useNavigate();


    const handleSubmit = async (event) => {
        event.preventDefault();

        const response = await SignInUser();

        if (response.ok) {
            //fix dette bør byttes ut med 'admin' side (redigere poster)
            navigate("/");
        } else {
            console.log("not ok");
        }

        
    }
    const [userInfo, setUserInfo] = useState({
        username: "",
        password: "",
        email:"hallo@gmail.com"
    });


    const handleChange = (event) => {
        console.log("change");
        //update state
        setUserInfo({ ...userInfo, [event.target.name]: event.target.value });
    };

    const SignInUser = async ()=> {
        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(userInfo)
        };
        const response = await fetch('/api/user/signinuser', requestOptions);
        //if (response.status.su
        return response;

        


    }

        return (
            <>
                <form onSubmit={handleSubmit}>

                    <TextField
                        required
                        onChange={(e) => handleChange(e)}
                        id="standard-basic" label="Username"
                        name="username"
                        variant="standard"
                        value={userInfo.username}

                        /*form validation*/ 
                        error={userInfo.username === ""}
                        helperText={userInfo.username === "" ? 'Srkiv inn brukernavn' : ''}

                    />
                    <br></br>

                    <TextField
                        required
                        type="password"
                        onChange={(e) => handleChange(e)}
                        id="standard-basic" label="Password"
                        variant="standard" value={userInfo.password}
                        name="password"
                        /*form validation*/ 
                        error={userInfo.password === "" || userInfo.password.length<6}
                        helperText={userInfo.password === "" ? 'Passordet må bestå av minst 6 tegn.' : ''}

                    />

                    <br></br>


                    <Button variant="contained" type="submit">
                        Logg inn
                    </Button>

                </form>
            </>
        );
    }


    export default Login;
