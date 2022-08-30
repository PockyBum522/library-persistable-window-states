# How to Use:

On your ViewModel, add the attribute:

```[PersistableWindow("11111111-2222-3333-4444-555555555555")]```

You must generate a new GUID for each ViewModel or Window

Then, make the viewmodel's class partial.

After that, simply save or build to allow the source generator to work its magic, and then add SaveWindowState(); where you would like to save window state in the viewmodel, such as on closing.

Add RestoreWindowState(); in the viewmodel where you would like to restore the saved state, such as in the constructor.

# Reference:

```
   <ItemGroup>    
        <ProjectReference Include="..\..\PersistableWindowStateManager\PersistableWindows\PersistableWindows.csproj" OutputItemType="Analyzer"  />
    </ItemGroup>    
```

Your reference must look like this in the project file of the project that is referencing this. Mostly, make sure OutputItemType="Analyzer" is present.

# XAML:

Make sure in your window you bind to the appropriate properties. 

```
<Window x:Class="MyNameSpace.MyWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:b="http://schemas.microsoft.com/xaml/behaviors"
        mc:Ignorable="d"
        Title="MyWindow" 
        d:DesignWidth="1000" 
        d:DesignHeight="700"
        Top="{ Binding Top, Mode=TwoWay }"
        Left="{ Binding Left, Mode=TwoWay }"
        Width="{ Binding Width, Mode=TwoWay }"
        Height="{ Binding Height, Mode=TwoWay }" >

```

Just ensure the bottom four lines in that example are bound as shown and that you've set your viewmodel as the datacontext.