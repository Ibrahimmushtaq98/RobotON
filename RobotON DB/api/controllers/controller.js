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

//Creates a new log for that sessionID
exports.create_a_log_ON = function(req, res) {
  var new_task = new Task(req.body);
  new_task.save(function(err, task) {
    if (err)
      res.send(err);
    res.json(task);
  });
};


//GET REQUEST FOR info Related to the SessionID
exports.read_a_log_ON = function(req, res) {
  Task.findOne({name: req.params.sessionID}, function(err, task) {
    if (err)
      res.send(err);
    res.json(task);
  });
};

//Update the log that is tied with the sessionID
exports.update_a_log_ON = function(req, res) {
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
  });
  Task.find
};

//Todo: Clean the retreived level from sub level, such as level2.1.xml is accepted, however level2.1b.xml isnt
//Gets the retrieved list of completed level
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
          compLevel.push(currentLevel + " " + 1);
          currentLevel = "";
        });
      };
      compLevel = compLevel.filter(onlyUnique);
      //sendBack = compLevel.sort();
      res.json(compLevel.toString());
    }
  });
}

// TODO ADD CHANGES TO BUG
//Updates the current level with the related info
exports.put_current_level_ON = function(req, res){
  var objName = req.params.name;
  var sessionID = req.params.sessionID.toString();

  Task.findOne({name: req.params.sessionID}, 'levels.name', function(err, task){
    var i = 0;
    if(task != null && typeof task == "object"){
      if(err){
        res.json(err);
      }else{
        var taskObj = task.toJSON();
        Object.entries(taskObj.levels).forEach(([key, value]) =>{

          i +=1;
        });
      }
    }
    i -=1;

    var query1 = {}
    var criteria1 = "levels." + i + "." + objName; 
    query1[criteria1] = req.body[objName];
  
    var query = {};
    var criteria = "name";
    query[criteria] = sessionID;

    
    if(req.body['totalPoints'] || req.body['upgrades']){

      var query2 = {}
      var criteria2 = objName; 
      query2[criteria2] = req.body[objName];
      var updateTypes;
      if(req.body['totalPoints']){
        Task.updateOne(
          query,{$set : query2}, {new:true},function(err1,task1){
            if(err){
              res.json("ERR " + err1);
            }else{
              res.json(task1);
            }
          }
        )
      }else{
        Task.updateOne(
          query,{$push : query2}, {new:true},function(err1,task1){
            if(err){
              res.json("ERR " + err1);
            }else{
              res.json(task1);
            }
          }
        )
      }
    }
    else if(req.body["totalPoint"] ||req.body["timeEnded"] || req.body["finalEnergy"] || req.body["progress"] || req.body["time"] || req.body["progress"] || req.body["points"] || req.body["timeBonus"]){
      Task.updateOne(
        query,
        {$set : query1}, 
        {new:true},
        function(err1,task1){
          if(err){
            res.json("ERR " + err1);
          }else{
            res.json(task1);
          }
        }
      )
  
    }else{
      Task.findOneAndUpdate(
        query,
        {$push : query1}, 
        {new:true},
        function(err1,task1){
          if(err){
            res.json("ERR " + err1);
          }else{
            res.json(task1);
          }
        }
      )
  
    }

  });
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
  });
};


exports.read_a_log_BUG = function(req, res) {
  TaskT.findOne({name: req.params.sessionID}, function(err, task) {
    if (err)
      res.send(err);
    res.json(task);
  });
};


exports.update_a_log_BUG = function(req, res) {
  TaskT.findOneAndUpdate({name: req.params.sessionID}, 
    {$push : {levels: req.body['levels']}}, 
    {new: true},
    function(err, task) {
    if (err)
      res.send(err);
    
    if(task == null)
    res.json(task);
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
          compLevel.push(currentLevel + " " + 1);
          currentLevel = "";
        });
      };
      compLevel = compLevel.filter(onlyUnique);
      //sendBack = compLevel.sort();
      res.json(compLevel.toString());
    }
  });
}

//Updates the current level with the related info
exports.put_current_level_BUG = function(req, res){
  var objName = req.params.name;
  var sessionID = req.params.sessionID.toString();

  TaskT.findOne({name: req.params.sessionID}, 'levels.name', function(err, task){
    var i = 0;
    if(task != null && typeof task == "object"){
      if(err){
        res.json(err);
      }else{
        var taskObj = task.toJSON();
        Object.entries(taskObj.levels).forEach(([key, value]) =>{
          i +=1;
        });
      }
    }
    i -=1;

    var query1 = {}
    var criteria1 = "levels." + i + "." + objName; 
    query1[criteria1] = req.body[objName];
  
    var query = {};
    var criteria = "name";
    query[criteria] = sessionID;
    
    if(req.body['totalPoints'] || req.body['upgrades']){

      var query2 = {}
      var criteria2 = objName; 
      query2[criteria2] = req.body[objName];
      var updateTypes;
      if(req.body['totalPoints']){
        TaskT.updateOne(
          query,{$set : query2}, {new:true},function(err1,task1){
            if(err){
              res.json("ERR " + err1);
            }else{
              res.json(task1);
            }
          }
        )
      }else{
        TaskT.updateOne(
          query,{$push : query2}, {new:true},function(err1,task1){
            if(err){
              res.json("ERR " + err1);
            }else{
              res.json(task1);
            }
          }
        )
      }
    }
    else if(req.body["totalPoint"] ||req.body["timeEnded"] || req.body["finalEnergy"] || req.body["progress"] || req.body["time"] || req.body["progress"] || req.body["points"] || req.body["timeBonus"]){
      TaskT.updateOne(
        query,
        {$set : query1}, 
        {new:true},
        function(err1,task1){
          if(err){
            res.json("ERR " + err1);
          }else{
            res.json(task1);
          }
        }
      )
  
    }else{
      TaskT.findOneAndUpdate(
        query,
        {$push : query1}, 
        {new:true},
        function(err1,task1){
          if(err){
            res.json("ERR " + err1);
          }else{
            res.json(task1);
          }
        }
      )
  
    }

  });
};