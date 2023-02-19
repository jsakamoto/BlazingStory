using BlazingStory.Internals.Models;

namespace BlazingStory.Test.Internals.Models;

internal class NavigationTreeItemTest
{
    [Test]
    public void IsExpandedAll_True_Test()
    {
        var item = new NavigationTreeItem
        {
            Expanded = true,
            SubItems = {
                new() {
                    Expanded = true,
                    SubItems = {
                        new() {
                            Expanded = true,
                            SubItems = {
                                new() { Expanded = false },
                                new() { Expanded = false }
                            }
                        }
                    }
                },
                new() {
                    Expanded = true,
                    SubItems = {
                        new() { Expanded = false }
                    }
                }
            }
        };

        item.IsExpandedAll.IsTrue();
    }

    [Test]
    public void IsExpandedAll_1stChildCollapsed_Test()
    {
        var item = new NavigationTreeItem
        {
            Expanded = true,
            SubItems = {
                new() {
                    Expanded = false, // the 1st child is collapsed.
                    SubItems = {
                        new() {
                            Expanded = true,
                            SubItems = {
                                new() { Expanded = false },
                                new() { Expanded = false }
                            }
                        }
                    }
                }
            }
        };

        item.IsExpandedAll.IsFalse();
    }

    [Test]
    public void IsExpandedAll_2ndChildCollapsed_Test()
    {
        var item = new NavigationTreeItem
        {
            Expanded = true,
            SubItems = {
                new() {
                    Expanded = true,
                    SubItems = {
                        new() {
                            Expanded = false, // the 2nd child is collapsed.
                            SubItems = {
                                new() { Expanded = false },
                                new() { Expanded = false }
                            }
                        }
                    }
                }
            }
        };

        item.IsExpandedAll.IsFalse();
    }

    [Test]
    public void ToggleSubItemsExpansion_from_AllExpanded_Test()
    {
        // Given
        var item = new NavigationTreeItem { Expanded = true };
        item.SubItems.AddRange(new NavigationTreeItem[] {
            new(){ Expanded = true },
            new(){ Expanded = true },
        });
        item.SubItems[0].SubItems.AddRange(new NavigationTreeItem[] {
            new(){ Expanded = true },
            new(){ Expanded = true },
        });
        item.SubItems[0].SubItems[0].SubItems.Add(new() { Expanded = false });
        item.SubItems[0].SubItems[1].SubItems.Add(new() { Expanded = false });
        item.SubItems[1].SubItems.AddRange(new NavigationTreeItem[] {
            new(){ Expanded = false },
            new(){ Expanded = false },
        });

        // When
        item.ToggleSubItemsExpansion();

        // Then
        item.Expanded.IsTrue();
        item.SubItems[0].Expanded.IsFalse();
        item.SubItems[0].SubItems[0].Expanded.IsFalse();
        item.SubItems[0].SubItems[0].SubItems[0].Expanded.IsFalse();
        item.SubItems[1].Expanded.IsFalse();
        item.SubItems[1].SubItems[0].Expanded.IsFalse();
        item.SubItems[1].SubItems[1].Expanded.IsFalse();

        // When: once again
        item.ToggleSubItemsExpansion();

        // Then
        item.Expanded.IsTrue();
        item.SubItems[0].Expanded.IsTrue();
        item.SubItems[0].SubItems[0].Expanded.IsTrue();
        item.SubItems[0].SubItems[0].SubItems[0].Expanded.IsTrue();
        item.SubItems[1].Expanded.IsTrue();
        item.SubItems[1].SubItems[0].Expanded.IsTrue();
        item.SubItems[1].SubItems[1].Expanded.IsTrue();
    }

