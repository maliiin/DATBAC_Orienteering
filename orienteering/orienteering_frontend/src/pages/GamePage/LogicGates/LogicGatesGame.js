import setup from "./LogicGatesScript";
import { useEffect, useState } from "react";
import { TextField, Button, Grid } from '@mui/material';
import "./LogicGatesStyle.css";
import LogicGatesData from "./LogicGatesData.js"



export default function LogicGatesGame() {

    const [tableBody, setTableBody] = useState("");
    const [gateDivs, setGateDivs] = useState("");

    var gates = ["AndGate", "NandGate", "Inverter", "OrGate"];

    useEffect(() => {
        
        createFunctionTable("nandnand");
        createGates();
        setup();

    }, [gateDivs.length]);

    function createGates() {
        setGateDivs(
            gates.map((gate, index) =>
                <img key={gate + "-" + index} className="logicgate" src={require(`./${gate}.png`)}></img>
            ));

    }

    function createFunctionTable(gates) {
        const data = (LogicGatesData.find(obj => obj.gates == gates)).values;;
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

    return (<>

        <div id="gamecontainer">
            <img id="task_background" src={require("./task1_background.png")}
                style={{
                    backgroundColor: "white",

                }}
            ></img>

            <div id="dropzoneUpper" className={'dropzone' + ' ' + 'dropzoneUpper'}></div>

            <div id="dropzoneLower" className={'dropzone' + ' ' + 'dropzoneLower'}></div>

            <img id="andgate" className="logicgate" src={require("./AndGate.png")}></img>
            {gateDivs}

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

        </div>

    </>);
}