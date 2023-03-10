import { useEffect, useState } from "react";
import setup from './ChemistryScript';
import "./style.css";

function ChemistryGame() {

    var solutionList = ['sol1', 'sol2', 'sol3', 'sol4', 'sol5'];
    var correctMix = ["sol2", "sol3"];
    const [solutionDivs, setSolutionDivs] = useState("");

    function createSolutions() {
        var solutions = (solutionList.map((solution, index) =>
            <div key={solution + "-" + index} className="drag-drop">
                <img src={require("./chemistryGlass.png")} ></img>
                {solution}
            </div>
        ))
        setSolutionDivs(solutions);
    }


    useEffect(() => {

        setup(correctMix);
        createSolutions();
    }, []);
    return (<>


        <div className="drag-drop">
            <img src={require("./chemistryGlass.png")} ></img>
        </div>

        <div id="outer-dropzone" className="dropzone">
            #outer-dropzone
            <div id="inner-dropzone" className="dropzone">#inner-dropzone</div>
        </div>

        <div className="solutions-container">
            {solutionDivs}
        </div>


        <button id="checkanswer" >Kontroller</button>

    </>);





}

export default ChemistryGame;
