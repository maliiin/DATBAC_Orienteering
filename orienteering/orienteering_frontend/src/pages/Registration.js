import { TextField, Button } from '@mui/material';
import React, { useState } from "react";


function Registration() {

    const [userInfo, setUserInfo] = useState({
        username: "",
        email: "",
        password: ""
    });

    const handleChange = (event) => {
        console.log("change");
        //update state
        setUserInfo({ ...userInfo, [event.target.name]: event.target.value });
    };

    const handleSubmit = (event) => {
        event.preventDefault();

        console.log(userInfo);

        addUserToDb();

        //clear input field
        //setUserInfo({ name: "", email: "", phonenumber: "" });
    }

   function addUserToDb() {
        
        const requestOptions = {

            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(userInfo)

        };

        fetch('https://localhost:3000/api/user/createuser', requestOptions)
            .then(response => response.json())
            .then(data => console.log(data));



    }




    return (
        <>
            <form onSubmit={handleSubmit}> 
                
                <TextField
                    onChange={(e) => handleChange(e)}
                    id="standard-basic" label="Username"
                    name="username"
                    variant="standard" value={userInfo.username}

                />
                <br></br>

                <TextField
                    type="password"
                    onChange={(e) => handleChange(e)}
                    id="standard-basic" label="Password"
                    variant="standard" value={userInfo.password}
                    name="password"

                />
                <br></br>

                <TextField
                    onChange={(e) => handleChange(e)}
                    id="standard-basic"
                    label="Email"
                    variant="standard"
                    value={userInfo.email}
                    name="email"
                    
                /><br></br>

                <Button variant="contained" type="submit">
                    Lag bruker
                </Button>

            </form>
        </>
    );
}


export default Registration;
