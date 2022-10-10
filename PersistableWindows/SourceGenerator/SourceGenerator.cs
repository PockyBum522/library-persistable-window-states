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
        public string WindowGuid => " + attributeWindowGuid + @";
        public double Top { get; set; }
        public double Left { get; set; }
        public double Height { get; set; }
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