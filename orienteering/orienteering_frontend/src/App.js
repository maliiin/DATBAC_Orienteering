import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./pages/Layout/Layout";
import Login from "./pages/Login/Login";
import CheckpointDetails from "./pages/CheckpointDetails/CheckpointDetails";
import Registration from "./pages/Registration/Registration";
import NoPage from "./pages/NoPage/NoPage";
import ErrorPage from "./pages/ErrorPage/ErrorPage";
import TrackOverview from "./pages/TrackOverview/TrackOverview";
import TrackDetails from "./pages/TrackDetails/TrackDetails";
import QuizPage from "./pages/QuizPage/QuizPage";
import QRCodePage from "./pages/QrCodePage/QRCodePage";
import GamePage from "./pages/GamePage/GamePage";
import CheckpointNavigation from "./pages/CheckpointNavigation/CheckpointNavigation";
import NavigationEditPage from "./pages/NavigationEditPage/NavigationEditPage";



export default function App() {

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
                </Route>

                <Route path="checkpoint/quiz/:checkpointId" element={<QuizPage />} />
                <Route path="checkpoint/game/:checkpointId" element={<GamePage />} />
                <Route path="checkpointnavigation/:checkpointId" element={<CheckpointNavigation />} />

            </Routes>
            </BrowserRouter>
    )
}
