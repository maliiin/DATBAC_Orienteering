//Kilder: https://dev.to/shaedrizwan/building-custom-hooks-in-react-to-fetch-data-4ig6 ( 20.03.2023)
import { useEffect, useState } from "react"



export default function useFetch(url) {

    const [data, setData] = useState(null)
    const [error, setError] = useState(null)

    useEffect(() => {
        (
            async function () {
                try {
                    const response = await fetch(url);
                    setData(response.json)
                } catch (err) {
                    setError(err)
                }
            }
        )()
    }, [url])

    return { data, error }

} 