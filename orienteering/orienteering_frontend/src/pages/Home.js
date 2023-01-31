import { useState, useEffect } from "react";
import QRContainer from '../components/QRContainer';

function Home() {
    const [image, setImage] = useState("")

    //kilder: https://devtrium.com/posts/async-functions-useeffect (31.01.2023)
    const fetchQR = async () => {
        const response = await fetch('qrcode');
        const data = await response.json();
        setImage(data);
    }
    useEffect(() => {

        fetchQR();
    }, []);

    return <>
        <h1> Home page</h1>;
        <QRContainer data={image}></QRContainer>
    </>


}

export default Home;
