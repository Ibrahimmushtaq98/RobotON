'use strict';

var mongoose = require('mongoose'),
    Task = mongoose.model('RobotON_Logs'),
    TaskT = mongoose.model('RobotBug_Logs');

//-------------------------------EXTERNAL FUNCTIONS------------------------------------------->
function onlyUnique(value, index, self){
  return self.indexOf(value) === index;
}
//-------------------------------EOF EXTERNAL FUNCTIONS!--------------------------------------->


//GET REQUEST FOR ALL DATA UNDER ON
exports.list_all_logs_ON = function(req,res){
    Task.find({}, function(err, task) {
        if (err)
          res.send(err);
        res.json(task);
      });
};

exports.create_a_log_ON = function(req, res) {
  console.log(req.body);
  var new_task = new Task(req.body);
  new_task.save(function(err, task) {
    if (err)
      res.send(err);
    res.json(task);
    console.log(task);
  });
};


exports.read_a_log_ON = function(req, res) {
    console.log(req.params);
  Task.findOne({name: req.params.sessionID}, function(err, task) {
    if (err)
      res.send(err);
    res.json(task);
  });
};


exports.update_a_log_ON = function(req, res) {
  //console.log(req.body);
  Task.findOneAndUpdate(
    {name: req.params.sessionID}, 
    {$push : {
      levels: req.body['levels']}}, 
    {new: true},
    function(err, task) {
    if (err){
      res.send(err);
    }
    res.json(task);
    console.log(task);
  });
  Task.find
};

exports.retrieve_comp_level_ON = function(req, res){
  var compLevel = [];
  var currentLevel;
  Task.findOne({name: req.params.sessionID}, function(err, task) {
    if (err){
      res.send(err);
    }else{
      if(task != null && typeof task == "object"){
        var jsonObjects = task.toJSON();
        Object.entries(jsonObjects.levels).forEach(([key, value]) =>{

          currentLevel = value.name;

          if(value.progress == "Passed" && currentLevel != ""){
            compLevel.push(currentLevel);
            currentLevel = "";
          }
        });
      };
      compLevel = compLevel.filter(onlyUnique);
      //sendBack = compLevel.sort();
      res.json(compLevel);
    }
  });
}

//------------------------------------------------------------------------------->
exports.list_all_logs_BUG = function(req,res){
  TaskT.find({}, function(err, task) {
      if (err)
        res.send(err);
      res.json(task);
    });
};

exports.create_a_log_BUG = function(req, res) {
  var new_task = new TaskT(req.body);
  new_task.save(function(err, task) {
    if (err)
      res.send(err);
    res.json(task);
    console.log(task);
  });
};


exports.read_a_log_BUG = function(req, res) {
  console.log(req.params);
  TaskT.findOne({name: req.params.sessionID}, function(err, task) {
    if (err)
      res.send(err);
    res.json(task);
  });
};


exports.update_a_log_BUG = function(req, res) {
  console.log(req.body);
  TaskT.findOneAndUpdate({name: req.params.sessionID}, 
    {$push : {levels: req.body['levels']}}, 
    {new: true},
    function(err, task) {
    console.log("PUT");
    console.log(req.body);
    if (err)
      res.send(err);
    
    if(task == null)
    res.json(task);
    console.log(task);
  });
  TaskT.find
};

exports.retrieve_comp_level_BUG = function(req, res){
  var compLevel = [];
  var currentLevel;
  TaskT.findOne({name: req.params.sessionID}, function(err, task) {
    if (err){
      res.send(err);
    }else{
      if(task != null && typeof task == "object"){
        var jsonObjects = task.toJSON();
        Object.entries(jsonObjects.levels).forEach(([key, value]) =>{

          currentLevel = value.name;

          if(value.progress == "Passed" && currentLevel != ""){
            compLevel.push(currentLevel + " " + 1);
            currentLevel = "";
          }
        });
      };
      compLevel = compLevel.filter(onlyUnique);
      //sendBack = compLevel.sort();
      res.json(compLevel.toString());
    }
  });
}
