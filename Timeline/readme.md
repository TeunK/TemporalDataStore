# Duet Asset Management 
Written by Teun kokke, in C# ASP.NET Core

### Structure
The source code can be found inside the `\timeline` directory.

### Order of execution
The Main method can be found inside `\timeline\Program.cs`. This is also where the loop is executed and where commands are read from the process' stdin.
The OperationParser parses the string and converts it into a generic operation. This operation contains any of the specified Create, Update, Get, Delete, Latest or Quit operations, along with a variable that stores which one.
Once an operation is parsed successfully, we execute the `function<Operation, string>` value found in the operationResponses dictionary. (This could alternatively be a set of if-then or switch statements)
The function could be considered as a controller in common MVC patterns, and takes the responsibility of validating the input, handling it (using task delegation) and returning an appropriate response.

The `Timeline.cs` class is what stores the final data. The data can be considered to be in the format of a dictionary<Identifier, BinarySearchTree> with the tree containing Timestamp as key and Observation (data) as value. The tree is in reality a SortedList which keeps track of an array of data as well as the ordering (based on Timestamp as key). Most methods are rather straight-forward. The interesting bit is the way to find the previously-occurring timestamp given a particular timestap that may or may not exist on the timeline.
This is done by doing a (in-place) binary search through the sorted list. Looking at where in the structure the given timestamp should be found. If not found, it simply returns whatever would be immediately on the left of it.

A result screenshot can be found included in `result.PNG`.

I did not add any automated tests as this would be too time-consuming.
However, on request, I will be more than happy to include these to the solution.
(Testing was instead done manually)

### Rules

###### CREATE <id> <timestamp> <data>
   - Creates a new history for the given identifier, if there is no existing history. Adds an 
       observation to the newly created history for the given timestamp and data. CREATE should
       not be executed if the provided identifier already has a history. Returns the data which 
       was inserted, to confirm insertion. 
       
###### UPDATE <id> <timestamp> <data>
   - Inserts an observation for the given identifier, timestamp and data. Returns the data from 
       the prior observation for that timestamp. 
       
###### DELETE <id> [timestamp]
   - If timestamp is provided, deletes all observations for the given identifier from that 
       timestamp forward. Returns the current observation at the given timestamp, or an error if 
       there is no available observation. 
   - If timestamp is not provided, deletes the history for the given identifier, and returns
       the observation with the greatest timestamp from the history which has been deleted. 

###### GET <id> <timestamp>
   - Returns the data from the observation for the given identifier at the given timestamp, or
       an error if there is no available observation. 
       
###### LATEST <id>
   - Locates the observation with the greatest timestamp from the history for the given identifier,
       and responds with its timestamp and data. 

###### QUIT
   - Terminates the process immediately. No response should be written. 