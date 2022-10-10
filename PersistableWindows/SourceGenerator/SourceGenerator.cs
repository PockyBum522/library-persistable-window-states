using JetBrains.Annotations;
using Microsoft.CodeAnalysis;

namespace PersistableWindows.SourceGenerator;

[PublicAPI, Generator]
public class PersistableWindowsSourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is null) throw new ArgumentNullException(nameof(context.SyntaxReceiver));
        
        var mainSyntaxReceiver = (MainSyntaxReceiver)context.SyntaxReceiver;
        var outputCode = BuildOutputCode(mainSyntaxReceiver);
        
        context.AddSource("persistableWindows.g.cs", outputCode);
    }

    private string BuildOutputCode(MainSyntaxReceiver mainSyntaxReceiver)
    {
        var outputCode = "using PersistableWindows;";

        foreach (var capture in mainSyntaxReceiver.Captured)
        { 
            var attributeFirstArgument = capture.FoundAttribute.ArgumentList?.Arguments.First();
            
            var attributeWindowGuid = attributeFirstArgument?.ToFullString();

            var namespaceFullName = capture.FoundNamespace.Name.ToFullString();
            
            outputCode += @"

namespace " + namespaceFullName + @"
{
    public partial class " + capture.FoundClass.Identifier.Text + @" : IPersistableWindow
    {
        /// <summary>
        /// The window GUID, must be unique to each individual window
        /// </summary>
        public string WindowGuid => " + attributeWindowGuid + @";

        /// <summary>
        /// The window distance from top of screen 0, so that we can bind to it
        /// </summary>
        public double Top { get; set; }
        
        /// <summary>
        /// The window distance from left side of screen 0, so that we can bind to it
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// The window height, so that we can bind to it
        /// </summary>
        public double Height { get; set; }
        
        /// <summary>
        /// The window width, so that we can bind to it
        /// </summary>
        public double Width { get; set; }

        private void SaveWindowState()
        {
            PersistableWindowManager.SaveWindowSizeAndLocation(this);
        }

        private void RestoreWindowState()
        {
            PersistableWindowManager.RestoreWindowSizeAndLocation(this);
        }
    }
}
       
";
        }

        Console.WriteLine(outputCode);

        return outputCode;
    }

    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUG
        //if (!Debugger.IsAttached) { Debugger.Launch(); }
#endif 
        
        context.RegisterForSyntaxNotifications(() => new MainSyntaxReceiver());
    }
}