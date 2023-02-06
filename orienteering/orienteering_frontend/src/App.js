import ReactDOM from "react-dom/client";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./pages/Layout";
import Home from "./pages/Home";
import Login from "./pages/Login";

import Checkpoint from "./pages/Checkpoint";
import Registration from "./pages/Registration";


export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Layout />}>
                    <Route index element={<Home />} />
                    <Route path="checkpoint/:checkpointId" element={<Checkpoint />} />
                    <Route path="login" element={<Login />} />
                    <Route path="registration" element={<Registration />} />


                    
                </Route>
            </Routes>
        </BrowserRouter>
    );
}
