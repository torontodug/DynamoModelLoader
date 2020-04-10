# TorontoDUG: Dynamo Model Loader

## A set of zero-touch Dynamo nodes in order to import 3D mesh files into Dynamo.


![alt text](https://github.com/torontodug/DynamoModelLoader/blob/master/DynModelLoader/img/ObjImport.png "Obj Import Example")

To use these nodes into your Dynamo environment:

1. Go to https://github.com/torontodug/DynamoModelLoader/releases and download the latest release zip file.
2. Once you have downloaded the zip file, right-click the zip file, select properties from the menu, check the "Unblock" check box at the bottom right of the window that pops up, and click Apply. This is a security feature enabled by windows where it blocks externally downloaded .dll files in zip files. We want to unblock these files because our zero-touch nodes are in the .dll files.
3. Extract the contents of the zip file to the default location of Dynamo packages, which is located here:
   
   C:\Users\\**USERNAME**\\AppData\Roaming\Dynamo\Dynamo Core\\**DYNAMOVERSION**\\packages

    Where USERNAME is your windows profile username, and DYNAMOVERSION is the version of dynamo that you are running.

4. Once the files have been extracted, you can find the packages' zero-touch nodes under the "ToDUG Model Loader" category under the Add-ons section of your nodes list in Dynamo.

This package currently has support for the following 3D mesh files:

- OBJ
- ~~IFC~~ (To be implemented soon)
- ~~FBX~~ (To be implemented soon)
- ~~STL~~ (To be implemented soon)

For issues with the package, please submit an issue to this repository with the following information:

- What happened?
- What were you expecting?
- What version of Dynamo were you using?
- A download link for the 3D mesh file that you used (optional, but not necessary)

If you would like to contribute to this project, please submit a pull request and we will review it. If everything with it looks good, we will merge it with the main project. If not, we will request some changes. Please also supply some test files and results with your pull request.

Credits:
- Wavefront OBJ C# File Loader: https://github.com/chrisjansson/ObjLoader

Enjoy, and feel free to discuss this project on our slack at torontodug.slack.com

