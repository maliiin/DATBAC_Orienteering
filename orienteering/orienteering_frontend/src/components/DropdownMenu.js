import Dropdown from 'react-bootstrap/Dropdown';

function DropdownMenu() {
    return (
        <Dropdown>
            <Dropdown.Toggle variant="success" id="dropdown-basic">
                Dropdown Button
            </Dropdown.Toggle>

            <Dropdown.Menu>
                <Dropdown.Item href="#/action-1">Game1</Dropdown.Item>
                <Dropdown.Item href="#/action-2">Game2 action</Dropdown.Item>
                <Dropdown.Item href="#/action-3">Game3 else</Dropdown.Item>
            </Dropdown.Menu>
        </Dropdown>
    );
}

export default DropdownMenu;