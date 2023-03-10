import { useEffect, useState } from "react";
import setup from './ChemistryScript';
import "./style.css";

var nextBoardIndex = 0;
var solutionList;
var correctMix;

function ChemistryGame() {

    var boardList = [
        {
            solutionList: ['sodshjgdfdsgfhadfagl1', 'sol2', 'sol3', 'sol4', 'sol5', 'asdfwa', 'asdawsd', 'gafsdad'],
            correctMix: ['sol2', 'sol3']
        },
        {
            solutionList: ['sohadfagl1', 'sol22', 'sol32', 'sol4', 'sol55', 'asdfwa', 'awsd', 'pqwrj'],
            correctMix: ['sohadfagl', 'sol4']
        }
    ]
    
    const [solutionDivs, setSolutionDivs] = useState("");

    function createSolutions() {
        var solutions = (solutionList.map((solution, index) =>
            <div key={solution + "-" + index} className="drag-drop">
                <img src={require("./chemistryGlass.png")} ></img>
                <div>{solution}</div>
            </div>
        ))
        setSolutionDivs(solutions);
        setup(correctMix);
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
        createSolutions();
        nextBoardIndex += 1;
    }


    useEffect(() => {
        //document.getElementById("retrygamebtn").addEventListener("click", retryGame);
        document.getElementById("nextboardbtn").addEventListener("click", nextBoard);
        nextBoard();
    }, []);
    return (<>
        <div>
        </div>
        {solutionDivs}

        <div id="outer-dropzone" className="dropzone">
            
        </div>


        <button id="checkanswer" className="gamebtn">Check</button>
        <button id="nextboardbtn" hidden className="gamebtn">Next board</button>
       
    </>);





}

export default ChemistryGame;
