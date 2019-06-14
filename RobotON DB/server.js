var express = require('express'),
app = express(),
port = 3000,
mongoose = require('mongoose'),
Task = require('./api/models/model'),
bodyParser = require('body-parser');

var cors = require('cors');

//app.use(cors)


var url = "mongodb://localhost:27017/robo";

mongoose.Promise = global.Promise;
mongoose.connect(url, { useNewUrlParser: true });

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

var routes = require('./api/routes/routes');
routes(app);


app.listen(port);

console.log("Server started on port " + port);