    [Test]
    public void ToggleAllExpansion_from_1stLevelChildCollapsed_Test()
    {
        // Given
        var item = new NavigationTreeItem { Expanded = true };
        item.SubItems.AddRange(new NavigationTreeItem[] {
            new(){ Expanded = true },
            new(){ Expanded = false }, // the 1st level child is collapsed.
        });
        item.SubItems[0].SubItems.AddRange(new NavigationTreeItem[] {
            new(){ Expanded = true },
            new(){ Expanded = true },
        });
        item.SubItems[0].SubItems[0].SubItems.Add(new() { Expanded = false });
        item.SubItems[0].SubItems[1].SubItems.Add(new() { Expanded = false });
        item.SubItems[1].SubItems.AddRange(new NavigationTreeItem[] {
            new(){ Expanded = false },
            new(){ Expanded = false },
        });

        // When
        item.ToggleSubItemsExpansion();

        // Then
        item.Expanded.IsTrue();
        item.SubItems[0].Expanded.IsTrue();
        item.SubItems[0].SubItems[0].Expanded.IsTrue();
        item.SubItems[0].SubItems[0].SubItems[0].Expanded.IsTrue();
        item.SubItems[1].Expanded.IsTrue();
        item.SubItems[1].SubItems[0].Expanded.IsTrue();
        item.SubItems[1].SubItems[1].Expanded.IsTrue();

        // When: once again
        item.ToggleSubItemsExpansion();

        // Then
        item.Expanded.IsTrue();
        item.SubItems[0].Expanded.IsFalse();
        item.SubItems[0].SubItems[0].Expanded.IsFalse();
        item.SubItems[0].SubItems[0].SubItems[0].Expanded.IsFalse();
        item.SubItems[1].Expanded.IsFalse();
        item.SubItems[1].SubItems[0].Expanded.IsFalse();
        item.SubItems[1].SubItems[1].Expanded.IsFalse();
    }

    [Test]
    public void ToggleAllExpansion_from_2ndLevelChildCollapsed_Test()
    {
        // Given
        var item = new NavigationTreeItem { Expanded = true };
        item.SubItems.AddRange(new NavigationTreeItem[] {
            new(){ Expanded = true },
            new(){ Expanded = true },
        });
        item.SubItems[0].SubItems.AddRange(new NavigationTreeItem[] {
            new(){ Expanded = false }, // the 2nd level child is collapsed.
            new(){ Expanded = true },
        });
        item.SubItems[0].SubItems[0].SubItems.Add(new() { Expanded = false });
        item.SubItems[0].SubItems[1].SubItems.Add(new() { Expanded = false });
        item.SubItems[1].SubItems.AddRange(new NavigationTreeItem[] {
            new(){ Expanded = false },
            new(){ Expanded = false },
        });

        // When
        item.ToggleSubItemsExpansion();

        // Then
        item.Expanded.IsTrue();
        item.SubItems[0].Expanded.IsTrue();
        item.SubItems[0].SubItems[0].Expanded.IsTrue();
        item.SubItems[0].SubItems[0].SubItems[0].Expanded.IsTrue();
        item.SubItems[1].Expanded.IsTrue();
        item.SubItems[1].SubItems[0].Expanded.IsTrue();
        item.SubItems[1].SubItems[1].Expanded.IsTrue();

        // When: once again
        item.ToggleSubItemsExpansion();

        // Then
        item.Expanded.IsTrue();
        item.SubItems[0].Expanded.IsFalse();
        item.SubItems[0].SubItems[0].Expanded.IsFalse();
        item.SubItems[0].SubItems[0].SubItems[0].Expanded.IsFalse();
        item.SubItems[1].Expanded.IsFalse();
        item.SubItems[1].SubItems[0].Expanded.IsFalse();
        item.SubItems[1].SubItems[1].Expanded.IsFalse();
    }

    [Test]
    public void FindParentOf_Test()
    {
        // Given
        var root = new NavigationTreeItem();
        var headingA = new NavigationTreeItem { Caption = "Heading A" };
        var componentA = new NavigationTreeItem { Caption = "Component A" };
        var storyA1 = new NavigationTreeItem { Caption = "Story A-1" };
        var storyA2 = new NavigationTreeItem { Caption = "Story A-2" };
        var headingB = new NavigationTreeItem { Caption = "Heading B" };
        var componentB = new NavigationTreeItem { Caption = "Component B" };
        var storyB1 = new NavigationTreeItem { Caption = "Story B-1" };
        var storyB2 = new NavigationTreeItem { Caption = "Story B-2" };
        root.SubItems.AddRange(new[] { headingA, headingB });
        headingA.SubItems.Add(componentA);
        componentA.SubItems.AddRange(new[] { storyA1, storyA2 });
        headingB.SubItems.Add(componentB);
        componentB.SubItems.AddRange(new[] { storyB1, storyB2 });

        // When
        var parentOfComponentA = root.FindParentOf(componentA);
        var parentOfStoryA1 = root.FindParentOf(storyA1);
        var parentOfStoryB2 = root.FindParentOf(storyB2);

        // Then
        parentOfComponentA.IsNotNull().Caption.Is("Heading A");
        parentOfStoryA1.IsNotNull().Caption.Is("Component A");
        parentOfStoryB2.IsNotNull().Caption.Is("Component B");
    }
}
