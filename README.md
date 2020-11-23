This branch is a try to create a toolkit to ease Matrix effects creation.

The new tool is called LedControl Toolkit.
Its purpose is to edit effects without needing to have addressable ledstrip hardware connected.

For doing so, it uses the current DirectOutput.dll (slighly modified but nothing should change in its behavior).
It then redirect outputs to a new Winforms based Led Controller to preview the effects.
It's using your usual config files (globalconfig, cabinet.xml & led control inifiles) to setup your toolkit workspace.

For now, you can only browse through all available table configs from provided inifile, test the effects and modify them.
Effect edition is done through a property grid which expose a Table Config Setting line for each effect.
The properties are automatically converted to dofconfigtool compatible commands you can then inject in the online tool.

Next step will be to create brand new effects which could be save/load using xml files.

Standalone releases are available in the releases section, they're provided with their own DirectOutput.dll so you can extract them in a separate directory than your DirectOutput installation. 
If you want to test this tool at its early stage, it'll be really appreciate.
Nothing is saved in any of your congif files, only tool settings are in a config subdirectory.
And also feel free to create issues on this branch for any feedback, bug or improvement.

Cheers
