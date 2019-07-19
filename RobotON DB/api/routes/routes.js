'use strict';
//The api routes
module.exports = function(app){
    var logControl = require('../controllers/controller');

    //A Get Request will show all the information stored on the DB
    //A Post Request will start the new session for a USER
    app.route('/logsON')
    .get(logControl.list_all_logs_ON)
    .post(logControl.create_a_log_ON);

    //A Get Request will show only the information that is tied with the sessionID
    //A Put requst will update the information related to the sessionID
    app.route('/logsON/:sessionID')
    .get(logControl.read_a_log_ON)
    .put(logControl.update_a_log_ON);

    //A Put requst will update currentlevel with the related info
    app.route('/logsON/currentlevel/:sessionID/:name')
    .put(logControl.put_current_level_ON);

    //A Get Request will grab all the completed level that the user has done
    app.route('/logsON/completedlevels/:sessionID')
    .get(logControl.retrieve_comp_level_ON);

    app.route('/logsBUG')
    .get(logControl.list_all_logs_BUG)
    .post(logControl.create_a_log_BUG);

    app.route('/logsBUG/:sessionID')
    .get(logControl.read_a_log_BUG)
    .put(logControl.update_a_log_BUG);

    app.route('/logsBUG/completedlevels/:sessionID')
    .get(logControl.retrieve_comp_level_BUG);

    app.route('/logsBUG/currentlevel/:sessionID/:name')
    .put(logControl.put_current_level_BUG);

};