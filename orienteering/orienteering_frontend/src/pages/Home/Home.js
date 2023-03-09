import { useState, useEffect, Component } from "react";
/*var perf = require('./../../');*/
import React from "react";
import useExternalScript from "./useExternalScript.js";
import ComponentWithScript from "../ComponentWithScript.js";
import Test from "./test";
import Login from "../Login/Login.js"
import test from "./../../testscript"


//var __html = require('./../../test.html');
//var template = { __html: __html };
//se på denne
//https://stackoverflow.com/questions/50792942/how-to-import-html-file-into-react-component-and-use-it-as-a-component
////basic spill er i testGame.html



function Home() {
    test();
    //window.test();
    //const externalScript = "../../testscript.js";
    //const state = useExternalScript(externalScript);
    //<div>
    //    {state === "loading" && <p>Loading...</p>}
    //    {state === "ready" && <ComponentWithScript />}
    //    hhh
    //</div>
    return (
        <p id="root1">home shhgdh</p>
    );

    //return (
    //    <div dangerouslySetInnerHTML={template} />

    //);
    //let [htmlFileString, setHtmlFileString] = useState();

    //async function fetchHtml() {
    //    setHtmlFileString(await (await fetch(`../../test.html`)).text());
    //}
    //useEffect(() => {
    //    fetchHtml();
    //}, []);

    //return (
    //    <div className="App">
    //        <div dangerouslySetInnerHTML={{ __html: htmlFileString }}></div>
    //    </div>
    //);


}

export default Home;