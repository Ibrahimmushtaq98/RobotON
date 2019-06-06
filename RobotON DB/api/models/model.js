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
            reqTask: String,
            compTask: String,
            timeTool: String,
            lineUsed: String
        }]
    }]
});

module.exports = mongoose.model('RobotON_Logs', roboSchema);
module.exports = mongoose.model('RobotBug_Logs', roboSchema);