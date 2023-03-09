function allowDrop(ev) {
    ev.preventDefault();
}

function drag(ev) {
    ev.dataTransfer.setData("text", ev.target.id);
}

function drop(ev) {
    ev.preventDefault();
    var data = ev.dataTransfer.getData("text");
    ev.target.appendChild(document.getElementById(data));
}

function testfunk() {
    console.log("trykket!!!!!!!!!!!!");
}

function test() {
    let hele = document.getElementById("root").addEventListener("touchstart", testfunk);

    console.log("inni test!!!!!!!!!!!!");
}
export default test;
