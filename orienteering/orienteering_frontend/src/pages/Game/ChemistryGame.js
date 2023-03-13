import { useEffect, useState } from "react";
import setup from './ChemistryScript';
import "./style.css";

var nextBoardIndex = 0;
var solutionList;
var correctMix;

function ChemistryGame() {

    var boardList = [
    // Fix: legg inn virkelige solutions
        {
            solutionList: ['sodshjgdfdsgfhadfagl1', 'sol2', 'sol4', 'sol3', 'sol5', 'asdfwa', 'asdawsd', 'gafsdad'],
            correctMix: ['sol2', 'sol3']
        },
        {
            solutionList: ['sohadfagl1', 'sol22', 'sol32', 'sol4', 'sol55', 'awsd', 'pqwrj', 'asdfwa'],
            correctMix: ['sohadfagl1', 'sol4']
        }
    ]
    
    const [solutionDivs, setSolutionDivs] = useState("");

    function createSolutions() {
        var solutions = (solutionList.map((solution, index) =>
            <div key={solution + "-" + index} className="drag-drop">
                <img src={require("./chemistryGlass.png")} ></img>
                <div className="solutiontext">{solution}</div>
            </div>
        ))
        setSolutionDivs(solutions);
        if (boardList.length == nextBoardIndex + 1) {
            setup(correctMix, true);
        }
        else {
            setup(correctMix);

        }
    }

    function clearSolutions() {
        setSolutionDivs("");
}


    //function retryGame() {
    //    var solutions = solutionDivs;
    //    for (let i = 0; i < solutions.length; i++) {
    //        console.log(solutions[i])
    //        solutions[i].style.transform = "translate(0px, 0px)";
    //    }
    //    if (solutions != "") {
    //        createSolutions();
    //        setSolutionDivs(solutions);
    //    }
    //    document.getElementById("retrygamebtn").style.display = "none";
    //}

    function nextBoard() {
        document.getElementById("nextboardbtn").style.display = "none";
        solutionList = boardList[nextBoardIndex].solutionList;
        correctMix = boardList[nextBoardIndex].correctMix;
        clearSolutions();
        createSolutions();
        nextBoardIndex += 1;
    }

    function navigateToCheckpoint() {
        console.log("gamefinished");
        //fix: implement navigate to next checkpoint
    }


    useEffect(() => {
        //document.getElementById("retrygamebtn").addEventListener("click", retryGame);
        document.getElementById("nextboardbtn").addEventListener("click", nextBoard);
        document.getElementById("navigationbtn").addEventListener("click", navigateToCheckpoint);
        nextBoard();
    }, []);


return (
        <>

        {solutionDivs}

        <div id="outer-dropzone" className="dropzone">
        </div>

        
        <button id="checkanswer" className="gamebtn">Check</button>
        <button id="nextboardbtn" hidden className="gamebtn">Next board</button>
        <button id="navigationbtn" hidden className="gamebtn"> Navigate to next checkpointnt</button>

        <div id="statusdiv"></div>
        <div id="scorediv"></div>

               
    </>);

}

export default ChemistryGame;
