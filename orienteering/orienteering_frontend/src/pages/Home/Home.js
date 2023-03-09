import { useState, useEffect, Component } from "react";
/*var perf = require('./../../');*/
import React from "react";
import useExternalScript from "./useExternalScript.js";
import ComponentWithScript from "../ComponentWithScript.js";
import Test from "./test";
import Login from "../Login/Login.js"

//her lastes funksjon fra scriptet inn
import test from "./../../testscript"


//var __html = require('./../../test.html');
//var template = { __html: __html };
//se på denne
//https://stackoverflow.com/questions/50792942/how-to-import-html-file-into-react-component-and-use-it-as-a-component
////basic spill er i testGame.html



function Home() {
    //dette er en js funksjon
    test();



    return (
        <p id="root1">home shhgdh</p>
    );




}

export default Home;