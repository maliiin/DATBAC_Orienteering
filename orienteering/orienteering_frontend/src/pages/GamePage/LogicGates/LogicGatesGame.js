import setup from "./LogicGatesScript";
import { useEffect, useState } from "react";
import { TextField, Button, Grid } from '@mui/material';
import "./LogicGatesStyle.css";
import LogicGatesData from "./LogicGatesData.js"



export default function LogicGatesGame() {

    const [tableBody, setTableBody] = useState("");
    const [gateDivs, setGateDivs] = useState("");
    const [gameDescription, setGameDescription] = useState("");


    var gates = ["AndGate", "NandGate", "Inverter", "OrGate"];
    var nextBoardIndex = 0;
    var correctGates;

    useEffect(() => {
        
        createFunctionTable();
        createGates();
        setup();

    }, []);

    function createGates() {
        setGateDivs(
            gates.map((gate, index) =>
                <img key={gate + "-" + index} className={'logicgate' + ' ' + gate} src={require(`./${gate}.png`)}></img>
            ));

    }

    function createFunctionTable() {
        //const data = (LogicGatesData.find(obj => obj.gates == gates)).values;;
        const data = LogicGatesData[nextBoardIndex].values;

        //data.forEach((row) => {
        //    const tr = document.createElement("tr");
        //    row.forEach((column) => {
        //        const td = document.createElement("td");
        //        td.textContent = column;
        //        tr.appendChild(td);
        //    });

        //    tbody.appendChild(tr);
        //});
        var tbody = (data.map((row, index) => {
            return (
                <tr key={row + "-" + index}>
                    {
                        row.map((column, index) => {
                            return (<td key={column + "-" + index}>{column}</td>)
                        })

                    }
                </tr>
            )
        }
        ));
        setTableBody(tbody);
    }

    function nextBoard() {
        document.getElementById("nextboardbtn").style.display = "none";
        createGates();
        createFunctionTable();
        setup();
        nextBoardIndex += 1;
    }

    function showGameDescription() {
        setGameDescription(<>
            <div style={{ margin: "5%" }}>
                <h3>Logic Gates game</h3>
                <p >
                    Make the scheme correspond to the functional table by correctly placing the logical gates.
                    <br></br>
                    You got 3 Hp for each task. For each check with wrong solutions, you loose 1 hp.
                    <br></br>
                    When checking answer and the correct solutions are in the drop area, the remaining hp are added to your score.
                </p>
                <br></br>
                <br></br>

                <Button
                    style={{ fontSize: '4vw'}}
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
    }

    useEffect(() => {
        showGameDescription();
        document.getElementById("nextboardbtn").addEventListener("click", nextBoard);
    }, []);

    return (<>

        <div id="gamecontainer">
            <img id="task_background" src={require("./task1_background.png")}
            ></img>

            <div id="dropzoneUpper" className={'dropzone' + ' ' + 'dropzoneUpper'}></div>

            <div id="dropzoneLower" className={'dropzone' + ' ' + 'dropzoneLower'}></div>

            <div id="gateContainer">{gateDivs}</div>

            <table>
                <thead>
                    <tr>
                        <th>A</th>
                        <th>B</th>
                        <th>C</th>
                        <th>Out</th>
                    </tr>
                </thead>
                <tbody>{tableBody}</tbody>

            </table>

            <button id="checkanswer" className="gamebtn">Check</button>
            <button id="nextboardbtn" hidden className="gamebtn">Next board</button>

            <div id="statusdiv"></div>
            <div id="scorediv"></div>
        </div>

        

        <div id="descriptionContainer" className="readabletext"> {gameDescription}</div>

    </>);
}