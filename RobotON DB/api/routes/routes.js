'use strict';
var cors = require('cors')

module.exports = function(app){
    var logControl = require('../controllers/controller');

    app.route('/logsON', cors())
    .get(logControl.list_all_logs_ON)
    .post(logControl.create_a_log_ON);

    app.route('/logsON/:sessionID', cors())
    .get(logControl.read_a_log_ON)
    .put(logControl.update_a_log_ON);

    app.route('/logsBUG', cors())
    .get(logControl.list_all_logs_BUG)
    .post(logControl.create_a_log_BUG);

    app.route('/logsBUG/:sessionID', cors())
    .get(logControl.read_a_log_BUG)
    .put(logControl.update_a_log_BUG);

};