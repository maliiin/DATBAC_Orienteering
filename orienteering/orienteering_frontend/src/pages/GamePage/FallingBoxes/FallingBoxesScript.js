"use strict";
import Data from "./Data.js";

var interact = require('interactjs')

//global variables
var fallingWidth = 30;
var fallingHeigth = 30;
const totalLives = 3;
var BasketHeight, BasketWidth;
const speed = 3;
var gameCanvas, BasketWidth;
var fallingObjects = [];
var gameArea = null,
    gameStatus = null;
var fallingTimer = null;

//this is position of basket
var positionBasket = {
    x: 0,
    y: 0,
}

function setup(basketWidth, basketHeight) {
    //display and hide elements
    //fix- her kunne klasser istedenfor id blitt brukt kanskje??
    gameCanvas = document.getElementById("gameCanvas");
    gameCanvas.style.display = "block";
    var basket = document.getElementById("basket");
    basket.style.display = "block";
    var gameInfo = document.getElementById("beforeGameDiv");
    gameInfo.style.display = "none";


    //get data from react
    BasketWidth = basketWidth;
    BasketHeight = basketHeight


    //setter "intern" størrelse av canvas
    //gameCanvas.height = window.screen.availHeight;
    gameCanvas.height = window.innerHeight;
    gameCanvas.width = window.screen.availWidth;

    //create game area and game status objects
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

    //add falling objects
    fallingTimer = window.setInterval(addFallingBox, 2000);
}

function GameStatus() {
    this.points = 0;
    this.lives = totalLives;
    this.gameOver = false;

    this.looseLife = function () {
        //only one life left-->game over
        if (this.lives <= 1) {
            this.gameOver = true;
            this.endGame();
            return;
        }
        //else lose a life
        this.lives--;

    }
    this.endGame = function () {
        //clear timers
        clearTimeout(fallingTimer);
        clearTimeout(gameArea.interval);

        //display score
        gameArea.clear()
        gameArea.context.strokeText("Spill slutt, du fikk " + gameStatus.points + " poeng.", gameArea.canvas.width / 2, gameArea.canvas.height / 2);

        //display button to next checkpoint directions
        var navigationButton = document.getElementById("navigationButton");
        navigationButton.style.display = "block";
        navigationButton.style.top = GameArea.canvas.height * 2 / 3 - 100 + "px";
        navigationButton.style.left = GameArea.canvas.height / 2  - 100 + "px";


        //remove basket
        var basket = document.getElementById("basket");
        basket.style.display = "none";

    }

}

//https://www.w3schools.com/graphics/tryit.asp?filename=trygame_default_gravity
function GameArea(canvas) {
    this.canvas = canvas;

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
    this.pos_x = x;
    this.pos_y = y;
    this.text = values[0];
    this.collect = values[1];
    this.width = fallingWidth;
    this.height = fallingHeigth;
    this.color = "white";

    this.moveElement = function () {
        this.pos_y += speed;
    };

    this.drawElement = function () {
        var ctx = gameArea.context;
        ctx.fillStyle = this.color;
        ctx.fillRect(this.pos_x, this.pos_y, this.width, this.height);
        ctx.fillStyle = "black";
        ctx.textAlign = "center";
        ctx.textBaseline = "middle";
        gameArea.context.font = "1rem Arial";
        gameArea.context.fillText(this.text, this.pos_x + this.width / 2, this.pos_y + this.height / 2, this.width);
    };

    this.onGround = function () {
        var bottom = gameArea.canvas.height - this.height;
        if (this.pos_y >= bottom) {
            if (this.collect) {
                //should have collected this
                gameStatus.looseLife();
            }
            return true;
        }
        return false;
    };

    //check if fallingObject hits the basket
    this.inBasket = function () {

        if ((this.pos_y + this.height >= positionBasket.y) &&
            (this.pos_x + this.width >= positionBasket.x) &&
            (this.pos_x <= positionBasket.x + BasketWidth)) {

            if (this.collect) {
                gameStatus.points += 10;
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
    gameArea.context.font = "1em Arial";
    gameArea.context.fillStyle = "black";
    gameArea.context.textAlign = "left";
    gameArea.context.textBaseline = "middle";
    gameArea.context.fillText("Poeng: " + gameStatus.points, 50, 50);
    gameArea.context.textAlign = "rigth";

    gameArea.context.fillText("liv: " + gameStatus.lives, gameArea.canvas.width-50,50);

    //display all falling elements
    for (var i = fallingObjects.length - 1; i >= 0; i--) {
        fallingObjects[i].moveElement();

        let remove = fallingObjects[i].onGround() || fallingObjects[i].inBasket()

        //remove element 
        if (remove) {

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

    let value = Data[Math.floor(Math.random() * Data.length)];

    //fix
    let fallingBox = new FallingObject(x, 0, value);

    fallingObjects.push(fallingBox);
}

export default setup;