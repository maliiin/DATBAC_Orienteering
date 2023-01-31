import { TextField, Button } from '@mui/material';
import React, { useState } from "react";



//dette er registrer, ikke login!!
function Login() {

    const handleSubmit = (event) => {
        event.preventDefault();

        logInUser()
    }
    const [userInfo, setUserInfo] = useState({
        username: "",
        password: ""
    });


    const handleChange = (event) => {
        console.log("change");
        //update state
        setUserInfo({ ...userInfo, [event.target.name]: event.target.value });
    };

    function logInUser() {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                username: 'test',
                password: 'passord123',
                email: 'testmail@gmail.com'
            })
        };
        fetch('https://localhost:3000/api/user/signinuser', requestOptions)
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


                    <Button variant="contained" type="submit">
                        Logg inn
                    </Button>

                </form>
            </>
        );
    }


    export default Login;
