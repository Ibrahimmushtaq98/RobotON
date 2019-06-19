'use strict';

var mongoose = require('mongoose'),
    Task = mongoose.model('RobotON_Logs'),
    TaskT = mongoose.model('RobotBug_Logs');


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

