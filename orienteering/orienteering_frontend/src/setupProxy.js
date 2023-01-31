const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/weatherforecast",
    "/api/user"///createuser"//,
    //"/api/user/signinuser"

];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'https://localhost:7243',
        secure: false
    });

    app.use(appProxy);
};