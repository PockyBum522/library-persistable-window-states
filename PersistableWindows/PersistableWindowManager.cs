using System.Xml.Serialization;
using JetBrains.Annotations;

namespace PersistableWindows;

/// <summary>
/// Allows to easily save and restore window size and position for any window that implements IPersistableWindow
/// </summary>
[PublicAPI]
public class PersistableWindowManager
{
    /// <summary>
    /// Call in the closing event of the window or ViewModel to handle saving window size and position
    /// In WPF this usually means using Interaction.Triggers in Microsoft.Xaml.Behaviors.Wpf (Nuget package)
    ///
    /// You will need to put something like this at the top of your Window XAML:
    /// 
    /// b:Interaction.Triggers
    ///     b:EventTrigger EventName="Closing"
    ///         b:InvokeCommandAction Command="{ Binding CommandOnClosing }" PassEventArgsToCommand="True" 
    ///
    /// AND you will need to bind Height, Top, etc if using a ViewModel
    /// 
    /// </summary>
    /// <param name="windowToSaveDataFor">Pass Window or ViewModel implementing IPersistableWindow as 'this'</param>
    public static void SaveWindowSizeAndLocation(IPersistableWindow windowToSaveDataFor)
    {
        var windowGuid = windowToSaveDataFor.WindowGuid;

        var stateFilePath = 
            Path.Join(
                LibraryPaths.PathPersistableWindowDataLocation, 
                $"{windowGuid}.lastState");

        var windowStateToSave = new WindowState
        {
            LocationTop = windowToSaveDataFor.Top,
            LocationLeft = windowToSaveDataFor.Left,

            SizeWidth = windowToSaveDataFor.Width,
            SizeHeight = windowToSaveDataFor.Height
        };
        
        WriteXmlSerialized(stateFilePath, windowStateToSave);
    }

    /// <summary>
    /// Call this in the constructor for the Window that implements IPersistableWindow
    /// </summary>
    /// <param name="windowToSaveDataFor">Pass Window implementing IPersistableWindow as 'this'</param>
    public static void RestoreWindowSizeAndLocation(IPersistableWindow windowToSaveDataFor)
    {
        var windowGuid = windowToSaveDataFor.WindowGuid;

        var stateFilePath = 
            Path.Join(
                LibraryPaths.PathPersistableWindowDataLocation, 
                $"{windowGuid}.lastState");
        
        var windowLastSavedState = DeserializeLastStateFile(stateFilePath);
        
        windowToSaveDataFor.Top = windowLastSavedState.LocationTop;
        windowToSaveDataFor.Left = windowLastSavedState.LocationLeft;
        windowToSaveDataFor.Height = windowLastSavedState.SizeHeight;
        windowToSaveDataFor.Width = windowLastSavedState.SizeWidth;
    }

    private static WindowState DeserializeLastStateFile(string stateFilePath)
    {
        var returnState = new WindowState();
        
        if (File.Exists(stateFilePath))
        {
            var serializer = new XmlSerializer(typeof(WindowState));

            // Create a TextReader to read the file.
            var fs = new FileStream(stateFilePath, FileMode.OpenOrCreate);
            var reader = new StreamReader(fs);

            var rawState = serializer.Deserialize(reader);

            if (rawState is null)
            {
                return new WindowState();
            }
            
            // Otherwise if it's not null:
            returnState = (WindowState)rawState;

            reader.Close();
        }
        
        returnState = EnsureSafeValues(returnState);
        
        return returnState;
    }

    private static void WriteXmlSerialized(string stateFilePath, WindowState windowStateToSave)
    {
        Directory.CreateDirectory(LibraryPaths.PathPersistableWindowDataLocation);
        
        var xmlSerializer = new XmlSerializer(typeof(WindowState));
        TextWriter writer = new StreamWriter(stateFilePath);
        xmlSerializer.Serialize(writer, windowStateToSave);
        
        writer.Close();
    }

    private static WindowState EnsureSafeValues(WindowState returnState)
    {
        if (returnState.LocationLeft is < -1920 or > 4050 ||
            returnState.LocationTop is < -1050 or > 1950 ||
            returnState.SizeWidth < 200 ||
            returnState.SizeHeight < 100)
        {
            return new WindowState();
        } 
        
        // Otherwise if everything is valid:
        return returnState;
    }
}

/// <summary>
/// Model for storing the Window size and position
/// </summary>
[Serializable]
public class WindowState
{
    /// <summary>
    /// Store Window X position
    /// </summary>
    public double LocationLeft = 100;
    /// <summary>
    /// Stored Window Y position
    /// </summary>
    public double LocationTop = 100;
        
    /// <summary>
    /// Stored Window size, width
    /// </summary>
    public double SizeWidth = 1400;
    /// <summary>
    /// Stored Window size, height 
    /// </summary>
    public double SizeHeight = 900;
        
}