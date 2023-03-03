import { TextField, Button } from '@mui/material';
import React, { useState } from "react";
import {useNavigate } from 'react-router-dom';


function Registration() {
    const navigate = useNavigate();

    const [userInfo, setUserInfo] = useState({
        username: "",
        password: "",
        email: ""

    });

    const handleChange = (event) => {
       // console.log("change");
        //update state
        setUserInfo({ ...userInfo, [event.target.name]: event.target.value });
    };

    const handleSubmit = async (event) => {
        console.log("handle submut registrer")

        event.preventDefault();
        console.log("handle submut registrer")

        console.log(userInfo);

        const response = await addUserToDb();
        
        if (response.ok)
        {
            navigate("/login");
        } else
        {
            console.log("not ok");
        }

        //clear input field
        //setUserInfo({ name: "", email: "", phonenumber: "" });
    }

    const addUserToDb=async ()=> {
        console.log("user to be created");
        console.log(userInfo);
        const requestOptions = {

            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(userInfo)

        };

        var response = await fetch('/api/user/createuser', requestOptions);
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
                    variant="standard" value={userInfo.username}

                />
                <br></br>

                <TextField
                    required
                    type="password"
                    onChange={(e) => handleChange(e)}
                    id="standard-basic" label="Password"
                    variant="standard" value={userInfo.password}
                    name="password"
                    inputProps={{ minLength: 6 }}


                />
                <br></br>

                <TextField
                    required
                    onChange={(e) => handleChange(e)}
                    id="standard-basic"
                    label="Email"
                    variant="standard"
                    value={userInfo.email}
                    name="email"
                    type="email"
                    
                /><br></br>

                <Button variant="contained" type="submit">
                    Lag bruker
                </Button>

            </form>
        </>
    );
}


export default Registration;