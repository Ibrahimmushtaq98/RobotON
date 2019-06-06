'use strict';

module.exports = function(app){
    var logControl = require('../controllers/controller');

    app.route('/logsON')
    .get(logControl.list_all_logs)
    .post(logControl.create_a_log);

    app.route('/logsON/:sessionID')
    .get(logControl.read_a_log)
    .put(logControl.update_a_log)

};