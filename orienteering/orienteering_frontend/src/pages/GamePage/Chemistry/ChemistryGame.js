import { useEffect, useState } from "react";
import setup from './ChemistryScript';
import { Button} from '@mui/material';
import "./ChemistryStyle.css";
import ChemistryData from "./ChemistryData";

var nextBoardIndex = 0;
var solutionList;
var correctMix;

function ChemistryGame() {

    //var boardList = [
    //// Fix: legg inn virkelige solutions
    //    {
    //        solutionList: ['sodshjgdfdsgfhadfagl1', 'sol2', 'sol4', 'sol3', 'sol5', 'asdfwa', 'asdawsd', 'gafsdad'],
    //        correctMix: ['sol2', 'sol3'],
    //        taskText: "create a aslfns solution"
    //    },
    //    {
    //        solutionList: ['sohadfagl1', 'sol22', 'sol32', 'sol4', 'sol55', 'awsd', 'pqwrj', 'asdfwa'],
    //        correctMix: ['sohadfagl1', 'sol4'],
    //        taskText: "create a ljkgaf solution"
    //    }
    //]
    
    const [solutionDivs, setSolutionDivs] = useState("");
    const [taskText, setTaskText] = useState("");
    const [gameDescription, setGameDescription] = useState("");

    function createSolutions() {
        var solutions = (solutionList.map((solution, index) =>
            <div key={solution + "-" + index} className="drag-drop">
                <img src={require("./chemistryGlass.png")} ></img>
                <div className="solutiontext">{solution}</div>
            </div>
        ))
        setSolutionDivs(solutions);
        if (ChemistryData.length == nextBoardIndex + 1) {
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
        solutionList = ChemistryData[nextBoardIndex].solutionList;
        correctMix = ChemistryData[nextBoardIndex].correctMix;
        setTaskText(ChemistryData[nextBoardIndex].taskText);
        clearSolutions();
        createSolutions();
        nextBoardIndex += 1;
    }

    function navigateToCheckpoint() {
        console.log("gamefinished");
        //fix: implement navigate to next checkpoint
    }

    function showGameDescription() {
        setGameDescription(<>
            <div style={{ margin: "5%" }}>
                <h3>Chemistry game</h3>
                <p >
                    Drag the solutions to the drop area which are needed to create the solution provided in the task.
                    <br></br>
                    You got 3 Hp for each task. For each check with wrong solutions, you loose 1 hp. 
                    <br></br>
                    When checking answer and the correct solutions are in the drop area, the remaining hp are added to your score. 
                </p>
                <p>Click start game when you are ready</p>
                <br></br>
                <br></br>

                <Button
                    style={{                        
                        backgroundColor: "white",
                    }}

                    onClick={hideGameDescription}
                >
                    Start game
                </Button></div>
            
        </>
        )
        document.getElementById("gamecontainer").style.display = "none";
    }

    function hideGameDescription() {
        setGameDescription("");
        document.getElementById("gamecontainer").style.display = "block";
        nextBoard();
    }


    useEffect(() => {
        //document.getElementById("retrygamebtn").addEventListener("click", retryGame);
        document.getElementById("nextboardbtn").addEventListener("click", nextBoard);
        showGameDescription();
        
    }, []);




return (
        <>
        <div id="gamecontainer">
            {solutionDivs}

            <div id="outer-dropzone" className="dropzone">
            </div>

            <div className="solutiontext">Task: {taskText}</div>

            <button id="checkanswer" className="gamebtn">Check</button>
            <button id="nextboardbtn" hidden className="gamebtn">Next board</button>

            <div id="statusdiv"></div>
            <div id="scorediv"></div>
        </div>

        

        {gameDescription}
    </>);

}

export default ChemistryGame;
