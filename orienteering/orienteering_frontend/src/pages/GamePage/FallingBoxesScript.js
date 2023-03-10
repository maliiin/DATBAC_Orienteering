"use strict";
var interact = require('interactjs')

//global variables
var fallingWidth = 30;
var fallingHeigth = 30;
const data = [["fang", true], ["ikke fang", false], ["fang11", true], ["ikke fang22", false], ["ta", true], ["ikke ta", false]]

var BasketHeight, BasketWidth;
const speed = 3;

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

    //setter "intern" størrelse
    gameCanvas.height = window.screen.height;
    gameCanvas.width = window.screen.width;

    gameArea = new GameArea(gameCanvas);

    //set position of basket
    positionBasket = {
        x: 0,
        y: gameArea.canvas.height - BasketHeight,
    }

    //start game/initialize
    gameArea.start();

    //make basket able to move
    moveBasket();
    addFallingBox();
    //window.setInterval(addFallingBox, 2000);
}

//https://www.w3schools.com/graphics/tryit.asp?filename=trygame_default_gravity
function GameArea(canvas) {
    this.point = 0;
    this.canvas = canvas;
    //console.log(this.canvas.height)
    //console.log(this.canvas.width)

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

function FallingObject(x, y, values) {
    console.log(values)
    this.pos_x = x;
    this.pos_y = y;
    this.text = values[0];
    this.collect = values[1];

    console.log(this.text)
    console.log(this.collect)

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
        //console.log(this.pos_y);
        //console.log(positionBasket.y);

        if ((this.pos_y + this.height >= positionBasket.y) &&
            (this.pos_x + this.width >= positionBasket.x) &&
            (this.pos_x <= positionBasket.x + gameArea.canvas.height)) {

            if (this.collect) {
                gameArea.point += 1;
            }
            return true;

        }
        return false;
    }
}

function updateGameArea() {
    gameArea.clear();
    //display score
    gameArea.context.font = "30px Arial";
    gameArea.context.strokeText("Poeng: " + gameArea.point, 50, 50);

    //display all falling elements
    for (var i = fallingObjects.length - 1; i >= 0; i--) {
        fallingObjects[i].moveElement();

        //remove element 
        let remove = fallingObjects[i].onGround() || fallingObjects[i].inBasket()
        if (remove) {

            console.log("remove")
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

    let value = data[Math.floor(Math.random() * data.length)];


    let fallingBox = new FallingObject(x, 0, value);

    //console.log(fallingBox);
    fallingObjects.push(fallingBox);
}

export default setup;