@echo off
cd /d %~dp0
.\.bin\nuget.exe pack BlazingStory.ProjectTemplates.nuspec -OutputDirectory ..\_dist