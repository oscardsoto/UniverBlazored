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
    public string userID { get; set; }

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
    /// Sets the user's id with the user type
    /// </summary>
    /// <param name="idValue">Id's value</param>
    public void SetUserId(string idValue)
    {
        if (Type is UniverUserType.UNRECOGNIZED)
            Type = UniverUserType.Owner;

        userID = $"{Type}_{idValue}";
    }

    /// <summary>
    /// Get The user's type
    /// </summary>
    /// <returns></returns>
    public UniverUserType GetUserType()
    {
        string[] types = Enum.GetNames(typeof(UniverUserType));
        foreach (string type in types)
            if (userID.Contains(type))
                return Enum.Parse<UniverUserType>(userID.Split('_')[0]);
        return UniverUserType.UNRECOGNIZED;
    }

    /// <summary>
    /// Set the user type
    /// </summary>
    /// <param name="type">New type for the User ID</param>
    public void SetUserType(UniverUserType type)
    {
        Type = type;
        if (string.IsNullOrEmpty(userID))
        {
            userID = $"{type}_";
        }

        string idValue = userID.Split('_')[1];
        userID = $"{type}_{idValue}";
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