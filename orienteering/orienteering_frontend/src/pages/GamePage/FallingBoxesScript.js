"use strict";
var interact = require('interactjs')
//import { FallingObject } from "./FallingObjectClass.js";

//fiks til neste gang
//la brukeren miste liv- ha 3
//lag gameStatus klasse
//når spill er slut vis score
//ha info om spill


//global variables
var fallingWidth = 30;
var fallingHeigth = 30;
const data = [["ikke", true]];  //[["fang", true], ["ikke fang", false], ["fang11", true], ["ikke fang22", false], ["ta", true], ["ikke ta", false]];
//fix endre denne
const totalLives = 1;
var BasketHeight, BasketWidth;
const speed = 3;

//this is position of basket
var positionBasket = {
    x: 0,
    y: 0,
}
var gameCanvas, BasketWidth
var fallingObjects = [];
var gameArea = null,
    gameStatus = null


function setup(basketWidth, basketHeight) {
    //get data from react
    BasketWidth = basketWidth;
    BasketHeight = basketHeight

    gameCanvas = document.getElementById("gameCanvas");

    //setter "intern" størrelse
    gameCanvas.height = window.screen.height;
    gameCanvas.width = window.screen.width;

    //create gamee area and game status
    gameArea = new GameArea(gameCanvas);
    gameStatus = new GameStatus();

    //set position of basket
    positionBasket = {
        x: 0,
        y: gameArea.canvas.height - BasketHeight,
    }

    //start game/initialize
    gameArea.start();


    //make basket able to move
    moveBasket();

    //add fallingf objects
    addFallingBox();

    //window.setInterval(addFallingBox, 2000);
}

function GameStatus() {
    this.points = 0;
    this.lives = totalLives;
    this.gameOver = false;

    this.looseLife = function () {
        console.log("lose life")
        //only one life left-->game over
        if (this.lives <= 1) {
            this.gameOver = true
            return;
        }
        //else lose a life
        this.lives--;

    }
}

//https://www.w3schools.com/graphics/tryit.asp?filename=trygame_default_gravity
function GameArea(canvas) {
    //fix-flytt til game
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
        //console.log("sjekk om bakjke")
        var bottom = gameArea.canvas.height - this.height;
        if (this.pos_y >= bottom) {
            console.log("bunn----------------")
            if (this.collect) {
                console.log("skulle ha blitt tatt-->mister liv")
                //should have collected this
                gameStatus.looseLife();
            }
            console.log("skulle ikke blitt tatt")
            return true;
        }
        return false;
    };

    //check if fallingObject hits the basket
    this.inBasket = function () {
        //console.log("sjakk om fanget")
        //console.log(this.pos_y);
        //console.log(positionBasket.y);

        //console.log((this.pos_y + this.height >= positionBasket.y))
        //console.log(this.pos_x + this.width >= positionBasket.x)
        //console.log(this.pos_x <= positionBasket.x + BasketWidth)

        if ((this.pos_y + this.height >= positionBasket.y) &&
            (this.pos_x + this.width >= positionBasket.x) &&
            (this.pos_x <= positionBasket.x + BasketWidth)) {
            console.log("denne ble fanget")


            if (this.collect) {
                gameArea.point += 1;
            } else {
                //lost a life
                gameStatus.looseLife();
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

        let remove = fallingObjects[i].onGround() || fallingObjects[i].inBasket()

        //remove element 
        if (remove) {

            console.log("remove")
            //remove element from list
            let firstPart = fallingObjects.slice(0, i);
            let lastPart = fallingObjects.slice(i + 1);
            fallingObjects = firstPart.concat(lastPart);
            //return;
        } else {
            //dont remove-> draw it
            fallingObjects[i].drawElement();
            //fallingObjects[i].inBasket();

        }
    }

    //check is game is over kanskje flytt en opp? fix
    if (gameStatus.gameOver) {
        gameArea.context.strokeText("Game over: " + gameArea.point, 50, 200);

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
    let maxX = gameArea.canvas.width - fallingWidth;
    let x = Math.floor(Math.random() * maxX);

    let value = data[Math.floor(Math.random() * data.length)];

    //fix
    let fallingBox = new FallingObject(x, 0, value);

    fallingObjects.push(fallingBox);
}

export default setup;