import setup from "./FallingBoxesScript";
import { useEffect } from "react";
import { TextField, Button, Grid } from '@mui/material';



export default function FallingBoxesGame() {
    const basketWidth = 100;
    const basketHeight = 50;

    const canvasWidth = window.screen.availWidth;
    const canvasHeight = window.screen.availHeight;

    function directions() {
        console.log("vei  1111111111111111111");
    }

    useEffect(() => {
        //setup game in js
        setup(basketWidth, basketHeight);
    }, []);

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
                    height: canvasHeight + "px",
                    backgroundColor: "green"
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
                    top: canvasHeight - basketHeight,
                }}
            >

            </div>
            <Button
                id="directionsButton"
                style={{
                    width: "200px",
                    display: "none",
                    position: "absolute",
                    top: canvasHeight * 2 / 3+"px",
                    left: canvasWidth - 200 + "px",
                    backgroundColor: "white",

                }}
                onClick={directions}
            >
                veibeskrivelse
            </Button>



        </div>



    </>);
}