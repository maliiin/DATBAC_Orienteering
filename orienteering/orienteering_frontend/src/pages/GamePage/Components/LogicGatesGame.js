import setup from "../Scripts/LogicGatesScript";
import { useEffect, useState } from "react";
import { Button } from '@mui/material';
import "../Assets/LogicGates/LogicGatesStyle.css";
import LogicGatesData from "../Data/LogicGatesData.js"
import LogicGateSizeUpdate from "../Components/LogicGateSizeUpdate";

export default function LogicGatesGame() {

    const [tableA, setTableA] = useState("");
    const [tableB, setTableB] = useState("");
    const [tableC, setTableC] = useState("");
    const [tableOut, setTableOut] = useState("");

    const [gateDivs, setGateDivs] = useState("");
    const [gameDescription, setGameDescription] = useState("");

    const gates = ["AndGate", "NandGate", "Inverter", "OrGate", "AndGate", "NandGate", "Inverter", "OrGate"];
    var nextBoardIndex = 0;

    useEffect(() => {

        createFunctionTable();
        createGates();
        setup();

    }, []);

    function createGates() {
        setGateDivs(
            gates.map((gate, index) =>
                <img
                    key={gate + "-" + index + nextBoardIndex}
                    className={'logicgate' + ' ' + gate}
                    src={require(`../Assets/LogicGates/${gate}.png`)}
                >
                </img>
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
        //var tbody = (data.map((row, index) => {
        //    return (
        //        <tr key={row + "-" + index}>
        //            {
        //                row.map((column, index) => {
        //                    return (<td key={column + "-" + index}>{column}</td>)
        //                })

        //            }
        //        </tr>
        //    )
        //}
        //));

        var tbody = (data.map((column, index) => {
            return (
                <>
                    {
                        column.map((row, index) => {
                            return (<td key={row + "-" + index}>{row}</td>)
                        })

                    }
                </>
            )
        }
        ));
        setTableA(tbody[0])
        setTableB(tbody[1])
        setTableC(tbody[2])
        setTableOut(tbody[3])
    }

    function clearGateDivs() {
        setGateDivs("");
    }

    function nextBoard() {
        document.getElementById("nextboardbtn").style.display = "none";
        clearGateDivs();
        createGates();
        createFunctionTable();
        const correctGates = LogicGatesData[nextBoardIndex].gates;
        if (LogicGatesData.length == nextBoardIndex + 1) {
            setup(correctGates, true);
        }
        else {
            setup(correctGates);
        }
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
    //fix- to useeffect som tar inn []-disse kan sl�s sammen?
    //og samle use effect p� samme sted i koden

    useEffect(() => {
        showGameDescription();
        document.getElementById("nextboardbtn").addEventListener("click", nextBoard);
    }, []);

    useEffect(() => {
        if (gateDivs != undefined || gateDivs != "") {
            LogicGateSizeUpdate();
        }
    }, [gateDivs]);

    return (<>

        <div
            id="gamecontainer"
            style={{
                height: window.innerHeight,
            }}
        >


            <img id="task_background" src={require("../Assets/LogicGates/task1_background.png")}></img>

            <div id="dropzoneUpper" className={'dropzone' + ' ' + 'dropzoneUpper'}></div>

            <div id="dropzoneLower" className={'dropzone' + ' ' + 'dropzoneLower'}></div>

            {gateDivs}

            <table>
                <tbody>
                    <tr>
                        <th>A</th>
                        {tableA}
                    </tr>
                    <tr>
                        <th>B</th>
                        {tableB}
                    </tr>
                    <tr>
                        <th>C</th>
                        {tableC}
                    </tr>
                    <tr>
                        <th>Out</th>
                        {tableOut}
                    </tr>
                </tbody>
            </table>

            <button id="checkanswer" className="gamebtn">Check</button>
            <button id="nextboardbtn" hidden className="gamebtn">Next board</button>
            <br></br>
            <p id="scorediv"></p>
            <p id="statusdiv"></p>

        </div>

        <div id="descriptionContainer" className="readabletext"> {gameDescription}</div>

    </>);
}

            //<span id="scorediv"></span>
            //<span id="statusdiv"></span>