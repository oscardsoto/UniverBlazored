using System.Threading.Tasks;
using UniverBlazored.Generic.Data;

namespace UniverBlazored.Generic.Services;

/// <summary>
/// User Manage, from Univer
/// </summary>
public class UniverUserManager
{
    private readonly IUniverJsInterop univerJS;

    /// <summary>
    /// User Manage, from Univer
    /// </summary>
    /// <param name="service">JS Interop service, for Univer</param>
    public UniverUserManager(IUniverJsInterop service) 
    {
        univerJS = service;
    }

    /// <summary>
    /// Returns the current user using Univer App
    /// </summary>
    /// <returns></returns>
    public async Task<UniverUser> GetCurrentUser() => await univerJS.SetAction("getUserManager").SetAction("getCurrentUser").ResolveAsync<UniverUser>();

    /// <summary>
    /// Returns a list with all users in the Univer App
    /// </summary>
    /// <returns></returns>
    public async Task<List<UniverUser>> ListAllUsers() => await univerJS.SetAction("getUserManager").SetAction("list").ResolveAsync<List<UniverUser>>();

    /// <summary>
    /// Returns the user info in the manager
    /// </summary>
    /// <param name="idUser">User Id</param>
    /// <returns></returns>
    public async Task<UniverUser> GetUser(string idUser) => await univerJS.SetAction("getUserService").SetAction("getUser", idUser).ResolveAsync<UniverUser>();

    /// <summary>
    /// Adds an user to the manager
    /// </summary>
    /// <param name="user">New user to add</param>
    /// <param name="setCurrent">True if this new user will be the current</param>
    /// <returns></returns>
    public async Task AddUser(UniverUser user, bool setCurrent = false)
    {
        await Add(user);
        if (setCurrent)
            await SetCurrentUser(user);
    }

    /// <summary>
    /// Adds a list of users to the manager
    /// </summary>
    /// <param name="users">Users to add in the list</param>
    /// <returns></returns>
    public async Task AddUser(params UniverUser[] users)
    {
        foreach(var user in users)
            await Add(user);
    }

    async Task Add(UniverUser user) => await univerJS.SetAction("getUserService").SetAction("addUser", user).ResolveAsync();

    /// <summary>
    /// Clear the User List
    /// </summary>
    /// <returns></returns>
    public async Task CleanList() => await univerJS.SetAction("getUserService").SetAction("clear").ResolveAsync();

    /// <summary>
    /// Deletes the selected user by id
    /// </summary>
    /// <param name="idUser">User Id in the list</param>
    public async Task DeleteUser(string idUser) => await univerJS.SetAction("getUserService").SetAction("delete", idUser).ResolveAsync();

    /// <summary>
    /// Sets the current user for the component
    /// </summary>
    /// <param name="user">User object in the list</param>
    public async Task SetCurrentUser(UniverUser user) => await univerJS.SetAction("getUserService").SetAction("setCurrentUser", user).ResolveAsync();
}