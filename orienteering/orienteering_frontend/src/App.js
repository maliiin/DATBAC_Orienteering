import ReactDOM from "react-dom/client";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./pages/Layout";
import Home from "./pages/Home";
import Login from "./pages/Login";

import CheckpointDetails from "./pages/CheckpointDetails/CheckpointDetails";
import Registration from "./pages/Registration";
import NoPage from "./pages/NoPage";
import TrackOverview from "./pages/TrackOverview/TrackOverview";
import TrackDetails from "./pages/TrackDetails/TrackDetails";

import QRCodePage from "./pages/QRCodePage"

export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Layout />}>
                    <Route index element={<Home />} />
                    <Route path="checkpoint/:checkpointId" element={<CheckpointDetails />} />
                    <Route path="login" element={<Login />} />
                    <Route path="*" element={<NoPage />} />
                    <Route path="registration" element={<Registration />} />
                    <Route path="trackoverview" element={<TrackOverview />} />
                    <Route path="track/:trackId" element={<TrackDetails />} />

                    <Route path="qrcodepage" element={<QRCodePage />} />
                </Route>
            </Routes>
            </BrowserRouter>
    )
}
