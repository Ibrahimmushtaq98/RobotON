'use strict';
 var mongoose = require('mongoose');
 var Schema = mongoose.Schema;

 var roboSchema = new Schema({
    name: {
        type: String,
        unique: true,
        required: 'Please Enter your name'
    },
    levels:[{
        name: String,
        time: String,
        progress: String,
        tools :[{
            name: String,
            correctLine: String,
            reqTask: String,
            compTask: String,
            timeTool: String,
            lineUsed: String
        }],
        states :[{
            preEnergy: String,
            finEnergy: String,
            toolName: String,
            toolLine: String,
            position: String,
            progress: String,
            timestamp: String
        }],
        obstacal: [{
            name: String,
            line: String
        }],
        obstacalState: [{
            name: String,
            preEnergy: String,
            finEnergy: String,
            line: String,
            position: String,
            timestamp: String
        }],
        enemy: [{
            name: String,
            preEenergy: String,
            finEnergy: String,
            movement: String,
            vision: String,

        }]
    }]
});

module.exports = mongoose.model('RobotON_Logs', roboSchema);
module.exports = mongoose.model('RobotBug_Logs', roboSchema);