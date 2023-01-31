import { TextField } from '@mui/material';


//dette er registrer, ikke login!!
function Login() {
    function logInUser() {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                username: 'test',
                password: 'passord123',
                email: 'testmail@gmail.com'
            })
        };
        //fetch('https://localhost:3000/api/user', requestOptions)
        //    .then(response => response.json())
        //    .then(data => console.log(data));

    }

        return (
            <>
                <TextField id="standard-basic" label="Standard" variant="standard" />
                <TextField id="standard-basic" label="Standard" variant="standard" />
                <button onClick={logInUser}>do thing</button>
            </>
        );
    }


    export default Login;
