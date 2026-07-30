using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Services;

namespace UniverBlazored.Generic.Services;

public class UniverUserManager
{
    private readonly IUniverJsInterop univerJS;
    private readonly string instanceId;

    public UniverUserManager(IUniverJsInterop service, string instanceId)
    {
        univerJS = service;
        this.instanceId = instanceId;
    }

    private SpreadsheetOperationContext Ctx => new(instanceId, null, OperationKind.Structural);

    public async Task<UniverUser> GetCurrentUser()
    {
        var queue = new UniverQueue(univerJS, Ctx);
        queue.SetAction("getUserService").SetAction("getCurrentUser");
        return await queue.ResolveQueueAsync<UniverUser>();
    }

    public async Task<List<UniverUser>> ListAllUsers()
    {
        var queue = new UniverQueue(univerJS, Ctx);
        queue.SetAction("getUserService").SetAction("list");
        return await queue.ResolveQueueAsync<List<UniverUser>>();
    }

    public async Task<UniverUser> GetUser(string idUser)
    {
        var queue = new UniverQueue(univerJS, Ctx);
        queue.SetAction("getUserService").SetAction("getUser", idUser);
        return await queue.ResolveQueueAsync<UniverUser>();
    }

    public async Task AddUser(UniverUser user, bool setCurrent = false)
    {
        await Add(user);
        if (setCurrent)
            await SetCurrentUser(user);
    }

    public async Task AddUser(params UniverUser[] users)
    {
        foreach (var user in users)
            await Add(user);
    }

    public async Task CleanList()
    {
        var queue = new UniverQueue(univerJS, Ctx);
        queue.SetAction("getUserService").SetAction("clear");
        await queue.ResolveQueueAsync();
    }

    public async Task DeleteUser(string idUser)
    {
        var queue = new UniverQueue(univerJS, Ctx);
        queue.SetAction("getUserService").SetAction("delete", idUser);
        await queue.ResolveQueueAsync();
    }

    public async Task SetCurrentUser(UniverUser user)
    {
        var queue = new UniverQueue(univerJS, Ctx);
        queue.SetAction("getUserService").SetAction("setCurrentUser", user);
        await queue.ResolveQueueAsync();
    }

    private async Task Add(UniverUser user)
    {
        var queue = new UniverQueue(univerJS, Ctx);
        queue.SetAction("getUserService").SetAction("addUser", user);
        await queue.ResolveQueueAsync();
    }
}
