function gateSizeUpdate() {
    var gateList = document.querySelectorAll(".logicgate");
    for (let i = 0; i < gateList.length; i++) {
        if (gateList[i].style.width == Math.floor((window.screen.width / 100) * 15) + 'px') {
            return
        }
        gateList[i].style.width = Math.floor((window.screen.width / 100) * 15) + 'px';
    }
}
export default gateSizeUpdate;
