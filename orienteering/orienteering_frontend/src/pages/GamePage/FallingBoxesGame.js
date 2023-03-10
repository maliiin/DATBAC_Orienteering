import setup from "./FallingBoxesScript";
import { useEffect } from "react";


export default function FallingBoxesGame() {
    const basketWidth = 200;



    useEffect(() => {
        setup(basketWidth);
    }, []);







    return (<>

            <div
                id="basket"
                className="draggable"
                style={{
                    //position: "absolute",
                    bottom: "20px",
                    width: basketWidth+"px",
                    backgroundColor: "lightblue",
                    bottom: "20px"

                }}


            >
                Draggable Element
            </div>
    </>);
}