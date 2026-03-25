using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace BlazingStory.ToolKit.Utils;

/// <summary>
/// This class contains extension methods for the Razor Components.
/// </summary>
public static class ComponentExtensions
{
    private static readonly List<Namespace> PredefinedNamespaces =
    [
        // Predefined namespaces
        new Namespace("BlazingStory", ""),
        new Namespace("BlazingStory", "Components"),

        new Namespace("BlazingStory.ToolKit", "Buttons", true),
        new Namespace("BlazingStory.ToolKit", "Icons", true),
        new Namespace("BlazingStory.ToolKit", "Inputs", true),
        new Namespace("BlazingStory.ToolKit", "Menus", true),

        new Namespace("Microsoft.AspNetCore.Components", ""),
        new Namespace("Microsoft.AspNetCore.Components.Forms", ""),
        new Namespace("Microsoft.AspNetCore.Components", "Forms"),
    ];

    private static readonly List<Namespace> DynamicNamespaces = new List<Namespace>();

    // Method to add namespaces from Program.cs
    public static void RegisterNamespaces(IEnumerable<Namespace> namespaces)
    {
        foreach (var ns in namespaces)
        {
            if (!DynamicNamespaces.Contains(ns))
            {
                DynamicNamespaces.Add(ns);
            }
        }
    }

    [SuppressMessage("Trimming", "IL2057:Unrecognized value passed to the parameter of method. It's not possible to guarantee the availability of the target type.", Justification = "<Pending>")]
    internal static Type? FindComponentType(this string componentName)
    {
        var allNamespaces = PredefinedNamespaces.Concat(DynamicNamespaces);

        foreach (var ns in allNamespaces)
        {
            var returnType = FindComponentType(componentName, ns.ProjectName, ns.NamespaceWithoutProjectName, ns.ThisProject);

            if (returnType != null)
            {
                return returnType;
            }
        }

        var componentType = Type.GetType(componentName);

        return componentType;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    private static Type? FindComponentType(string componentName, string? projectName, string? namespaceWithoutProjectName, bool thisProject = false)
    {
        // Concatenate the component's name with the namespace
        var fullTypeName = string.Empty; // $"{projectName}.{namespaceWithoutProjectName}.{componentName}"

        if (!string.IsNullOrWhiteSpace(projectName))
        {
            fullTypeName = projectName;
        }

        if (!string.IsNullOrWhiteSpace(namespaceWithoutProjectName))
        {
            fullTypeName = fullTypeName + "." + namespaceWithoutProjectName;
        }

        if (!string.IsNullOrWhiteSpace(componentName))
        {
            fullTypeName = fullTypeName + "." + componentName;
        }

        var assembly = !thisProject && !string.IsNullOrWhiteSpace(projectName) ? Assembly.Load(projectName) : Assembly.GetExecutingAssembly();

        var returnType = assembly?.GetType(fullTypeName) ??
               assembly?.GetType($"{fullTypeName}`1[[{typeof(string).AssemblyQualifiedName}]]");

        return returnType;
    }
}

/// <summary>
/// The class that represents the full name of a namespace.
/// </summary>
public class Namespace
{
    public Namespace(string projectName, string namespaceWithoutProjectName, bool thisProject = false)
    {
        this.ProjectName = projectName;
        this.NamespaceWithoutProjectName = namespaceWithoutProjectName;
        this.ThisProject = thisProject;
    }

    public string? ProjectName { get; set; }

    public string? NamespaceWithoutProjectName { get; set; }

    public bool ThisProject { get; set; } = false;

    public override bool Equals(object? obj)
    {
        return obj is Namespace @namespace &&
               this.ProjectName == @namespace.ProjectName &&
               this.NamespaceWithoutProjectName == @namespace.NamespaceWithoutProjectName &&
               this.ThisProject == @namespace.ThisProject;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.ProjectName, this.NamespaceWithoutProjectName, this.ThisProject);
    }
}
