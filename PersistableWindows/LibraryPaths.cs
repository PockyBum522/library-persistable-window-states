namespace PersistableWindows;

internal static class LibraryPaths
{
    /// <summary>
    /// Generates the path for the folder in AppData
    /// </summary>
    private static string PathAppDataBaseLocation =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "Persistable Window State Manager");
    
    /// <summary>
    /// Generates the full path for app window size/location data storage
    /// </summary>
    internal static string PathPersistableWindowDataLocation => 
        Path.Combine(PathAppDataBaseLocation, "Window States");
}