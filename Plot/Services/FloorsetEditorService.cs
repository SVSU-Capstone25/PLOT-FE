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
    public static void GetFixture(int floorsetId, int Tuid, int EditorTuid, string? Name, int? HangerStack, string? Subcategory, string?
    Note)
    {
        var fixtureInstance = new FixtureInstance
        {
            TUID = Tuid,
            NAME = Name,
            FLOORSET_TUID = floorsetId,
            EDITOR_ID = EditorTuid,
            HANGER_STACK = HangerStack,
            NOTE = Note,
            SUBCATEGORY = Subcategory
        };

        OnFixtureReceived?.Invoke(fixtureInstance);
    }
}
