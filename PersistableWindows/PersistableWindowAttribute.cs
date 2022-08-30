namespace PersistableWindows;

[AttributeUsage(AttributeTargets.Class)]  
public class PersistableWindowAttribute : Attribute
{
    public string WindowGuid { get; }

    public PersistableWindowAttribute(string windowGuidString)
    {
        WindowGuid = windowGuidString;
    }
}