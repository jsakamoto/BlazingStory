namespace BlazingStory.Internals.Utils;

internal delegate ValueTask ValueTaskCallback();

internal delegate ValueTask ValueTaskCallback<T>(T sender);
