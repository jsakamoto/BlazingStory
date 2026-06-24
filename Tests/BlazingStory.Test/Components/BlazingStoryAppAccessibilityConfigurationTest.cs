using System.Reflection;
using BlazingStory.Addons;
using BlazingStory.Components;
using BlazingStory.Configurations;

namespace BlazingStory.Test.Components;

public class BlazingStoryAppAccessibilityConfigurationTest
{
    private const string AccessibilityAddonTypeName = "BlazingStory.Addons.BuiltIns.Panel.Accessibility.AccessibilityPanelAddon";

    [Test]
    public void AccessibilityReport_DisabledByDefault_NotRegistered()
    {
        var app = new BlazingStoryApp();
        var addonStore = CreateAddonStore();

        InvokePrivate(app, "ConfigureAddons", addonStore);
        InvokePrivate(app, "ConfigureConditionalAddons", addonStore);

        HasRegisteredAddon(addonStore, AccessibilityAddonTypeName).IsFalse();
    }

    [Test]
    public void AccessibilityReport_EnabledByParameter_Registered()
    {
        var app = new BlazingStoryApp
        {
#pragma warning disable BL0005
            EnableAccessibilityReport = true
#pragma warning restore BL0005
        };
        var addonStore = CreateAddonStore();

        InvokePrivate(app, "UpdateOptionsFromParameters");
        InvokePrivate(app, "ConfigureAddons", addonStore);
        InvokePrivate(app, "ConfigureConditionalAddons", addonStore);

        HasRegisteredAddon(addonStore, AccessibilityAddonTypeName).IsTrue();
    }

    [Test]
    public void AccessibilityReport_EnabledByOptions_Registered()
    {
        var app = new BlazingStoryApp();
        var addonStore = CreateAddonStore();

        var optionsField = typeof(BlazingStoryApp).GetField("_Options", BindingFlags.Instance | BindingFlags.NonPublic).IsNotNull();
        var options = optionsField.GetValue(app).IsInstanceOf<BlazingStoryOptions>();
        options.EnableAccessibilityReport = true;

        InvokePrivate(app, "ConfigureAddons", addonStore);
        InvokePrivate(app, "ConfigureConditionalAddons", addonStore);

        HasRegisteredAddon(addonStore, AccessibilityAddonTypeName).IsTrue();
    }

    [Test]
    public void ConfigureAddons_NullStore_Throws()
    {
        var app = new BlazingStoryApp();
        Action action = () => InvokePrivate(app, "ConfigureAddons", [null]);

        Assert.That(action, Throws.TypeOf<TargetInvocationException>().With.InnerException.TypeOf<ArgumentNullException>());
    }

    private static IAddonStore CreateAddonStore()
    {
        var addonStoreType = typeof(IAddonStore).Assembly.GetType("BlazingStory.Addons.Internals.AddonStore", throwOnError: true).IsNotNull();
        return Activator.CreateInstance(addonStoreType, nonPublic: true).IsInstanceOf<IAddonStore>();
    }

    private static bool HasRegisteredAddon(IAddonStore addonStore, string addonTypeName)
    {
        var getAddonsMethod = addonStore.GetType().GetMethod("GetAddons", BindingFlags.Instance | BindingFlags.NonPublic).IsNotNull();
        var addons = getAddonsMethod.Invoke(addonStore, null).IsInstanceOf<IEnumerable<IAddon>>();
        return addons.Any(addon => addon.GetType().FullName == addonTypeName);
    }

    private static object? InvokePrivate(object target, string methodName, params object?[]? args)
    {
        var method = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic).IsNotNull();
        return method.Invoke(target, args);
    }
}
