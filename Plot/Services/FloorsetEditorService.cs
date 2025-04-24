using Microsoft.JSInterop;
using Plot.Data.Models.Fixtures;

public class FloorsetEditorService
{
    public static event Func<Task>? OnUpdate;
    public static Action<FixtureInstance>? OnFixtureReceived;

    public static async Task TriggerUpdateAsync()
    {
        if (OnUpdate != null)
        {
            await OnUpdate.Invoke();
        }
    }

    [JSInvokable("UpdateAllocations")]
    public static async Task UpdateAllocations()
    {
        await TriggerUpdateAsync();
    }

    [JSInvokable("GetFixture")]
    public static void GetFixture(int floorsetId, int Tuid, string? Name, int? HangerStack, string? Subcategory, string?
    Note)
    {

        Console.WriteLine(Subcategory);
        var fixtureInstance = new FixtureInstance
        {
            TUID = Tuid,
            NAME = Name,
            FLOORSET_TUID = floorsetId,
            HANGER_STACK = HangerStack,
            NOTE = Note,
            SUBCATEGORY = Subcategory
        };

        OnFixtureReceived?.Invoke(fixtureInstance);
    }
}
