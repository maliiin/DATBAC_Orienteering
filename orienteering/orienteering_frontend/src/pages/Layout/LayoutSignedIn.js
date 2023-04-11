//import * as React from 'react';
//import AppBar from '@mui/material/AppBar';
//import Box from '@mui/material/Box';
//import Toolbar from '@mui/material/Toolbar';
//import Typography from '@mui/material/Typography';
//import Button from '@mui/material/Button';
//import IconButton from '@mui/material/IconButton';
//import MenuIcon from '@mui/icons-material/Menu';
//import { Outlet, Link } from "react-router-dom";
////fix not in use?
//function LayoutSignedIn() {
//    const linkStyle = {
//        margin: "1rem",
//        textDecoration: "none",
//        color: 'white',
//        flexDirection: 'column',
//        justifyContent: 'space-between',
//    };

//    const signOutUser = async () => {
//        console.log("loggut")
//        await fetch("api/user/signOut");

//    }

//    const linkContainer = {
//        height: '40px',
//        display: "flex",


//    };
//    return (


//        <>
//            <AppBar position="static" style={linkContainer}>
//                <div>



//                    <Link to="/" style={linkStyle}>Vis loyper</Link>
//                    <span onClick={signOutUser} style={linkStyle}>Sign out</span>






//                </div>

//            </AppBar>

//            <Outlet></Outlet>


//        </>
//    )
//};

//export default LayoutSignedIn;
