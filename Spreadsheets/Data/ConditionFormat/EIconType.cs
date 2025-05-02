namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// IIconType, from univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/models/icon-map.ts) 
/// (Every enum starts with "I_", but will be replaced with empty on declaration)
/// </summary>
public enum EIconType
{
    ///<summary>
    /// Represents three-arrow formatting icon.
    ///</summary>
    I_3Arrows,

    ///<summary>
    /// Represents a gray three-arrow formatting icon.
    ///</summary>
    I_3ArrowsGray,

    ///<summary>
    /// Represents four-arrow formatting icon.
    ///</summary>
    I_4Arrows,

    ///<summary>
    /// Represents a gray four-arrow formatting icon.
    ///</summary>
    I_4ArrowsGray,

    ///<summary>
    /// Represents five-arrow formatting icon.
    ///</summary>
    I_5Arrows,

    ///<summary>
    /// Represents a gray five-arrow formatting icon.
    ///</summary>
    I_5ArrowsGray,

    ///<summary>
    /// Represents a gray three-triangle formatting icon.
    ///</summary>
    I_3Triangles,

    ///<summary>
    /// Represents a red to black traffic light symbol.
    ///</summary>
    I_4RedToBlack,

    ///<summary>
    /// Represents a traffic light symbol with one red and two yellow lights.
    ///</summary>
    I_3TrafficLights1,

    ///<summary>
    /// Represents a traffic light symbol with two red lights and one green light.
    ///</summary>
    I_3TrafficLights2,

    ///<summary>
    /// Represents three sign symbols formatting icon (i.e., signs or marks).
    ///</summary>
    I_3Signs,

    ///<summary>
    /// Represents five shading or felling symbols formatting icon.
    ///</summary>
    I__5Felling, 
    
    ///<summary>
    /// Represents four traffic lights symbol.
    ///</summary>
    I_4TrafficLights,

    ///<summary>
    /// Represents three symbols formatting icon.
    ///</summary>
    I_3Symbols,

    ///<summary>
    /// Represents another three symbols formatting icon.
    ///</summary>
    I_3Symbols2,

    ///<summary>
    /// Represents a flag symbol.
    ///</summary>
    I_3Flags,

    ///<summary>
    /// Represents four rating symbols formatting icon.
    ///</summary>
    I_4Rating,

    ///<summary>
    /// Represents five rating symbols formatting icon.
    ///</summary>
    I_5Rating,

    ///<summary>
    /// Represents a five-quarter symbol (i.e., quarter marks).
    ///</summary>
    I_5Quarters,

    ///<summary>
    /// Represents five felling or shading symbols formatting icon.
    ///</summary>
    I_5Felling,

    ///<summary>
    /// Represents a box symbol.
    ///</summary>
    I_5Boxes,

    ///<summary>
    /// Represents three stars formatting icon.
    ///</summary>
    I_3Stars
}