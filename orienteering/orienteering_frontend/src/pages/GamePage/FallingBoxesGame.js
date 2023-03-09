import setup from "./FallingBoxesScript";
import { useEffect } from "react";


export default function FallingBoxesGame() {




    useEffect(() => {
        setup();
    }, []);







    return (<>
        <div
            id="GameDiv"
            style={{
                width: "700px",
                height: "500px",
                backgroundColor: "red",


            }}>heihe
            <div
                id="basket"
                className="draggable"
                style={{
                    //position: "absolute",
                    bottom: "20px",
                    width: "200px",
                    backgroundColor: "lightblue",
                    bottom: "20px"

                }}


            >
                Draggable Element
            </div>
        </div>
    </>);
}