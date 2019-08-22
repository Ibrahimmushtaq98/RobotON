# Instruction on Setting Up the Server & DB Server

## How-To Guides
1. [Prerequisites](#prereq)
2. [Installation of Main Server](#installServer)
3. [Installation of DB API Server](#installDB)
4. [What about the Game?](#installGame)


<a name="prereq"></a>
## 1. Prerequisites

1. To use either of the servers, you must have Nodejs installed. 
(https://nodejs.org/en/)
2. To use the Database, you must have MongoDB installed (https://docs.mongodb.com/manual/installation/)
3. We are going to need the server file and the API file. This is located at https://github.com/Ibrahimmushtaq98/RobotON/tree/ServerBranch in `./RobotON DB`, `./RobotON Server` and `./Bat-Term` (Just download the branch, and use that three file)


<a name="installServer"></a>
## 2. Installation of Server

* ## Lazy Way
    * The laziest way possible to get a server up and running is installing http-server (https://www.npmjs.com/package/http-server)
    1. To install it just issue `npm install -g http-server` command in your terminal window
    2. To use it, go to the directory that contains all the file. In this case `RobotON Server/`
    3. In the terminal issue the command which will start the server in port 8080

* ## The way I used (not preferred)
    * The reason why this is not preferred is that this server was set up to listen to any incoming changes from Github. If there were any changes, it would start the auto compiler. However, the server would get knocked offline, since it wouldn't know how to handle some request
    * if you are planning to use this server to auto compile, then there are two options.

        * ### Auto Compile with Mail (Only use this on a test server, Not for Production)
            1. To receive mail on when the compile started and ended you first must make a JSON file called `config.json` located outside of the `RobotON Server` directory
            2. The content of the file inside look like this (Yes storing password openly):
                ```json
                {
                    "email": "EMAIL FOR SENDING TO EMAIL2",
                    "password" : "EMAIL PASSWORD",
                    "email2" : "EMAIL FOR RECEIVING EMAIL"
                }
                ```

            3. Setup the Github webhook in your Github Project setting
            4. Depending on what system you are using, we will need to configure the script accordingly
                1. In `RobotON Server/` there is either a file called `CompileProject.sh`(Linux) or `run.bat`(Window)
                    1. In `run.bat` or `CompileProject.sh` you are going to have to set the path to where your Unity installation is installed, and on the same line, where `-projectPath` is located, set the path to where the project is being held at
                    2. The first cd command in the `run.bat`, should have the appropriate path on where the server is located, the second should be the appropriate path to where the `Bat-Term Script` is being held
                2. In `server.js` line 64 `child.spawn("./CompileProject.sh");` change the `CompileProject.sh` to `run.bat` if you are using Windows
            5. In the directory of `RobotON Server`, opening the terminal, and type in `npm start`
            6. You should be good, if there is any trouble, it could just be a path problem
        * ### Auto Compile without Mail
            1. Setup the Github webhook in your Github Project setting
            2. Depending on what system you are using, we will need to configure the script accordingly
                1. In `RobotON Server/` there is either a file called `CompileProject.sh`(Linux) or `run.bat`(Window)
                    1. In `run.bat` or `CompileProject.sh` you are going to have to set the path to where your Unity installation is installed, and on the same line, where `-projectPath` is located, set the path to where the project is being held at
                    2. The first cd command in the `run.bat`, should have the appropriate path on where the server is located, the second should be the appropriate path to where the `Bat-Term Script` is being held
                2. In `server.js` line 64 `child.spawn("./CompileProject.sh");` change the `CompileProject.sh` to `run.bat` if you are using Windows
            3. In `server.js` comment out this code
                ```js
                    var transporter = nodemailer.createTransport({
                        service: 'gmail',
                        auth: {
                            user: config.email,
                            pass: config.password
                        }
                    });
                ```
                and this code

                ```js
                    if(jsonString.commits.length != 0){
                        console.log("Git Push Request");
                        var dateTime = new Date();
                        //The code below should be commented out
                        sendEmail("Unity Compilation Notice!", "Started Compiling at " + dateTime );

                ```
            4. In the directory of `RobotON Server`, opening the terminal, and type in `npm start`               
## DB Server Installation
* ## Installation
    * To use the DB such that the game can be communicated with server, we are going to use the `./RobotON DB` Directory
    1. In the terminal, type `mongod`, this will start the DB server. If the command `mongod` is not found, you may have to set the appropriate path. If it does launches but quickly closes, then you may have to set the `/data/db` directory in your system
    2. In another Terminal, type `npm start`
    3. You are done! :)
* ## What does it store?
The model on which the DB store data is :
```javascript
 var roboSchema = new Schema({
    name: {
        type: String,
        unique: true,
        required: 'Please Enter your name!'
    },
    username: String,
    timeStarted: String,
    totalPoints: String,
    currentPoints: String,
    speedUpgrades: String,
    xpUpgrades: String,
    resistanceUpgrade: String,
    energyUpgrades: String,
    upgrades: [{
        name: String,
        timestamp: String,
        prePoints: String,
        curPoints: String,
    }],
    levels:[{
        name: String,
        time: String,
        progress: String,
        timeStarted: String,
        timeEnded: String,
        stars: String,
        points: String,
        totalPoint: String,
        timeBonus: String,
        finalEnergy: String,
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
        obstacle: [{
            name: String,
            line: String
        }],
        obstacleState: [{
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
            preEnergy: String,
            finEnergy: String,
            position: {
                line:String,
                x_pos:String,
                y_pos:String
            },
            timestamp: String

        }],
    }]
});
```
* ## API Call Usage
### For RoboON
    
|             URL endpoint             |  Method  |                                                                What it does                                                                |
|:-------------------------------------:|:--------:|:------------------------------------------------------------------------------------------------------------------------------------------:|
|                /logsON                | GET,POST |                                    GET: Returns all Data in DB POST: Used for putting Initial Data to DB                                   |
|           /logsON/:sessionID          |  GET,PUT |                             GET: Returns Data that is tied to that SessionID PUT: Put Initial Level Data to DB                             |
|   /logsON/completedlevels/:sessionID  |    GET   |                       GET: Returns all completed level that is tied to the sessionID, plus the level the user was on                       |
| /logsON/currentlevel/:sessionID/:name |    PUT   |                     PUT: Put Data to the current level that the sessionID is tied to name is there to Put certain data                    |
|    /logsON/points/:sessionID/:name    |  GET,PUT | GET: Returns points that are tied to the sessionID PUT: Puts points that are tied to the sessionID name is there to Get/Put certain points |
|          /logsON/check/:word          |    GET   |            GET: Returns true if the username exists, or if the username has explicit word else it would return false (NOT IN USE)           |
|     /logsON/leaderboard/:levelName    |    GET   |                                       GET: Returns all user with points, and the ranking (NOT IN USE)                                      |

### For RoboBUG
|              URL endpoint              	|  Method  	|                                                                What it does                                                                	|
|:--------------------------------------:	|:--------:	|:------------------------------------------------------------------------------------------------------------------------------------------:	|
|                /logsBUG                	| GET,POST 	|                                    GET: Returns all Data in DB POST: Used for putting Initial Data to DB                                   	|
|           /logsBUG/:sessionID          	|  GET,PUT 	|                             GET: Returns Data that is tied to that SessionID PUT: Put Initial Level Data to DB                             	|
|   /logsBUG/completedlevels/:sessionID  	|    GET   	|                       GET: Returns all completed level that is tied to the sessionID, plus the level the user was on                       	|
| /logsBUG/currentlevel/:sessionID/:name 	|    PUT   	|                     PUT: Put Data to the current level that the sessionID is tied to  name is there to Put certain data                    	|
|    /logsBUG/points/:sessionID/:name    	|  GET,PUT 	| GET: Returns points that are tied to the sessionID PUT: Puts points that are tied to the sessionID name is there to Get/Put certain points 	|
|           /logsON/check/:word          	|    GET   	|            GET: Returns true if the username exist, or if the username has explicit word else it would return false (NOT IN USE)           	|
|     /logsON/leaderboard/:levelName     	|    GET   	|                                       GET: Returns all user with points, and the ranking (NOT IN USE)                                      	|

<a name="installGame"></a>
# Yes, What about the Game?

Well Assuming you have the game compiled for web, and you know the directory, There are two options
1. Put the game folder, where the Game File server is located, and all you have to do is enter in the browser `<URL>:<Port>\(BuildFolderName)`
2. Using the lazy way in Installing the Server, Go into the build file, and launch `http-server` from there

<a name="installPorts"></a>
# What about ports?

Assuming you have port forwarded some other ports than the one I used, it is really simple, 
* For the `http-server`, just set the `-p` argument with the port number you like
* For `./RobotON Server`, just set the port number in `server.js` to whatever you like, the same thing with `./RobotON DB`
* I would suggest leaving mongo port not forwarded, and kept the same

## That's it
### If there is any problem/question, just send me an email, and I'll reply as quickly as I could