import { useEffect, useState } from "react";
import setup from '../Scripts/ChemistryScript';
import { Button } from '@mui/material';
import "../Assets/Chemistry/ChemistryStyle.css";
import ChemistryData from "../Data/ChemistryData";

var nextBoardIndex = 0;
var solutionList;
var correctMix;

function ChemistryGame() {

    const [solutionDivs, setSolutionDivs] = useState("");
    const [taskText, setTaskText] = useState("");
    const [gameDescription, setGameDescription] = useState("");

    function createSolutions() {
        const solutions = (solutionList.map((solution, index) =>
            <div
                key={solution + "-" + index + nextBoardIndex}
                className="drag-drop"
            >
                <div className="chemistryGlassImage"></div>
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

    function nextBoard() {
        document.getElementById("nextboardbtn").style.display = "none";
        document.getElementById("statusdiv").textContent = "";
        solutionList = ChemistryData[nextBoardIndex].solutionList;
        correctMix = ChemistryData[nextBoardIndex].correctMix;
        setTaskText(ChemistryData[nextBoardIndex].taskText);
        clearSolutions();
        createSolutions();
        nextBoardIndex += 1;
    }

    function showGameDescription() {
        setGameDescription(<>
            <div>
                <h3>Chemistry game</h3>
                <p >
                    Drag the solutions to the drop area which are needed to create the solution provided in the task.
                    <br></br>
                    You got 3 Hp for each task. For each mixing with wrong solutions, you loose 1 hp.
                    <br></br>
                    When mixing solutions and the correct solutions are in the drop area, the remaining hp are added to your score.
                </p>
                <br></br>
                <br></br>

                <Button
                    style={{ fontSize: '4vw' }}

                    onClick={hideGameDescription}
                >
                    Start game
                </Button></div>

        </>
        )
        document.getElementById("gamecontainer").style.display = "none";
        document.getElementById("descriptionContainer").style.display = "block";
    }

    function hideGameDescription() {
        document.getElementById("gamecontainer").style.display = "block";
        document.getElementById("descriptionContainer").style.display = "none";
        nextBoard();
    }

    useEffect(() => {
        document.getElementById("nextboardbtn").addEventListener("click", nextBoard);
        showGameDescription();

    }, []);

    return (
        <>
            <div id="gamecontainer">
                <h4 className="solutiontext">Task: {taskText}</h4>

                {solutionDivs}

                <div id="outer-dropzone" className="dropzone">
                </div>

                <button id="checkanswer" className="gamebtn">Mix</button>
                <button id="nextboardbtn" hidden className="gamebtn">Next board</button>

                <div id="statusdiv"></div>
                <div id="scorediv"></div>
            </div>

            <div id="descriptionContainer" className="readabletext">{gameDescription}</div>
        </>);
}

export default ChemistryGame;
