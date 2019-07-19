'use strict';
 var mongoose = require('mongoose');
 var Schema = mongoose.Schema;
//Models for the DB

 var roboSchema = new Schema({
    name: {
        type: String,
        unique: true,
        required: 'Please Enter your name'
    },
    timeStarted: String,
    levels:[{
        name: String,
        time: String,
        progress: String,
        timeStarted: String,
        timeEnded: String,
        stars: String,
        points: String,
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
            position: {
                line: String,
                x_pos: String,
                y_pos: String
            },
            progress: String,
            time: String,
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
            position: {
                line:String,
                x_pos:String,
                y_pos:String
            },
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