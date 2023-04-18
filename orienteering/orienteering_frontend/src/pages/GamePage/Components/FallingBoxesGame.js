import setup from "../Scripts/FallingBoxesScript";
import { Button } from '@mui/material';


export default function FallingBoxesGame() {
    const basketWidth = 100;
    const basketHeight = 50;

    const canvasWidth = window.screen.availWidth;

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
                    height: window.innerHeight,
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
                    top: window.innerHeight - basketHeight,
                    display: "none"
                }}
            >
            </div>


            <div id="beforeGameDiv" style={{ margin: "10px" }}>
                <h3>Prime number Game</h3>
                <p >
                    Catch the falling prime numbers. Catching a non-prime number results in losing 1 HP, so does not catching a prime number.
                    You got 3 HP. Start the game by clicking the button below.
                </p>
                <br></br>
                <br></br>

                <Button
                    id="startGame"
                    style={{
                        width: "200px",
                        position: "absolute",
                        left: canvasWidth / 2 - 100 + "px",
                        backgroundColor: "white",
                    }}
                    onClick={test}
                >
                    Start Game
                </Button></div>
        </div>
    </>);
}