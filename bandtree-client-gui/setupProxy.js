const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function (app) {
  app.use(
    '/wikipedia-search',
    createProxyMiddleware({
      target: 'https://localhost:7088', // Your server's URL
      changeOrigin: true,
    })
  );
};
