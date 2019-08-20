# Instruction on Setting Up the Server & DB Server

## How To Guides
1. [Prerequisites](#prereq)
2. [Installation of Main Server](#installServer)
3. [Installation of DB API Server](#installDB)


<a name="prereq"></a>
## 1. Prerequisites

1. In order to use either of the server, you must have nodejs installed. 
(https://nodejs.org/en/)
2. In order to use the Database, you must have MongoDB installed (https://docs.mongodb.com/manual/installation/)

   The beauty of using nodejs is that we could use it on any platform 

<a name="installServer"></a>
## 2. Installation of Server

* ## Lazy Way
    * The most lazy way possible to get a server up and running is installing http-server (https://www.npmjs.com/package/http-server)
    1. To install it just issue `npm install -g http-server` command in you terminal window
    2. To use it, goto the directory that contains all the file. In this case `RobotON Server/`
    3. In terminal issue the command
        ```
        http-server ./ -p8080 -cors="*"
        ``` 
    4. You have now setup a basic server that handles GET request



## DB Server API Calls

This instruction will sghow show you how to set up the Server & Database API server packages.

