import setup from "./FallingBoxesScript";
import { useEffect } from "react";
import { TextField, Button, Grid } from '@mui/material';


export default function FallingBoxesGame() {
    const basketWidth = 100;
    const basketHeight = 50;

    const canvasWidth = window.screen.availWidth;
    const canvasHeight = window.screen.availHeight;


    function test() {
        setup(basketWidth, basketHeight);
    }

    return (<>
        <div id="container"
            style={{
                position: "relative"

            }}
        >
            <canvas
                id="gameCanvas"
                style={{
                    position: "absolute",
                    width: canvasWidth + "px",
                    //height: canvasHeight + "px",
                    //height:"100vh",   //denne blir dekket av bunnmeny
                    height:window.innerHeight,
                    backgroundColor: "green",
                    display: "none"
                }}
            ></canvas>

            <div
                id="basket"
                className="draggable"
                style={{
                    position: "absolute",

                    width: basketWidth + "px",
                    height: basketHeight + "px",

                    backgroundColor: "lightblue",
                    //top: canvasHeight - basketHeight,
                    top: window.innerHeight - basketHeight,


                    display: "none"
                }}
            >

            </div>
            

            <div id="beforeGameDiv" style={{ margin: "10px" }}>
                <h3>Primtallspill</h3>
                <p >
                    Spillet går ut på å fange primtallene som faller ned.
                    Du har 3 liv. Start spillet ved å trykke på knappen under.
                </p>
                <br></br>
                <br></br>

                <Button
                    id="startGame"
                    style={{
                        width: "200px",
                        position: "absolute",
                        //top: canvasHeight * 2 / 3 + "px",
                        left: canvasWidth/2 - 100 + "px",
                        backgroundColor: "white",

                    }}

                    onClick={test}
                >
                    Start spill
                </Button></div>




        </div>



    </>);
}