import * as React from 'react';
import { useState, useEffect } from 'react';
import AppBar from '@mui/material/AppBar';
import { Outlet, Link, useNavigate } from "react-router-dom";

function Layout() {
    const [isSignedIn, setIsSignedIn] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        currentUser()
    }, [])

    const currentUser = async () => {
        var res = await fetch("/api/user/GetSignedInUserId");
        if (res.ok) {
            setIsSignedIn(true);
        } else {
            setIsSignedIn(false);
        }
    }

    const linkStyle = {
        margin: "1rem",
        textDecoration: "none",
        color: 'white',
        flexDirection: 'column',
        justifyContent: 'space-between',
    };

    const linkContainer = {
        height: '40px',
        display: "flex",
    };

    const signOutUser = async () => {
        const url = ("api/user/signOut");
        await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });
        setIsSignedIn(false)
        navigate("login")

    }
    return (
        <>
            <AppBar position="static" style={linkContainer}>
                <div>

                    <Link to="/" style={linkStyle}>Show tracks</Link>

                    {isSignedIn ?
                        (
                            <Link to="" style={linkStyle} onClick={signOutUser}>
                                Sign out
                            </Link>

                        ) : (
                            <>
                                <Link to="/login" style={linkStyle}>Log in</Link>

                                <Link to="/registration" style={linkStyle}>Register</Link>
                            </>
                        )
                    }
                </div>
            </AppBar>

            <Outlet context={[isSignedIn, setIsSignedIn] }></Outlet>

        </>
    )
};

export default Layout;
