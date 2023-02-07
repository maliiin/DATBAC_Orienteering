import { Outlet, Link } from "react-router-dom";

function Layout() {
    return (
        <>
            <nav>
                <ul>
                    <li>
                        <Link to="/">Home</Link>
                    </li>
                    <li>
                        <Link to="/checkpoint/2">Checkpoint/2</Link>
                    </li>
                    <li>
                        <Link to="/login">Log in</Link>
                    </li>
                    <li>
                        <Link to="/registration">Registrer deg</Link>
                    </li>
                    <li>
                        <Link to="/trackoverview">track overview1</Link>
                    </li>
                   
                </ul>
            </nav>

            <Outlet />
        </>
    )
};

export default Layout;
