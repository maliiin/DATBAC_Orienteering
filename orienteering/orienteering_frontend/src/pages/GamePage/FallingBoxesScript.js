"use strict";
var interact = require('interactjs')

//global variables
var fallingWidth = 30;
var fallingHeigth = 30;

var BasketHeight, BasketWidth;
const speed = 1;

//this is position of basket
var positionBasket = {
    x: 0,
    y: 0,
}
var gameCanvas, BasketWidth
var fallingObjects = [];
var gameArea = null

function setup(basketWidth, basketHeight) {
    BasketWidth = basketWidth;
    BasketHeight = basketHeight

    gameCanvas = document.getElementById("gameCanvas");

    //create gameArea
    console.log(gameCanvas)
    //setter "intern" størrelse
    gameCanvas.height = window.screen.height;
    gameCanvas.width = window.screen.width;

    gameArea = new GameArea(gameCanvas);
    console.log(gameArea.canvas)
    //console.log(gameCanvas.style.height);
    //console.log(gameArea.canvas.height);
    //console.log(BasketHeight);

    //set position of basket
    positionBasket = {
        x: 0,
        y: gameCanvas.style.height.replace("px", "") - BasketHeight,
    }
    //console.log(positionBasket.y);
    //start game/initialize
    gameArea.start();

    //make basket able to move
    moveBasket();
    addFallingBox();
    //window.setInterval(addFallingBox, 2000);
}

//https://www.w3schools.com/graphics/tryit.asp?filename=trygame_default_gravity
function GameArea(canvas) {
    this.canvas = canvas;
    console.log(this.canvas.height)
    console.log(this.canvas.width)

    //canvas: document.createElement("canvas"),
    this.start = function () {
        //fix, flytt noe av koden her til setup

        //get context
        this.context = this.canvas.getContext("2d");


        //dont let screen scroll when dragging
        //fix, kun body? trengs tror jeg
        let rootDiv = document.getElementById("root");
        rootDiv.style.touchAction = "none"
        let body1 = document.getElementsByTagName("body")[0];
        body1.style.touchAction = "none"

        this.interval = setInterval(updateGameArea, 20);

    };
    this.clear = function () {
        //clear whole canvas
        this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
        //fix, trengs clear hvis vi har fill bg
        //bacground of canvas
        this.context.fillStyle = "green"
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
            return true;
        }
        return false;
    };

    //check if fallingObject hits the basket
    this.inBasket = function () {
        console.log(this.pos_y);
        //console.log(positionBasket.y);

        if (this.pos_y >= positionBasket.y) {// && ((this.pos_x >= position.x && this.pos_x <= position.x + gameCanvas.canvas.height))){
            console.log("kræsj!!!");
        }
    }
}

function updateGameArea() {
    gameArea.clear()

    //display all falling elements
    for (var i = fallingObjects.length - 1; i >= 0; i--) {
        fallingObjects[i].moveElement();
        let remove = fallingObjects[i].onGround();

        if (remove) {
            //remove element from list
            let firstPart = fallingObjects.slice(0, i);
            let lastPart = fallingObjects.slice(i + 1);
            fallingObjects = firstPart.concat(lastPart);
            return;
        }
        fallingObjects[i].drawElement();
        fallingObjects[i].inBasket();
    }
}


//this function makes basket able to move
function moveBasket() {
    //kilde https://interactjs.io/docs/draggable/ 09/03.23
    interact('.draggable').draggable({

        //gir posisjonen
        listeners: {
            start(event) {
                //console.log(event.type, event.target)
            },
            move(event) {
                positionBasket.x += event.dx
                //console.log(positionBasket.x);
                //console.log(positionBasket.y);

                event.target.style.transform =
                    //`translate(${positionBasket.x}px, ${positionBasket.y}px)`
                    `translate(${positionBasket.x}px, 0px)`

            },
        },
        modifiers: [
            //cannot be dragged outside of parent
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
    //console.log(fallingBox);
    fallingObjects.push(fallingBox);
}

export default setup;