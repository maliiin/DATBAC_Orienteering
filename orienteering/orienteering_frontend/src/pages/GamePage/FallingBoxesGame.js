import setup from "./FallingBoxesScript";
import { useEffect } from "react";


export default function FallingBoxesGame() {
    const basketWidth = 200;



    useEffect(() => {
        setup(basketWidth);
    }, []);






    
    return (<>
        <div id="container"
            style={{
                position: "relative"
                } }
        >
            <canvas
                id="gameCanvas"
                style={{
                    position:"absolute",
                    width: "200px",
                    height: "200px",
                    backgroundColor: "green"
                }}
            ></canvas>
            <div
                id="basket"
                className="draggable"
                style={{
                    position: "absolute",
                    width: basketWidth + "px",
                    backgroundColor: "lightblue",
                    //bottom: "10px",
                    top:"50px",
                    height: "50px",

                }}



            >
                Draggable Element
            </div>

        </div>


     
    </>);
}