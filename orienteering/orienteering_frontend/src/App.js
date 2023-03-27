import ReactDOM from "react-dom/client";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./pages/Layout/Layout";
import LayoutSignedIn from "./pages/Layout/LayoutSignedIn";
import Login from "./pages/Login/Login";
import CheckpointDetails from "./pages/CheckpointDetails/CheckpointDetails";
import Registration from "./pages/Registration/Registration";
import NoPage from "./pages/NoPage";
import ErrorPage from "./pages/ErrorPage";
import Unauthorized from "./pages/Unauthorized/Unauthorized";
import TrackOverview from "./pages/TrackOverview/TrackOverview";
import TrackDetails from "./pages/TrackDetails/TrackDetails";
import QuizPage from "./pages/QuizPage/QuizPage";
import QRCodePage from "./pages/QrCodePage/QRCodePage";
import GamePage from "./pages/GamePage/GamePage";

import ChemistryGame from "./pages/GamePage/Chemistry/ChemistryGame";

import CheckpointNavigation from "./pages/CheckpointNavigation/CheckpointNavigation";
import NavigationEditPage from "./pages/NavigationEditPage/NavigationEditPage";

import LogicGatesGame from "./pages/GamePage/LogicGates/LogicGatesGame";
import FallingBoxesGame from "./pages/GamePage/FallingBoxes/FallingBoxesGame";


export default function App() {
    const authenticated = false;

    //fix slett den under? ikke i bruk
    //fra https://stackoverflow.com/questions/40055439/check-if-logged-in-react-router-app-es6/40055744#40055744 13.feb.23
    function requireAuth(nextState, replace, next) {
        if (!authenticated) {
            replace({
                pathname: "/login",
                state: { nextPathname: nextState.location.pathname }
            });
        }
        next();
    }
    //fix slett rene spill pagesx3 stk
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Layout />}>
                    <Route index element={<TrackOverview />} />
                    <Route path="track/:trackId" element={<TrackDetails />} />
                    <Route path="checkpointdetails/:checkpointId" element={<CheckpointDetails />} />
                    <Route path="navigationEdit/:checkpointId" element={<NavigationEditPage />} />
                    <Route path="qrcodepage" element={<QRCodePage />} />

                    <Route path="login" element={<Login />} />
                    <Route path="registration" element={<Registration />} />

                    <Route path="*" element={<NoPage />} />
                    <Route path="errorpage" element={<ErrorPage />} />
                    <Route path="unauthorized" element={<Unauthorized />} />
                </Route>

                    


                <Route path="checkpoint/quiz/:checkpointId" element={<QuizPage />} />
                <Route path="checkpoint/game/:checkpointId" element={<GamePage />} />
                <Route path="checkpointnavigation/:checkpointId" element={<CheckpointNavigation />} />




                <Route path="primtall" element={<FallingBoxesGame/> }/>
                <Route path="kjemi" element={<ChemistryGame/> }/>
                <Route path="elektro" element={<LogicGatesGame/> }/>






            </Routes>
            </BrowserRouter>
    )
}
