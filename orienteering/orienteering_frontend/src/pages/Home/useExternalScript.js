import { useEffect, useState } from "react";
//fiks kilde https://plainenglish.io/blog/how-to-load-a-dynamic-script-in-react-2940d30998dd 
//08/03
export default function useExternalScript(url) {
    let [state, setState] = useState(url ? "loading" : "idle");

    useEffect(() => {
        if (!url) {
            setState("idle");
            return;
        }

        let script = document.querySelector(`script[src="${url}"]`);

        const handleScript = (e) => {
            setState(e.type === "load" ? "ready" : "error");

        };

        if (!script) {
            script = document.createElement("script");
            script.type = "application/javascript";
            script.src = url;
            script.async = true;
            document.body.appendChild(script);
            script.addEventListener("load", handleScript);
            script.addEventListener("error", handleScript);
            //malin
            console.log(script.test());
        }

        script.addEventListener("load", handleScript);
        script.addEventListener("error", handleScript);

        return () => {
            script.removeEventListener("load", handleScript);
            script.removeEventListener("error", handleScript);
        };
    }, [url]);

    return state;
};