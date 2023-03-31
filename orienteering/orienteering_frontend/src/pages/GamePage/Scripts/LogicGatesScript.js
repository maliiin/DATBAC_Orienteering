var interact = require("interactjs");
var GameInitialized = false;
var score = 0;
var hp = 3;
var goToNextBoard = false;
var lastBoard = false;
var correctGates = [];


function setup(inpCorrectGates, inpLastBoard = false) {
    if (!GameInitialized) {
        initDropzones();
        document.getElementById("gamecontainer").style.width = window.screen.width + 'px';
        document.getElementById("task_background").style.width = window.screen.width + 'px';
        document.getElementById("task_background").style.position = 'relative';

        document.getElementById("dropzoneUpper").style.width = Math.floor((window.screen.width / 100) * 15) + 'px';
        document.getElementById("dropzoneUpper").style.height = Math.floor((window.screen.width / 100) * 10) + 'px';
        document.getElementById("dropzoneUpper").style.position = 'absolute';
        document.getElementById("dropzoneUpper").style.left = Math.floor((window.screen.width / 100) * 20) + 'px';
        document.getElementById("dropzoneUpper").style.top = Math.floor(((document.getElementById("task_background").clientHeight) / 100) * 10) + 'px';
        //document.getElementById("dropzoneUpper").style.top = 5 + 'px';

        document.getElementById("dropzoneLower").style.width = Math.floor((window.screen.width / 100) * 15) + 'px';
        document.getElementById("dropzoneLower").style.height = Math.floor((window.screen.width / 100) * 10) + 'px';
        document.getElementById("dropzoneLower").style.position = 'absolute';
        document.getElementById("dropzoneLower").style.left = Math.floor((window.screen.width / 100) * 60) + 'px';
        document.getElementById("dropzoneLower").style.top = Math.floor(((document.getElementById("task_background").clientHeight) / 100) * 20) + 'px';
    //document.getElementById("dropzoneLower").style.top = 20 + 'px';
        document.getElementById("checkanswer").addEventListener("click", checkAnswer);
        GameInitialized = true;
    }
    


    correctGates = inpCorrectGates;
    lastBoard = inpLastBoard;
    goToNextBoard = false;
    
    

}
export default setup;

function checkAnswer() {
    if (goToNextBoard) {
        // Prevents user from checking board when no more lifes (hp) left or correctMix has been submitted
        return
    }
    const droppedGateUpper = document.getElementsByClassName("droppedUpper")[0];
    const droppedGateLower = document.getElementsByClassName("droppedLower")[0];
    var boardPassed = true;

    if (droppedGateUpper == undefined || droppedGateLower == undefined) {
        document.getElementById("statusdiv").textContent = `Some droparea missing a logic gate`;
        return 
    }
 
    if (droppedGateUpper.classList.contains(correctGates[0]) == false) {
        boardPassed = false;
    }
    if (droppedGateLower.classList.contains(correctGates[1]) == false) {
        boardPassed = false;
    }
    
    if (boardPassed) {
        document.getElementById("statusdiv").textContent = `Correct gates`;
    }
    else {
        hp -= 1;
        document.getElementById("statusdiv").textContent = `Wrong gates`;
        document.getElementById("scorediv").textContent = `HP left: ${hp}`;
    }
    if (boardPassed || hp < 1)
        {
        goToNextBoard = true;
        score += hp;
        hp = 3;
        document.getElementById("scorediv").textContent = `Score: ${score}`;

        if (lastBoard)
            {
            document.getElementById("gamecontainer").style.display = "none";
            const newLine = "\r\n";
            if (boardPassed)
                {
                document.getElementById("descriptionContainer").textContent = "Correct gates" + newLine + "Game finished" + newLine + `Total score: ${score}`;
                }
            else
            {
                document.getElementById("descriptionContainer").style.display = "block";
                document.getElementById("descriptionContainer").style.whiteSpace = "pre";
                document.getElementById("descriptionContainer").textContent = "Wrong gates" + newLine + "Game finished" + newLine + `Total score: ${score}`;
                

                }
            document.getElementById("navigationButton").style.display = "block";
            
            }
        else
            {
            document.getElementById("nextboardbtn").style.display = "inline-block";
            }
        }
}


/* The dragging code for '.draggable' from the demo above
 * applies to this demo as well so it doesn't have to be repeated. */

// enable draggables to be dropped into this
function initDropzones() {

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
        accept: '.logicgate',
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

    interact('.dropzoneUpper').dropzone({
        ondragleave: function (event) {
            // remove the drop feedback style
            event.target.classList.remove('drop-target')
            event.relatedTarget.classList.remove('can-drop')
            event.relatedTarget.classList.remove('droppedUpper');
        },
        ondrop: function (event) {
            event.relatedTarget.classList.add('droppedUpper');
        }
    })

    interact('.dropzoneLower').dropzone({
        ondragleave: function (event) {
            // remove the drop feedback style
            event.target.classList.remove('drop-target')
            event.relatedTarget.classList.remove('can-drop')
            event.relatedTarget.classList.remove('droppedLower');
        },
        ondrop: function (event) {
            event.relatedTarget.classList.add('droppedLower');
        }
    })


    // Kilder: https://interactjs.io/ (09.03.2022)

    interact('.logicgate')
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
