async function addScore(points) {

    const requestAlternatives = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        },
        body: JSON.stringify({ Score: "" + points })
    };
    await fetch("/api/session/addScore", requestAlternatives)
}
export default addScore;