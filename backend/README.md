# Blockchain-Simulator : Backend(.Net API Implementation)

The application is designed using React JS as frontend and backend API's in .Net. 
Below demonstrates the steps for running the backend API's:

1.) Install a free copy of Visual Studio IDE (Community Edition): https://www.visualstudio.com/vs/

2.) Once installed, open the solution file (BlockchainSimulator.sln)

    File > Open > Project/Solution
    
3.) Install required Nuget packages for running the application . Below is the command for the same:

    nuget restore BlockchainSimulator.sln

4.) Click the "Start" button, or hit F5 to run. The browser points to http://localhost:44319

    For seeing the chain of all the blocks: 
    Point your webbrowser to https://localhost:44319/api/block/chain

    For mining the block: 
    Point your webbrowser to https://localhost:44319/api/block/mine

    For creating a new transaction :
    Point your webbrowser to https://localhost:44319/api/block/transactions/new

    For registering new nodes :
    http://localhost:44319/api/block/register
    
    For consensus :
    http://localhost:44319/api/block/resolve
    
