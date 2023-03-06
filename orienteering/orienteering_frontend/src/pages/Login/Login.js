import { TextField, Button, Grid } from '@mui/material';
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
        password: ""
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


                <Grid
                    container
                    spacing={0}
                    direction="column"
                    alignItems="center"
                    justifyContent="center"
                    style={{ minHeight: '50vh' }}
                >

                    <Grid item xs={3}>
                        <form onSubmit={handleSubmit}>
                            <h4>Logg inn</h4>

                            <TextField
                                required
                                onChange={(e) => handleChange(e)}
                                id="standard-basic" label="Username"
                                name="username"
                                variant="standard"
                                value={userInfo.username}
                            />
                            <br></br>

                            <TextField
                                required
                                type="password"
                                onChange={(e) => handleChange(e)}
                                id="standard-basic" label="Password"
                                variant="standard" value={userInfo.password}
                                name="password"
                            />

                            <br></br>
                            <br></br>



                            <Button variant="contained" type="submit">
                                Logg inn
                            </Button>

                        </form>
                    </Grid>

                </Grid> 
            </>
        );
    }

    export default Login;
