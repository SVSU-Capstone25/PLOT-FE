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

//Call the DotNet method to add a duplicate floorset card.
function floorsetDashboardCopyCard(floorsetName)
{
    //console.log("Floorset Copy Card!");
    //console.log("Stored obj: "+dotNet);
    dotNet.invokeMethodAsync("CopyFloorset",floorsetName);
}