#! /bin/bash
cd ../
echo Now Compiling Project
/home/ibrahimmushtaq/Unity/Hub/Editor/2019.2.0b6/Editor/Unity -quit -force-free -nographics -batchmode -logFile stdout.log -projectPath /home/ibrahimmushtaq/Desktop/RobotON\ Master/RobotON\ Ibrahim/RobotON/ -executeMethod WebGLBuilder.build
echo Finished Job, check Logs!
cd ../
gnome-terminal --working-directory=/home/ibrahimmushtaq/Desktop/RobotON\ Master/RobotON\ Ibrahim/RobotON/Bat-Term\ Script/ -e './CopyFIleToServerLinux.sh '
gnome-terminal --working-directory=/home/ibrahimmushtaq/Desktop/RobotON\ Master/RobotON\ Ibrahim/RobotON/Bat-Term\ Script/ -e './RunServerLinux.sh'