const http = require('http');
const url = require('url');
const fs = require('fs');
const path = require('path');
const { parse } = require('querystring');

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

  // extract URL path
  // Avoid https://en.wikipedia.org/wiki/Directory_traversal_attack
  if(req.method === "POST"){
    let body = '';
    req.on('data', chunk => {
        body += chunk.toString();
    }); 
    req.on('end', () => {
      var parsString = body.toString();        
        var jsonString = JSON.parse(parsString);
        console.log("commits :" + jsonString.commits);

        try{
          if(jsonString.commits.length != 0){
            console.log("Git Push Request");

            var sys = require('util'),
            exec = require('child_process').exec;

            exec("run.bat", function(err, stdout, stderr) {
              console.log("run.bat: " + err + " : "  + stdout);
            });
          }
        }catch{

        }

        res.end('ok');
    });
  }else if(req.method === "GET"){
    const parsedUrl = url.parse(req.url);
    const sanitizePath = path.normalize(parsedUrl.pathname).replace(/^(\.\.[\/\\])+/, '');
    let pathname = path.join(__dirname, sanitizePath);
    fs.exists(pathname, function (exist) {
      if(!exist) {
        res.statusCode = 404;
        res.end(`File not found!`);
        return;
      }

      // read file from file system
      fs.readFile(pathname, function(err, data){
        if(err){
          res.statusCode = 500;
          res.end(` Error getting the file: ${err}.`);
        } else {
          const ext = path.parse(pathname).ext;

          res.setHeader("Access-Control-Allow-Credentiald", "true");
          res.setHeader("Access-Control-Allow-Methods", "GET, POST, PUT");
          res.setHeader("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Requested-With,content-type");
          res.setHeader("Access-Control-Allow-Origin", "*");
          res.setHeader('Content-type', mimeType[ext] || 'text/plain' );
          res.setHeader("Accept", "*");
          res.end(data);
        }
      });
    });
}


}).listen(parseInt(port));

console.log(`Server listening on port ${port}`);

function collectRequestData(request, callback) {
  const FORM_URLENCODED = 'application/x-www-form-urlencoded';
  if(request.headers['content-type'] === FORM_URLENCODED) {
      let body = '';
      request.on('data', chunk => {
          body += chunk.toString();
      });
      request.on('end', () => {
          callback(parse(body));
      });
  }
  else {
      callback(null);
  }
}