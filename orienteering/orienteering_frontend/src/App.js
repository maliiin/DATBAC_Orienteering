import ReactDOM from "react-dom/client";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./pages/Layout/Layout";
import Home from "./pages/Home/Home";
import Login from "./pages/Login/Login";

import CheckpointDetails from "./pages/CheckpointDetails/CheckpointDetails";
import Registration from "./pages/Registration/Registration";
import NoPage from "./pages/NoPage";
import Unauthorized from "./pages/Unauthorized/Unauthorized";

import TrackOverview from "./pages/TrackOverview/TrackOverview";
import TrackDetails from "./pages/TrackDetails/TrackDetails";
import QuizPage from "./pages/QuizPage/QuizPage";

import QRCodePage from "./pages/QrCodePage/QRCodePage"
import CheckpointRedirection from "./pages/CheckpointRedirection"

export default function App() {
    const authenticated = false;
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

    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Layout />}>
                    <Route path="checkpointdetails/:checkpointId" element={<CheckpointDetails />} />
                    <Route path="login" element={<Login />} />
                    <Route path="*" element={<NoPage />} />
                    <Route path="registration" element={<Registration />} />
                    <Route path="trackoverview" element={<TrackOverview />}  />
                    <Route path="track/:trackId" element={<TrackDetails />} />
                    <Route path="qrcodepage" element={<QRCodePage />} />
                    <Route path="unauthorized" element={<Unauthorized />} />
                    <Route path="checkpoint/:checkpointId" element={<CheckpointRedirection />} />
                </Route>

                <Route path="checkpoint/quiz/:quizId" element={<QuizPage />} />
                <Route index element={<Home />} />


            </Routes>
            </BrowserRouter>
    )
}
