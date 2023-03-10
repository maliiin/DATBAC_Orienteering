const fallingWidth = 30;
const fallingHeigth = 30;
const speed = 5;
//this is position of basket
const position = { x: 0, y: 0 }
var interact = require('interactjs')
var gameCanvas, basketWidth

function setup(basketWidth) {
    gameCanvas = document.getElementById("gameCanvas");
    basketWidth = basketWidth;

    moveBasket();
    addFallingBox();
    //dette skal være med
    //window.setInterval(addFallingBox, 2000);
}


//https://www.w3schools.com/graphics/tryit.asp?filename=trygame_default_gravity
var GameArea = {
    //canvas: gameCanvas,
    canvas: document.createElement("canvas"),
    start: function () {

        this.canvas.width = window.screen.availWidth-50
        this.canvas.height = window.screen.availHeight-500
        this.context = this.canvas.getContext("2d");

        let rootDiv = document.getElementById("root");
        rootDiv.style.width = (window.screen.availWidth - 50) + "px"
        rootDiv.style.touchAction = "none"
        let body = document.getElementsByTagName("body");
        body.style.touchAction="none"



        //rootDiv.style.height = "600px";
        //rootDiv.style.width = "600px";

        console.log(rootDiv.style.top);
        //rootDiv.appendChild(this.canvas)
        document.body.insertBefore(this.canvas, document.body.childNodes[0]);
        this.interval = setInterval(updateGameArea, 20);

    },
    clear: function () {
        //clear whole canvas
        this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
    }
};

var fallingObjects = [];


GameArea.start();


function FallingObject(x, y) {
    this.pos_x = x;
    this.pos_y = y;

    this.width = fallingWidth;
    this.height = fallingHeigth;

    this.color = "black";

    this.moveElement = function () {
        this.pos_y += speed;
    };

    this.drawElement = function () {
        var ctx = GameArea.context;
        ctx.fillStyle = this.color;
        ctx.fillRect(this.pos_x, this.pos_y, this.width, this.height);
    };

    this.onGround = function () {
        var bottom = GameArea.canvas.height - this.height;
        if (this.pos_y >= bottom) {
            console.log("hit bottom")
            return true;
        }
        return false;
    };

    //check if fallingObject hits the basket
    this.inBasket = function () {

    }




}

function updateGameArea() {
    GameArea.clear()
    //let i = 0;
    //for (i in fallingObjects) {
    //    console.log(i);
    //    fallingObjects[i].moveElement();
    //    fallingObjects[i].drawElement();
    //    let remove = fallingObjects[i].onGround();

    //    if (remove) {
    //        console.log("remove element");
    //        console.log(fallingObjects)
    //        //remove element from list
    //        let firstPart = fallingObjects.slice(0, i);
    //        let lastPart = fallingObjects.slice(i+1);
    //        fallingObjects = firstPart.concat(lastPart);
    //        console.log(fallingObjects)
    //    }
    //}

    //display all falling elements
    for (var i = fallingObjects.length - 1; i >= 0; i--) {
        fallingObjects[i].moveElement();
        let remove = fallingObjects[i].onGround();

        if (remove) {
            console.log("remove element");
            console.log(fallingObjects)
            //remove element from list
            let firstPart = fallingObjects.slice(0, i);
            let lastPart = fallingObjects.slice(i + 1);
            fallingObjects = firstPart.concat(lastPart);
            console.log(fallingObjects)
            return;
        }

        fallingObjects[i].drawElement();

    }



}

//fiks sjekk her. elementer blir ikke fjernet



function controllPosition() {
    var basket = document.getElementById("basket");
    //console.log(basket.offsetLeft);
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

                event.target.style.transform =
                    `translate(${position.x}px, ${position.y}px)`
            },
        },
        modifiers: [
            interact.modifiers.restrictRect({
                restriction:'parent'
                })
            ]
    })
}

function addFallingBox() {
    let max = GameArea.canvas.width - fallingWidth;
    let x = Math.floor(Math.random() * max);
    let fallingBox = new FallingObject(x, 0);
    fallingObjects.push(fallingBox);
}

export default setup;