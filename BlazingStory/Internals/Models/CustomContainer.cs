﻿using System.Diagnostics.CodeAnalysis;
using BlazingStory.Internals.Services.XmlDocComment;
using BlazingStory.Types;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Models;

/// <summary>
/// Represents a "custom component", container for custom pages.
/// </summary>
internal class CustomContainer
{
    internal readonly string Title;

    /// <summary>
    /// Gets a navigation path string for this custom page (component).<br/>
    /// (ex. "documentation-guides-setup")
    /// </summary>
    internal readonly string NavigationPath;

    /// <summary>
    /// The type of the layout component to use when displaying these custom pages.
    /// </summary>
    [DynamicallyAccessedMembers(All)]
    internal readonly Type? Layout;

    private readonly CustomRazorDescriptor _CustomRazorDescriptor;

    internal readonly RenderFragment RenderFragment;

    /// <summary>
    /// Initialize a new instance of <see cref="CustomContainer"/>.
    /// </summary>
    /// <param name="layout">A type of the layout component to use when displaying these stories.</param>
    /// <param name="customRazorDescriptor">A descriptor of a type of Custom Razor component (..custom.razor) and its <see cref="CustomAttribute"/>.</param>
    public CustomContainer([DynamicallyAccessedMembers(All)] Type? layout, CustomRazorDescriptor customRazorDescriptor, IServiceProvider services, RenderFragment renderFragment)
    {
        this._CustomRazorDescriptor = customRazorDescriptor ?? throw new ArgumentNullException(nameof(customRazorDescriptor));
        this.Layout = layout;
        this.Title = this._CustomRazorDescriptor.CustomAttribute.Title ?? throw new ArgumentNullException(nameof(customRazorDescriptor)); ;
        this.NavigationPath = Services.Navigation.NavigationPath.Create(this.Title);
        this.RenderFragment = renderFragment;
    }

}