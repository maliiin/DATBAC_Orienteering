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

function testfunk(event) {
    console.log(event);
    console.log(event.originalEvent.changedTouches[0]);
    console.log("trykket!!!!!!!!!!!!");
    let x_pos = 0;
    let y_pos = 0;
    var d = document.getElementById('root1');
    d.style.position = "absolute";
    d.style.left = x_pos + 'px';
    d.style.top = y_pos + 'px';
    console.log("inni test!!!!!!!!!!!!");
}

function test() {
    let hele = document.getElementById("root").addEventListener("touchmove", testfunk);


}
export default test;
