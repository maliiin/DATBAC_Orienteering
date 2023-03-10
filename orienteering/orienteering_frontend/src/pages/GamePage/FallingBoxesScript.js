"use strict";


const fallingWidth = 30;
const fallingHeigth = 30;
const speed = 5;

//this is position of basket
const position = { x: 0, y: 0 }
var interact = require('interactjs')
var gameCanvas, BasketWidth
var fallingObjects = [];

var gameArea=null

function setup(basketWidth) {
    //gameCanvas = document.getElementById("gameCanvas");
    BasketWidth = basketWidth;

    gameCanvas = document.getElementById("gameCanvas");

    //create gameArea
    gameArea = new GameArea(gameCanvas);
    //start game/initialize
    gameArea.start();

    //make basket able to move
    moveBasket();

    addFallingBox();


    console.log(gameCanvas)
    console.log(document.getElementById("basket"));


    //dette skal være med
    //window.setInterval(addFallingBox, 2000);
}

//window.onload = loadfunk();

//function loadfunk() {
//    let rootDiv = document.getElementById("root");
//    console.log(document.getElementById("gameCanvas"));
//    console.log(document.getElementById("basket"));

//    //console.log(rootDiv)
//    //rootDiv.appendChild(document.createElement("div"));

//    //let basket = document.createElement("div")
//    //basket.id = "basket"
//    //basket.className = "draggable"
//    //basket.style.position = "absolute";
//    //basket.style.width = 200 + "px";
//    //basket.style.backgroundColor = "lightblue";
//    //basket.style.bottom = "200px"
//    //basket.style.height = "50px"

//    //let basket=document.getElementById("basket")
//    //console.log(basket)
//    //rootDiv.appendChild(basket);
//    //console.log(rootDiv.childElementCount);


//    //rootDiv.reload(false)


//}




//https://www.w3schools.com/graphics/tryit.asp?filename=trygame_default_gravity
//var GameArea = {
//    canvas: gameCanvas,
//    //canvas: document.createElement("canvas"),
//    start: function () {
//        //fix, flytt noe av koden her til setup

//        //setup canvas
//        this.canvas.width = window.screen.availWidth - 50
//        this.canvas.height = window.screen.availHeight - 500
//        this.context = this.canvas.getContext("2d");

//        //this.canvas.style.position = "absolute"

//        //dont let whole screen scroll
//        let rootDiv = document.getElementById("root");
//        rootDiv.style.width = (window.screen.availWidth - 50) + "px"
//        rootDiv.style.touchAction = "none"
//        let body1 = document.getElementsByTagName("body")[0];
//        body1.style.touchAction = "none"
//        //rootDiv.insertBefore(this.canvas, rootDiv.firstChild)
//        //rootDiv.appendChild(document.createElement("div"))
//        //rootDiv.reload(false)
//        //rootDiv.insertBefore(this.canvas, rootDiv.firstChild)
//        //rootDiv.appendChild(this.canvas)

//        //rootDiv.style.position = "relative"
//        document.body.insertBefore(this.canvas, document.body.childNodes[0]);
//        //document.body.style.position="relative"
//        this.interval = setInterval(updateGameArea, 20);

//    },
//    clear: function () {
//        //clear whole canvas
//        this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
//        this.context.fillStyle = "pink"
//        this.context.fillRect(0, 0, this.canvas.width, this.canvas.height)
//    }
//};

function GameArea(canvas) {
    this.canvas = canvas;
    //canvas: document.createElement("canvas"),
    this.start = function () {
        //fix, flytt noe av koden her til setup

        //setup canvas
        this.canvas.width = window.screen.availWidth - 50
        this.canvas.height = window.screen.availHeight - 500
        this.context = this.canvas.getContext("2d");

        //this.canvas.style.position = "absolute"

        //dont let whole screen scroll
        let rootDiv = document.getElementById("root");
        rootDiv.style.width = (window.screen.availWidth - 50) + "px"
        rootDiv.style.touchAction = "none"
        let body1 = document.getElementsByTagName("body")[0];
        body1.style.touchAction = "none"
        //rootDiv.insertBefore(this.canvas, rootDiv.firstChild)
        //rootDiv.appendChild(document.createElement("div"))
        //rootDiv.reload(false)
        //rootDiv.insertBefore(this.canvas, rootDiv.firstChild)
        //rootDiv.appendChild(this.canvas)

        //rootDiv.style.position = "relative"
        document.body.insertBefore(this.canvas, document.body.childNodes[0]);
        //document.body.style.position="relative"
        this.interval = setInterval(updateGameArea, 20);

    };
    this.clear = function () {
        //clear whole canvas
        this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
        this.context.fillStyle = "pink"
        this.context.fillRect(0, 0, this.canvas.width, this.canvas.height)
    };
}



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
        var ctx = gameArea.context;
        ctx.fillStyle = this.color;
        ctx.fillRect(this.pos_x, this.pos_y, this.width, this.height);
    };

    this.onGround = function () {
        var bottom = gameArea.canvas.height - this.height;
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
    //console.log(gameArea)
    gameArea.clear()

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
                restriction: 'parent'
            })
        ]
    })
}

function addFallingBox() {
    let max = gameArea.canvas.width - fallingWidth;
    let x = Math.floor(Math.random() * max);
    let fallingBox = new FallingObject(x, 0);
    fallingObjects.push(fallingBox);
}

export default setup;