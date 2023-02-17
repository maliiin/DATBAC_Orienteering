const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    //"/api/user",
    //"/api/qrcode",
    //"/api/track",
    //"/api/checkpoint",
    //"/api/quiz"

    "/api/"

];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'https://localhost:7243',
        secure: false
    });

    app.use(appProxy);
};