import FallingBoxesGame from "./FallingBoxes/FallingBoxesGame";
import ChemistryGame from "./Chemistry/ChemistryGame";
import { TextField, Button, Grid } from '@mui/material';
import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from 'react-router-dom';
import LogicGatesGame from "./LogicGates/LogicGatesGame";


export default function GamePage() {

    // Fix/fiks: Fikse canvasheight og width variablene 
    const canvasHeight = 200;
    const canvasWidth = 200;
    const navigate = useNavigate();
    const params = useParams();
    const [chosenGame, setChosenGame] = useState("");


    useEffect(() => {
        setGame();
    }, []);

    async function setGame() {
        const url = "/api/checkpoint/getCheckpoint?checkpointId=" + params.checkpointId;
        const checkpointDto = await fetch(url).then(res => res.json());
        if (checkpointDto.gameId == 1) {
            setChosenGame(<FallingBoxesGame></FallingBoxesGame>);
        }
        else if (checkpointDto.gameId == 2) {
            setChosenGame(<ChemistryGame></ChemistryGame>);
        }
        else {
            setChosenGame(<LogicGatesGame></LogicGatesGame>);
        }
        
    }

    function navigateToCheckpoint() {
        navigate("/checkpointnavigation/" + params.checkpointId);
    }

    
    return (
        <>
            {chosenGame}

            <Button
                id="navigationButton"
                style={{
                    fontSize: 5 + "vw",
                    display: "none",
                    position: "absolute",
                    //top: canvasHeight * 2 / 3 + "px",
                    //left: canvasWidth / 2 - 100 + "px",
                    backgroundColor: "white",

                }}
                onClick={navigateToCheckpoint}
            >
                Navigate to next checkpoint
            </Button>
            


        </>
    );
}