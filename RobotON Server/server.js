const http = require('http');
const url = require('url');
const fs = require('fs');
const path = require('path');
const { parse } = require('querystring');
const nodemailer = require('nodemailer');
const config = require('../config.json');
const port = 8080;

var transporter = nodemailer.createTransport({
  service: 'gmail',
  auth: {
    user: config.email,
    pass: config.password
  }
});

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
    try{
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
              var dateTime = new Date();
              sendEmail("Unity Compilation Notice!", "Started Compiling at " + dateTime );

              var sys = require('util'),
              child = require('child_process');
              child.spawn("run.bat");
              child.on('exit', code=>{
                console.log('Exit code: ${code}');
                //sendMail("Unity Compilation Notice!", "Finished Compiling at " + dateTime );
              });

              // exec("run.bat", function(err, stdout, stderr) {
              //   console.log("run.bat: " + err + " : "  + stdout);
              // });

              // exec.on
            }
          }catch(e){

          }

          res.end('ok');
      });
    }catch(e){

    }
  }else if(req.method === "GET"){
    try{
    const parsedUrl = url.parse(req.url);
    const sanitizePath = path.normalize(parsedUrl.pathname).replace(/^(\.\.[\/\\])+/, '');
    let pathname = path.join(__dirname, sanitizePath);
    fs.exists(pathname, function (exist) {
      if(!exist) {
        res.statusCode = 404;
        res.setHeader("Access-Control-Allow-Origin", "*");
        res.end(`File not found!`);
        return;
      }

      // read file from file system
      fs.readFile(pathname, function(err, data){
        if(err){
          res.statusCode = 500;
          res.setHeader("Access-Control-Allow-Origin", "*");
          res.end(` Error getting the file: ${err}.`);
        } else {
          const ext = path.parse(pathname).ext;

          res.setHeader("Access-Control-Allow-Origin", "*");
          res.setHeader("Access-Control-Allow-Credentiald", "true");
          res.setHeader("Access-Control-Allow-Methods", "GET, POST, PUT");
          res.setHeader("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Requested-With,content-type");
          res.setHeader('Content-type', mimeType[ext] || 'text/plain' );
          res.setHeader("Accept", "*");
          res.end(data);
        }
      });
    });
  }catch(e){
    console.log(e);
  }
}


}).listen(parseInt(port));

function sendEmail(subject, message){
  var mailOptions = {
      from: config.email,
      to: config.email2,
      subject: subject,
      text: message
  };

  transporter.sendMail(mailOptions, function(err, info){
      if(err){
        console.log(err);
      }else{
        console.log("Email Sent"+ info.response);
      }
  });
}

console.log(`Server listening on port ${port}`);