"use strict";
var interact = require('interactjs')

//global variables
var fallingWidth = 30;
var fallingHeigth = 30;
const speed = 3;

//this is position of basket
const position = { x: 0, y: 0 }
var gameCanvas, BasketWidth
var fallingObjects = [];
var gameArea=null

function setup(basketWidth) {
    BasketWidth = basketWidth;

    gameCanvas = document.getElementById("gameCanvas");

    //create gameArea
    gameArea = new GameArea(gameCanvas);

    //start game/initialize
    gameArea.start();

    //make basket able to move
    moveBasket();
    window.setInterval(addFallingBox, 2000);
}

//https://www.w3schools.com/graphics/tryit.asp?filename=trygame_default_gravity
function GameArea(canvas) {
    this.canvas = canvas;
    //canvas: document.createElement("canvas"),
    this.start = function () {
        //fix, flytt noe av koden her til setup

        //setup canvas
        //dette skjer i react nå
        //this.canvas.width = window.screen.availWidth - 50
        //this.canvas.height = window.screen.availHeight - 500
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
        console.log(this.height)
        var ctx = gameArea.context;
        ctx.fillStyle = this.color;
        ctx.fillRect(this.pos_x, this.pos_y, this.width, this.height);
    };

    this.onGround = function () {
        console.log(gameArea)
        console.log(gameArea.canvas)
        console.log(gameArea.canvas.height)
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
    //kilde https://interactjs.io/docs/draggable/ 09/03.23
    interact('.draggable').draggable({

        //gir posisjonen
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
            //cannot be dragged outside of parent
            interact.modifiers.restrictRect({
                restriction: 'parent'
            })
        ]
    })
}

function addFallingBox() {
    console.log("added falling")
    let max = gameArea.canvas.width - fallingWidth;
    let x = Math.floor(Math.random() * max);
    let fallingBox = new FallingObject(x, 0);
    console.log(fallingBox);
    fallingObjects.push(fallingBox);
}

export default setup;