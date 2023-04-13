import { TextField, Button, Grid } from '@mui/material';
import React, { useState } from "react";
import { useNavigate } from 'react-router-dom';


function Registration() {
    const navigate = useNavigate();

    const [userInfo, setUserInfo] = useState({
        username: "",
        password: "",
        email: ""

    });

    const handleChange = (event) => {
        //update state
        setUserInfo({ ...userInfo, [event.target.name]: event.target.value });
    };

    const handleSubmit = async (event) => {
        event.preventDefault();

        const response = await addUserToDb();

        if (response.ok) {
            navigate("/login");
            //fix-error om ikke ok
            //krav til passord osv!!
        }

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

                            />
                            <br />
                            <br />
                            <Button variant="contained" type="submit">
                                Create user
                            </Button>

                        </form>
                    </Grid>
                </Grid>
            </>
        );
    }
}
export default Registration;
