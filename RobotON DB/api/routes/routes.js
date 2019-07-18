'use strict';
module.exports = function(app){
    var logControl = require('../controllers/controller');

    app.route('/logsON')
    .get(logControl.list_all_logs_ON)
    .post(logControl.create_a_log_ON);

    app.route('/logsON/:sessionID')
    .get(logControl.read_a_log_ON)
    .put(logControl.update_a_log_ON);

    app.route('/logsON/currentlevel/:sessionID/:name')
    .put(logControl.put_current_level_ON);

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