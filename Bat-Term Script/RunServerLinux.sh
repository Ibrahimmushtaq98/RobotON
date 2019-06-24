#! /bin/bash
gnome-terminal --working-directory=/home/ibrahimmushtaq/Desktop/RobotON\ Master/RobotON\ Ibrahim/ -e 'killall -9 node'
gnome-terminal -c sudo 'service mongod restart' 
gnome-terminal --working-directory=/home/ibrahimmushtaq/Desktop/RobotON\ Master/RobotON\ Ibrahim/RobotON/RobotON\ DB/ -e 'npm start'
gnome-terminal --working-directory=/home/ibrahimmushtaq/Desktop/RobotON\ Master/RobotON\ Ibrahim/RobotON/RobotON\ Server/ -e 'npm start'
gnome-terminal --working-directory=/home/ibrahimmushtaq/Desktop/Build/ -e 'http-server /home/ibrahimmushtaq/Desktop/Build/  -p8081'