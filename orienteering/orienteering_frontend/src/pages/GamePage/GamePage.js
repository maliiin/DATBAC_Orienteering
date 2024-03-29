import FallingBoxesGame from "./Components/FallingBoxesGame";
import ChemistryGame from "./Components/ChemistryGame";
import { Button } from '@mui/material';
import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from 'react-router-dom';
import LogicGatesGame from "./Components/LogicGatesGame";
import CheckpointDescription from '../../Components/CheckpointDescription';

export default function GamePage() {
    const navigate = useNavigate();
    const params = useParams();
    const [chosenGame, setChosenGame] = useState("");
    const [checkpointDescriptionDisplayed, setCheckpointDescriptionDisplayed] = useState(false);

    useEffect(() => {
        checkSession();
        setGame();
    }, []);

    async function checkSession() {
        const url = "/api/session/setStartCheckpoint";
        await fetch(url, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                CheckpointId: params.checkpointId
            })
        });
    }

    async function setGame() {
        const url = "/api/checkpoint/getGameidForCheckpoint?checkpointId=" + params.checkpointId;
        const response = await fetch(url);
        if (!response.ok) {
            navigate("/errorpage");
        }
        const gameId = await response.json();
        if (gameId == 1) {
            setChosenGame(<FallingBoxesGame></FallingBoxesGame>);
        }
        else if (gameId == 2) {
            setChosenGame(<ChemistryGame></ChemistryGame>);
        }
        else {
            setChosenGame(<LogicGatesGame></LogicGatesGame>);
        }
    }

    function hideCheckpointDescription() {
        setCheckpointDescriptionDisplayed(true);
    }

    function navigateToCheckpoint() {
        navigate("/checkpointnavigation/" + params.checkpointId);
    }

    if (checkpointDescriptionDisplayed) {
        return (
            <>
                {chosenGame}

                <Button
                    id="navigationButton"
                    style={{
                        fontSize: 5 + "vw",
                        display: "none",
                        position: "absolute",
                        backgroundColor: "white",
                    }}
                    onClick={navigateToCheckpoint}
                >
                    Navigate to next checkpoint
                </Button>
            </>
        );
    }

    if (!checkpointDescriptionDisplayed) {
        return (
            <>
                <CheckpointDescription
                    checkpointId={params.checkpointId}
                    hideDescription={hideCheckpointDescription}>
                </CheckpointDescription>
            </>);
    }
}