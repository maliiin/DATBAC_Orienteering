import moveBasket from "./FallingBoxesScript";

export default function FallingBoxes() {
    moveBasket();

    return (<>
        
        <div
            className="draggable"
            style={{
                position: "absolute",
                bottom: "20px",
                width: "200px",
                backgroundColor: "lightblue"

            }}
            
        >
            Draggable Element
        </div>
    </>);
}