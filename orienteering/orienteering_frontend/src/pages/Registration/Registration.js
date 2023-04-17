import { TextField, Button, Grid } from '@mui/material';
import React, { useState } from "react";
import { useNavigate } from 'react-router-dom';

function Registration() {
    const navigate = useNavigate();
    const [errorMsg, setErrorMsg] = useState("");


    const [userInfo, setUserInfo] = useState({
        username: "",
        password: "",
        email: ""

    });

    const handleChange = (event) => {
        setUserInfo({ ...userInfo, [event.target.name]: event.target.value });
    };

    const addUserToDb = async () => {
        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(userInfo)
        };

        const response = await fetch('/api/user/createuser', requestOptions);
        return response;
    }

    const handleSubmit = async (event) => {
        event.preventDefault();

        const response = await addUserToDb();

        if (response.ok) {
            navigate("/login");
        }
        else {
            setErrorMsg("Username or email already taken")
        }
    }
    return (
        <>
            <Grid
                container
                spacing={0}
                direction="column"
                alignItems="center"
                justifyContent="center"
                style={{ minHeight: '50vh' }}>
                <Grid item xs={3}>
                    <h4>Sign up</h4>
                    <form onSubmit={handleSubmit}>

                        <TextField
                            required
                            onChange={(e) => handleChange(e)}
                            label="Username"
                            name="username"
                            variant="standard" value={userInfo.username}
                            error={errorMsg == "" ? false : true}
                        />
                        <br></br>

                        <TextField
                            required
                            type="password"
                            onChange={(e) => handleChange(e)}
                            label="Password"
                            variant="standard" value={userInfo.password}
                            name="password"
                            inputProps={{ minLength: 6 }}
                            error={errorMsg == "" ? false : true}
                        />
                        <br></br>

                        <TextField
                            required
                            onChange={(e) => handleChange(e)}
                            label="Email"
                            variant="standard"
                            value={userInfo.email}
                            name="email"
                            type="email"
                            error={errorMsg == "" ? false : true}
                        />
                        <br />
                        <br />
                       
                        <Button variant="contained" type="submit">
                            Create user
                        </Button>

                    </form>
                </Grid>
                {errorMsg}
            </Grid>
        </>
    );
}
export default Registration;
