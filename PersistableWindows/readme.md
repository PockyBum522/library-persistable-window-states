To Use:
Reference PersistableWindows from your project.

Go into that project file and add OutputItemType="Analyzer" to the ProjectReference tag

You should end up with something like this:
```angular2html
<ItemGroup>
    <ProjectReference Include="..\..\PersistableWindows\PersistableWindows.csproj" OutputItemType="Analyzer" />
</ItemGroup>
```
