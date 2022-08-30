#pragma warning disable CS1591 
// Because standards say we don't have to XML comment public properties if their meaning is obvious

namespace PersistableWindows;

/// <summary>
/// Used for storing window size/position between application sessions
///
/// GUID is unique per window
/// </summary>
public interface IPersistableWindow
{
    /// <summary>
    /// Each windows must assign its own GUID here
    /// </summary>
    string WindowGuid { get; }
        
    double Top { get; set; }
    double Left { get; set; }
    double Height { get; set; }
    double Width { get; set; }
}