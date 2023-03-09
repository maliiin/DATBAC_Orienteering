
const position = { x: 0, y: 0 }
var interact = require('interactjs')
var gameCanvas

function setup() {
    console.log("setup")
    GameDiv = document.getElementById("GameDiv");
    gameCanvas = document.getElementById("gameCanvas")
    console.log(gameCanvas)
    console.log("hei");
    moveBasket();
    addFallingBox();
}


//https://www.w3schools.com/graphics/tryit.asp?filename=trygame_default_gravity
var GameArea = {
    //canvas: gameCanvas,
    canvas:document.createElement("canvas"),
    start: function () {


        this.canvas.width = 480;
        this.canvas.height = 270;
        this.context = this.canvas.getContext("2d");

        document.body.insertBefore(this.canvas, document.body.childNodes[0]);
        this.interval = setInterval(updateGameArea, 2000);


        
    },
    clear: function () {
        this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
        console.log("clear canvas")
    }
};
var fallingObjects = [];


GameArea.start();


function FallingObject(x, y) {
    //this.div1 = "";
    this.pos_x = x;
    this.pos_y = y;

    //let div = document.createElement("div");
    //div.style.width = "50px";
    //div.style.height = "50px";
    //div.style.backgroundColor = "blue";

    //div.style.left = this.pos_x;
    //div.style.top = this.pos_y;
    this.width = 50
    this.height=50


    this.color = "black";

    this.moveElement=function(){
        this.pos_y += 10;
    };


    this.drawElement = function () {
        var ctx = GameArea.context;
        ctx.fillStyle = this.color;
        ctx.fillRect(this.pos_x, this.pos_y, this.width, this.height);


        //console.log("movelelement");
        //console.log(this.pos_y);
        //this.pos_y += 50;
        //this.div1.style.top = this.pos_y + "px";
        //this.div1.innerHTML = "hei";
        ////return "hei22";

    }

}

var fallingObject=new FallingObject(150,50)

function updateGameArea() {
    console.log("update area")
    GameArea.clear()
    fallingObject.moveElement();
    fallingObject.drawElement();
    console.log(fallingObject)
}
//window.setInterval(updateGameArea, 1000);

let GameDiv = null;

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
    //window.setInterval((fallingBox) => { fallingBox.div1.style.top = fallingBox.pos_y },(fallingBox), 2000);
    fallingBox.moveElement();
    fallingBox.moveElement();
    fallingBox.moveElement();
    fallingBox.moveElement();
    fallingBox.moveElement();

    const textnode = document.createTextNode("Water");
    let div = document.createElement("div")
    div.style.width = "50px";
    div.style.height = "50px";
    div.style.backgroundColor = "blue";
    GameDiv.appendChild(div);

}

export default setup;