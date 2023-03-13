import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import { Outlet, Link } from "react-router-dom";

function Layout() {
    const linkStyle = {
        margin: "1rem",
        textDecoration: "none",
        color: 'white',
        flexDirection: 'column',
        justifyContent: 'space-between',




    };

    const linkContainer= {
        height: '40px',
        display: "flex",


    };
    return (


        <>
            <AppBar position="static" style={linkContainer }>
                <div>



                    <Link to="/" style={linkStyle} >Home</Link>

                    <Link to="/login" style={linkStyle}>Log in</Link>

                    <Link to="/registration" style={linkStyle}>Registrer deg</Link>

                    <Link to="/trackoverview" style={linkStyle}>Vis loyper</Link>



                </div>

            </AppBar>

            <Outlet></Outlet>


        </>
    )
};

export default Layout;
