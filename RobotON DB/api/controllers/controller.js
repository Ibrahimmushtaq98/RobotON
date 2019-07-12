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

//Todo: Clean the retreived level from sub level, such as level2.1.xml is accepted, however level2.1b.xml isnt
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

          if(currentLevel != ""){
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

exports.list_current_level_ON = function(req, res){
  console.log("SessionID: " + req.params.sessionID);

  Task.findOne({name: req.params.sessionID},{'levels': {$slice: -1}}, function(err, task){
    if(err){
      res.json(err);
    }else{
      try{
        res.json(task.levels[0]._id);
      }catch(err){
        
      }
    }
    
  })
};

exports.put_current_level_ON = function(req, res){
  var objName = req.params.name;
  var positionalID = req.params.sessionID.toString();
  var currentLevelID = req.params.currentlevelID.toString();
  console.log(positionalID + " " + currentLevelID);

  var query1 = {}
  var criteria1 = "levels." + positionalID + "." + objName; 
  query1[criteria1] = req.body[objName];

  var query = {};
  var criteria = "levels." + positionalID + "._id";
  query[criteria] = currentLevelID;

  if(req.body['timeEnded'] || req.body['progress']){
    Task.updateOne(
      query,
      {$set : query1}, 
      {new:true},
      function(err,task){
        if(err){
          res.json("ERR " + err);
        }else{
          //console.log("task: " + task);
          res.json(task);
        }
      }
    )

  }else{
    Task.findOneAndUpdate(
      query,
      {$push : query1}, 
      {new:true},
      function(err,task){
        if(err){
          res.json("ERR " + err);
        }else{
          //console.log("task: " + task);
          res.json(task);
        }
      }
    )

  }
};

exports.get_total_level_ON = function(req, res){
  Task.findOne({name: req.params.sessionID}, 'levels.name', function(err, task){
    var i = 0;

    if(task != null && typeof task == "object"){
      if(err){
        res.json(0);
      }else{
        var taskObj = task.toJSON();
        Object.entries(taskObj.levels).forEach(([key, value]) =>{
          i = i + 1;
        });
      }
    }
    res.json(i);
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

          if(currentLevel != ""){
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

exports.list_current_level_BUG = function(req, res){
  console.log("SessionID: " + req.params.sessionID);

  TaskT.findOne({name: req.params.sessionID},{'levels': {$slice: -1}}, function(err, task){
    if(err){
      res.json(err);
    }else{
      res.json(task.levels[0]._id);
    }
    
  })
};

exports.put_current_level_BUG = function(req, res){
  var objName = req.params.name;
  var positionalID = req.params.sessionID.toString();
  var currentLevelID = req.params.currentlevelID.toString();
  console.log(positionalID + " " + currentLevelID);

  var query1 = {}
  var criteria1 = "levels." + positionalID + "." + objName; 
  query1[criteria1] = req.body[objName];

  var query = {};
  var criteria = "levels." + positionalID + "._id";
  query[criteria] = currentLevelID;

  if(req.body['timeEnded'] || req.body['progress']){
    TaskT.updateOne(
      query,
      {$set : query1}, 
      {new:true},
      function(err,task){
        if(err){
          res.json("ERR " + err);
        }else{
          //console.log("task: " + task);
          res.json(task);
        }
      }
    )

  }else{
    TaskT.findOneAndUpdate(
      query,
      {$push : query1}, 
      {new:true},
      function(err,task){
        if(err){
          res.json("ERR " + err);
        }else{
          //console.log("task: " + task);
          res.json(task);
        }
      }
    )

  }
};

exports.get_total_level_BUG = function(req, res){
  TaskT.findOne({name: req.params.sessionID}, 'levels.name', function(err, task){
    var i = 0;

    if(task != null && typeof task == "object"){
      if(err){
        res.json(0);
      }else{
        var taskObj = task.toJSON();
        Object.entries(taskObj.levels).forEach(([key, value]) =>{
          i = i + 1;
        });
      }
    }
    res.json(i);
  });
}