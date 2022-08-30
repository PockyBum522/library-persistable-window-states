using System.Collections.Specialized;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PersistableWindows.SourceGenerator;

public class MainSyntaxReceiver : ISyntaxReceiver
{
    public List<(
        AttributeSyntax FoundAttribute, 
        ClassDeclarationSyntax FoundClass, 
        FileScopedNamespaceDeclarationSyntax FoundNamespace
        )> Captured { get; set; } = new();
    
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (SyntaxNodeIsNotCorrectAttribute(syntaxNode)) return;

        var attributeSyntaxNode = (AttributeSyntax)syntaxNode;
        
        // If we got this far, we found one of the right attributes:
        var attachedClass = GetAttachedClass(attributeSyntaxNode);

        if (attachedClass is null) return;

        var attachedNamespace = GetAttachedNamespace(attributeSyntaxNode);
        
        if (attachedNamespace is null) return;
        
        // Otherwise, we now have a class that had the attribute
        // Add the attribute and that to the list
        Captured.Add( (attributeSyntaxNode, attachedClass, attachedNamespace) );
    }

    private static ClassDeclarationSyntax? GetAttachedClass(AttributeSyntax convertedSyntaxNode)
    {
        var attributeList = convertedSyntaxNode.Parent;

        if (attributeList is null) return null;
        if (attributeList.Parent is null) return null;
        if (attributeList.Parent is not ClassDeclarationSyntax) return null;

        // We now have the class that the attribute is attached to
        return (ClassDeclarationSyntax)attributeList.Parent;
    }

    private static bool SyntaxNodeIsNotCorrectAttribute(SyntaxNode syntaxNode)
    {
        // This is all just checks and casts once we know it's safe to cast
        if (syntaxNode is not AttributeSyntax) return true;

        var convertedSyntaxNode = (AttributeSyntax)syntaxNode;

        if (convertedSyntaxNode.Name is not IdentifierNameSyntax) return true;

        var syntaxNodeName = (IdentifierNameSyntax)convertedSyntaxNode.Name;

        if (syntaxNodeName.Identifier.Text != "PersistableWindow") return true;
        
        return false;
    }
    
    private FileScopedNamespaceDeclarationSyntax? GetAttachedNamespace(AttributeSyntax attributeSyntaxNode)
    {
        var namespaceNode = attributeSyntaxNode.Parent?.Parent?.Parent;

        if (namespaceNode is null) return null;
        
        var convertedNamespace = (FileScopedNamespaceDeclarationSyntax)namespaceNode;

        return convertedNamespace;
    }
}