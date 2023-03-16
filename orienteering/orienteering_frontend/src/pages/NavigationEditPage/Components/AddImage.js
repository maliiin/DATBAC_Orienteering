import { TextField, Button, Grid, Box } from '@mui/material';
import React, { useState, useEffect } from "react";
import { Link, redirect, useNavigate, useParams } from 'react-router-dom';

export default function AddImage() {
    const [uploadedImage, setUploadedImage] = useState({
        FormFile: ""
    });

    const [testBilde, setTestBilde] = useState("");

    const handleSubmit = async (event) => {
        event.preventDefault();


        //const checkUserUrl = "/api/user/getSignedInUserId";
        //const response1 = await fetch(checkUserUrl);
        //console.log(response1);

        console.log("last bilde")
        //test
        //await fetch('https://localhost:7243/api/checkpoint/test?formFile=heihallo');


        const formData = new FormData();
        formData.append("FormFile", uploadedImage);
        formData.append("FileName", "hei");

        const testData = {
            FileName: "testnavn",
            FormFile: uploadedImage
        }

        console.log(uploadedImage)
        const requestAlternatives = {

            method: 'POST',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',

                //fix linje under -sikkerhet
                'Access-Control-Allow-Origin': '*',
            },
            //body: JSON.stringify(testBilde)
            body: testBilde

        };

        var response = await fetch('/api/checkpoint/AddImage', requestAlternatives);







        //const requestAlternatives = {

        //    method: 'POST',
        //    //mode: 'cors',
        //    headers: {


        //        'Content-Type': 'application/json',
        //        'Accept': 'application/json',

        //        //fix linje under -sikkerhet
        //        'Access-Control-Allow-Origin': '*',
        //    },
        //    body: JSON.stringify(uploadedImage)

        //};

        ////https://localhost:7243/
        //var response = await fetch('/api/checkpoint/AddImage', requestAlternatives);


        console.log("not done yet")




        ////dette er ikke min kode
        //var t=await fetch('api/navigation/AddImage',
        //    {
        //        method: 'POST',
        //        //
        //        mode: 'cors',
        //        headers: {
        //            'Accept': 'application/json',
        //        },
        //        body: uploadedImage
        //    }
        //)
    }

    const handleChange = function (event) {
        event.preventDefault();
        console.log(event.target.files[0])
        console.log(event.target.files[0])
        //var t = URL.createObjectURL(event.target.files[0])
        //console.log(t);

        //før
        //setUploadedImage(prevState => {
        //    return {
        //        ...prevState, FormFile: event.target.files[0]
        //    }

        //}
        //);

        //nå https://stackoverflow.com/questions/55205135/how-to-upload-image-from-react-to-asp-net-core-web-api
        let form = new FormData();
        form.append("FormFile", event.target.files[0]);
        //form.append("FileName", event.target.files[0]);

        setTestBilde({
            FileName: "tetshjfnf",
            FormFile: form
        });

        //setUploadedImage({
        //    Image: "hajskk"
        //});
    }

    return (<>

        <Box onSubmit={handleSubmit} component="form">

            <input
                //multiple
                type="file"
                id="image"
                required
                accept="image/*"
                onChange={handleChange}
            />

            <Button
                type="submit"
            >
                Add Image
            </Button>
        </Box>

    </>);
}