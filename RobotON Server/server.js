const http = require('http');
const url = require('url');
const fs = require('fs');
const path = require('path');

const port = 8080;

const mimeType = {
  '.html': 'text/html',
  '.png': 'image/png',
  '.jpg': 'image/jpeg',
  '.mp3': 'audio/mpeg',
  '.mp4': 'video/mp4',
  '.xml': 'application/xml'
};


http.createServer(function (req, res) {
  console.log(`${req.method} ${req.url}`);

  // parse URL
  const parsedUrl = url.parse(req.url);

  // extract URL path
  // Avoid https://en.wikipedia.org/wiki/Directory_traversal_attack
  const sanitizePath = path.normalize(parsedUrl.pathname).replace(/^(\.\.[\/\\])+/, '');
  let pathname = path.join(__dirname, sanitizePath);

  fs.exists(pathname, function (exist) {
    if(!exist) {
      res.statusCode = 404;
      res.end(`File ${pathname} not found!`);
      return;
    }

    // read file from file system
    fs.readFile(pathname, function(err, data){
      if(err){
        res.statusCode = 500;
        res.end(` Error getting the file: ${err}.`);
      } else {
        const ext = path.parse(pathname).ext;

        // res.setHeader("Access-Control-Allow-Credentiald", "true");
        // res.setHeader("Access-Control-Allow-Methods", "GET, POST, PUT");
        // res.setHeader("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Requested-With,content-type");
        // res.setHeader("Access-Control-Allow-Origin", "*");
        res.setHeader('Content-type', mimeType[ext] || 'text/plain' );
        res.setHeader("Accept", "*");
        res.end(data);
      }
    });
  });


}).listen(parseInt(port));

console.log(`Server listening on port ${port}`);