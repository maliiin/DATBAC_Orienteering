var interact = require("interactjs");
//const correctMix = ['sol2', 'sol3'];
var correctMix;
var submitted = false;


function setup(prop) {
    chemistry();
    document.getElementById("checkanswer").addEventListener("click", checkAnswer);
    storeCorrectMix(prop);

}
export default setup;

function checkAnswer() {
    const droppedSolutions = document.getElementsByClassName("dropped");
    var boardPassed = true;
    for (let i = 0; i < droppedSolutions.length; i++) {
        if ((correctMix.includes(droppedSolutions[i].textContent.trim()) == false)) {
            console.log("feil svar");
            boardPassed = false;
        }
    }
    if (correctMix.length != droppedSolutions.length) {
        boardPassed = false;
    }
    if (boardPassed) {
        document.getElementById("nextboardbtn").style.display = "block";
    }
    submitted = true;
}


function storeCorrectMix(prop) {
    correctMix = prop;
}



/* The dragging code for '.draggable' from the demo above
 * applies to this demo as well so it doesn't have to be repeated. */

// enable draggables to be dropped into this
function chemistry() {

    // Kilder: https://interactjs.io/ (09.03.2022)

    function dragMoveListener(event) {
        var target = event.target
        // keep the dragged position in the data-x/data-y attributes
        var x = (parseFloat(target.getAttribute('data-x')) || 0) + event.dx
        var y = (parseFloat(target.getAttribute('data-y')) || 0) + event.dy

        // translate the element
        target.style.transform = 'translate(' + x + 'px, ' + y + 'px)'

        // update the posiion attributes
        target.setAttribute('data-x', x)
        target.setAttribute('data-y', y)
    }

    // Kilder: https://interactjs.io/ (09.03.2022) 

    interact('.dropzone').dropzone({
        // only accept elements matching this CSS selector
        accept: '.drag-drop',
        // Require a 75% element overlap for a drop to be possible
        overlap: 0.75,

        // listen for drop related events:

        ondropactivate: function (event) {
            // add active dropzone feedback
            event.target.classList.add('drop-active')
        },
        ondragenter: function (event) {
            var draggableElement = event.relatedTarget
            var dropzoneElement = event.target

            // feedback the possibility of a drop
            dropzoneElement.classList.add('drop-target')
            draggableElement.classList.add('can-drop')
        },
        ondragleave: function (event) {
            // remove the drop feedback style
            event.target.classList.remove('drop-target')
            event.relatedTarget.classList.remove('can-drop')
            event.relatedTarget.classList.remove('dropped');
        },
        ondrop: function (event) {
            event.relatedTarget.classList.add('dropped');
        },
        ondropdeactivate: function (event) {
            // remove active dropzone feedback
            event.target.classList.remove('drop-active')
            event.target.classList.remove('drop-target')
        }
    })

    // Kilder: https://interactjs.io/ (09.03.2022)

    interact('.drag-drop')
        .draggable({
            inertia: true,
            modifiers: [
                interact.modifiers.restrictRect({
                    restriction: 'parent',
                    endOnly: true
                })
            ],
            autoScroll: false,
            // dragMoveListener from the dragging demo above
            listeners: { move: dragMoveListener }
        })
}
