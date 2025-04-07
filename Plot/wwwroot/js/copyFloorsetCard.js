/* Tristan Calay - 3/22/25
Script to manage communication between JS and C#.
Copies the floorset card data to a new card.
*/

var dotNet; //Reference to the DotNet object of the Floorset Editor


function floorsetDashboardSetDotNet(dotNetObject)
{
    if (dotNetObject === null){
        console.log("DotNet argument was null!");
    }
    else{
        dotNet = dotNetObject;
        //console.log("Passed obj: "+dotNetObject)
        //console.log("Stored obj: "+dotNet);
    }
    
}


//Function called when a new fixture is dragged onto the grid. DotNet will search the 
//dict by name to get the Fixture reference.
function jsCreateNewFixture(name) {
    console.log("Calling DotNet Add Fixture: " + name);
    return dotNet.invokeMethodAsync("dictAddNewFixture", name);
}

// Function for the onclick to send back the fixture ID to the C# code.
function selectFixtureByID(fixtureID) {
    console.log("Calling DotNet Select Fixture: " + fixtureID);
    dotNet.invokeMethodAsync("selectFixtureByID", parseInt(fixtureID))
}

function moveFixtureByID(fixtureID, newX, newY) {
    dotNet.invokeMethodAsync("moveFixtureByID", fixtureID, newX, newY)
}



//Call the DotNet method to add a duplicate floorset card.
function floorsetDashboardCopyCard(floorsetName)
{
    //console.log("Floorset Copy Card!");
    //console.log("Stored obj: "+dotNet);
    dotNet.invokeMethodAsync("CopyFloorset",floorsetName);
}