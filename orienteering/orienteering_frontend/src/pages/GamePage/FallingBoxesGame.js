import setup from "./FallingBoxesScript";
import { useEffect } from "react";


export default function FallingBoxesGame() {
    const basketWidth = 200;
    const basketHeight = 100;

    const canvasWidth = window.screen.availWidth;
    const canvasHeight = window.screen.availHeight;

    useEffect(() => {
        //setup game in js
        setup(basketWidth);
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
                    width: canvasWidth+"px",
                    height: canvasHeight+"px",
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
                    top: canvasHeight-basketHeight,
                }}
            >
         
            </div>

        </div>



    </>);
}