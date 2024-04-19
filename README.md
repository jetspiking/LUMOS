# <img src="https://raw.githubusercontent.com/jetspiking/LUMOS/main/Readme/Icon.png" width="64" height="64"> LUMOS Unified Mock Operating System
![](https://github.com/jetspiking/LUMOS/blob/main/Readme/Boot.gif)

# Description
LUMOS is a mock operating system that can be utilized to limit a system to its purpose(s) to enhance the end-user experience. LUMOS can be deployed as a supplement for systems heavily relying on users opening or using specific tools on a Windows, Linux or MacOS machine.

<img src="https://raw.githubusercontent.com/jetspiking/LUMOS/main/Readme/SoftKeyboardPortrait.png">

# Overview
<img src="https://raw.githubusercontent.com/jetspiking/LUMOS/main/Readme/VirtualBox.png">
<img src="https://raw.githubusercontent.com/jetspiking/LUMOS/main/Readme/SoftKeyboardLandscape.png">

# Installation
Navigate to the releases tab, i.e. 
[Releases](https://github.com/jetspiking/LUMOS/releases)

From here you can download Roblocks for ```Mac OS```, ```Windows```, ```Linux```.
Launch the executable to display LUMOS, it should work out of the box. 

If you are using ```Mac OS``` it might be reported that opening the application is not possible, due to not having the permissions or the binary would be corrupt. This can be resolved by opening terminal and executing the following command: 

```xattr -d com.apple.quarantine /path/to/LUMOS```

The Linux application be started by performing:

```chmod +x LUMOS-lin-x64```
```./LUMOS-lin-x64```

# Arguments
The application supports two alternative run modes by appending an argument for the binary.
- ```(no arguments)```: Run inside a window (default)
- ```--drm```: Direct Rendering Manager (display directly into framebuffer)
- ```--fsl```: Lock the application in maximized fullscreen (immersive for when running through framebuffer is not available or suitable)

Especially for the ```--drm``` and ```--fsl``` modes it seems appropiate to automatically start the application on after the operating system has booted. This introduced a new edge-case where exiting the application upon maintenance or checks could prove problematic. As an alternative exit mode a terminal command ```LUMOS_EXIT``` was added. This will shutdown the application regardless of the launch argument.

# Content
Two example applications are included. A boot animation is added as app to display how the API can be used to continuously write updates for displaying the application. The second application is a terminal program that is a suitable example for showing lifecycle management related to opening, hiding or closing the application.

# Architecture
Originally the idea was to create a microservices architecture communicating via IPC (Named Pipes) for higher maintainability. However, this introduced multiple (OS-specific) challenges that, after careful consideration, would greatly improve complexity. The solution builds to a single process that needs to run and is responsible for everything (divided into multiple domains) and the overall source code quantity is limited. As a tradeoff, this means that building or expanding the current solution will require to (re)compile and (re)deploy after each source code update.

Information regarding the architecture and especially relevant information for expanding, adjusting or maintaining the solution will be added here.

# Documentation
A requirement for applying LUMOS is that the workflow can be automated by an API or DLL. Ideally this API would be in C#. However, by creativily calling your API or (CLI) tools in the LUMOS C#-backend you can integrate any back-end in the launcher. The front-end of LUMOS is build upon Avalonia UI and suitable for most applications limitedly relying on performance.

Documentation will be added here on how to register your application inside the "launcher". The launcher is used to start your custom application(s).

# Thank you for using LUMOS
If you enjoy this software series, you could consider supporting me by purchasing application [Colorpick - PRO](https://store.steampowered.com/app/1388790/Colorpick__PRO). For a few dollars (depending on Steam pricing in region) you receive a DRM-free Colorpick application.

<a href="https://www.buymeacoffee.com/DustinHendriks" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174"></a>
