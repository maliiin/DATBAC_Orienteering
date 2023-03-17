import setup from "./LogicGatesScript";
import { useEffect } from "react";
import { TextField, Button, Grid } from '@mui/material';
import "./LogicGatesStyle.css";


export default function LogicGatesGame() {

    useEffect(() => {
        setup();

    }, []);
    

    return (<>

        <div id="gamecontainer">
            <img id="task_background" src={require("./task1_background.png")}
                style={{
                    backgroundColor: "white",

                }}
            ></img>

            <div id="dropzoneUpper" className={'dropzone' + ' ' + 'dropzoneUpper'}
                style={{
                    width: "200px",
                    position: "absolute",
                    //top: canvasHeight * 2 / 3 + "px",
                    //left: canvasWidth / 2 - 100 + "px",

                }}
            ></div>

            <div id="dropzoneLower" className={'dropzone'+ ' ' + 'dropzoneLower'}></div>


            <img id="andgate" className="logicgate" src={require("./AndGate.png")}></img>
            
        </div>

        </>);
}