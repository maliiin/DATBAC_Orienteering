const position = { x: 0, y: 0 }
var interact = require('interactjs')

function moveBasket() {

    //kilde https://interactjs.io/docs/draggable/ 09/03.23
    interact('.draggable').draggable({
        listeners: {
            start(event) {
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

export default moveBasket;