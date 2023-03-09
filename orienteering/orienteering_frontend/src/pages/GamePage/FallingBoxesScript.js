
const position = { x: 0, y: 0 }
var interact = require('interactjs')

class FallingObject {
    div1 = "";
    constructor(pos_x, pos_y) {
        this.pos_x = pos_x;
        this.pos_y = pos_y;

        let div = document.createElement("div");
        div.style.width = "50px";
        div.style.height = "50px";
        div.style.backgroundColor = "blue";

        div.style.left = this.pos_x;
        div.style.top = this.pos_y;


        this.div1 = div;
        

    }

    moveElement() {
        console.log("movelelement");
        this.pos_y += 5;
        this.div1.style.top = this.pos_y;

    }
        
}


let GameDiv = null;
function setup() {
    GameDiv = document.getElementById("GameDiv");
    console.log(GameDiv);
    moveBasket();
    addFallingBox();
}

function controllPosition() {
    var basket = document.getElementById("basket");
    console.log(basket.offsetLeft);
    //console.log(basket.position.dx);


}


//this function makes basket able to move
function moveBasket() {
    //controllPosition();
    //kilde https://interactjs.io/docs/draggable/ 09/03.23
    interact('.draggable').draggable({

        //gir posisjonen
        //onmove: () => { console.log(position.x) },
        listeners: {
            start(event) {
                console.log("start   lll")
                console.log(event.type, event.target)
            },
            move(event) {
                position.x += event.dx
                //position.y += event.dy
                

                event.target.style.transform =
                    `translate(${position.x}px, ${position.y}px)`
               
            },

        }
    })
}
//window.setInterval(addFallingBox, 200000);

function addFallingBox() {
    console.log("hei")
    let fallingBox = new FallingObject(50, 50);
    //make box fall
    window.setInterval(fallingBox.moveElement(), 1000);

    const textnode = document.createTextNode("Water");
    let div=document.createElement("div")
    div.style.width = "50px";
    div.style.height = "50px";
    div.style.backgroundColor = "blue";
    GameDiv.appendChild(div);

}

export default setup;