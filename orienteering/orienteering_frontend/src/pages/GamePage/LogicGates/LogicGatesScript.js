var interact = require("interactjs");



function setup() {
    initDropzones();
    document.getElementById("gamecontainer").style.width = window.screen.width + 'px';
    document.getElementById("task_background").style.width = window.screen.width + 'px';
    document.getElementById("task_background").style.position = 'relative';
    document.getElementById("andgate").style.width = Math.floor((window.screen.width / 100) * 15) + 'px';

    document.getElementById("dropzoneUpper").style.width = Math.floor((window.screen.width / 100) * 15) + 'px';
    document.getElementById("dropzoneUpper").style.position = 'absolute';
    document.getElementById("dropzoneUpper").style.left = Math.floor((window.screen.width / 100) * 20) + 'px';
    document.getElementById("dropzoneUpper").style.top = Math.floor(((document.getElementById("task_background").clientHeight) / 100) * 10) + 'px';
    //document.getElementById("dropzoneUpper").style.top = 5 + 'px';

    document.getElementById("dropzoneLower").style.width = Math.floor((window.screen.width / 100) * 15) + 'px';
    document.getElementById("dropzoneLower").style.position = 'absolute';
    document.getElementById("dropzoneLower").style.left = Math.floor((window.screen.width / 100) * 60) + 'px';
    document.getElementById("dropzoneLower").style.top = Math.floor(((document.getElementById("task_background").clientHeight) / 100) * 10) + 'px';
    //document.getElementById("dropzoneLower").style.top = 20 + 'px';



}
export default setup;



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
            event.relatedTarget.classList.remove('dropped1');
        },
        ondrop: function (event) {
            event.relatedTarget.classList.add('dropped1');
        }
    })

    interact('.dropzoneLower').dropzone({
        ondragleave: function (event) {
            // remove the drop feedback style
            event.target.classList.remove('drop-target')
            event.relatedTarget.classList.remove('can-drop')
            event.relatedTarget.classList.remove('dropped2');
        },
        ondrop: function (event) {
            event.relatedTarget.classList.add('dropped2');
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
