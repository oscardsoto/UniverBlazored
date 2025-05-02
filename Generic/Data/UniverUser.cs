using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace UniverBlazored.Generic.Data;

/// <summary>
/// Univer User's data from the UserManager
/// </summary>
public struct UniverUser
{
    /// <summary>
    /// User id
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// User's name
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// Image of the user (in Data Uri format)
    /// </summary>
    public string avatar { get; set; }

    /// <summary>
    /// User's type
    /// </summary>
    UniverUserType Type { get; set; }

    /// <summary>
    /// Get The user's type
    /// </summary>
    /// <returns></returns>
    public UniverUserType GetUserType()
    {
        string[] types = Enum.GetNames(typeof(UniverUserType));
        foreach (string type in types)
            if (id.Contains(type))
                return Enum.Parse<UniverUserType>(id.Split('_')[0]);
        return UniverUserType.UNRECOGNIZED;
    }

    /// <summary>
    /// Set the user type
    /// </summary>
    /// <param name="type">New type for the User ID</param>
    public void SetUserType(UniverUserType type)
    {
        string idValue = id.Split('_')[1];
        Type = type;
        id = $"{type}_{idValue}";
    }
}

/// <summary>
/// User type, used in the UserManager
/// </summary>
public enum UniverUserType
{
    /// <summary>
    /// User not recognized
    /// </summary>
    UNRECOGNIZED,

    /// <summary>
    /// User Owner
    /// </summary>
    Owner,

    /// <summary>
    /// Only edit
    /// </summary>
    Editor,

    /// <summary>
    /// Only read
    /// </summary>
    Reader
}