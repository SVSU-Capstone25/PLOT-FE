/* Tristan Calay - 3/22/25
Script to manage communication between JS and C#.
Copies the floorset card data to a new card.
*/

var dotNet; //Reference to the DotNet object of the Floorset Editor


function setDotNet(dotNetObject) {
    if (dotNetObject === null) {
        console.log("DotNet argument was null!");
    }
    else {
        dotNet = dotNetObject;
        //console.log("Passed obj: "+dotNetObject)
        //console.log("Stored obj: "+dotNet);
    }

}

// Function for the onclick to send back the fixture ID to the C# code.
function setSelectedFixture(fixtureID) {
    dotNet.invokeMethodAsync("selectFixtureByID", parseInt(fixtureID))
}

//Call the DotNet method to add a duplicate floorset card.
function floorsetDashboardCopyCard(floorsetName) {
    //console.log("Floorset Copy Card!");
    //console.log("Stored obj: "+dotNet);
    dotNet.invokeMethodAsync("CopyFloorset", floorsetName);
}