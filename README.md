# LUMOS
LUMOS Unified Modern Operating System

User Space:
- Filesystem Manager

Interface Space:
- UI Catalyst Runtime

Package Space:
- Package Manager

Security Space:
- Permission Manager
- Security Components (e.g., for user authentication, access control)

Driver Space:
- Device Drivers

Kernel Space:
- Linux Kernel

Applications: .los extension.


# UI Catalyst Engine

To ensure security while allowing flexibility in swapping the User Interface Layout System:

Define a Clear Interface Specification: Create a well-defined specification for the layout language. Include the types of elements allowed, properties, and interactions. This serves as a contract between the UI Catalyst Engine and the layout files.

Use a Domain-Specific Language (DSL): Implement a DSL for the layout system. This DSL should be restricted to layout descriptions, preventing the execution of arbitrary or harmful code.

Sandboxing: Run the layout parser in a sandbox environment. This restricts the parser's access to the system, preventing potential security breaches from maliciously crafted layout files.

Validation and Sanitization: Implement strict validation of the layout files. Ensure that all input conforms to the expected format and sanitize it to remove potential exploits.

```xml
<view id="musicPlayerView" background="musicPlayerBackgroundResource" actions="click">
  <menu id="navigationMenu" type="navigation" emphasis="high" actions="click">
    <content>
      <item value="Music"/>
      <item value="Videos"/>
      <item value="Podcasts"/>
      <item value="Radio"/>
      <item value="Marketplace"/>
    </content>
  </menu>
  <list id="songList" emphasis="low" actions="click">
    <content>
      <item pos="0,0">
        <value>Song Title 1</value>
        <button id="playSong1" emphasis="low" actions="click" value="►"/>
      </item>
      <item pos="1,0">
        <value>Song Title 2</value>
        <button id="playSong2" emphasis="low" actions="click" value="►"/>
      </item>
      <item pos="2,0">
        <value>Song Title 3</value>
        <button id="playSong3" emphasis="low" actions="click" value="►"/>
      </item>
    </content>
  </list>
</view>

<button id="submitBtn" emphasis="high" actions="click" value="Submit"/>
<label id="infoLabel" emphasis="low" actions="" value="Information"/>
<edittext id="usernameField" emphasis="high" actions="input" value="Enter Username"/>
<checkbox id="agreeCheck" emphasis="low" actions="change" value="true"/>
<radiobutton id="optionOne" emphasis="low" group="optionsGroup" actions="select" value="Option One"/>
<image id="logoImg" emphasis="high" actions="" value="logoImageResource"/>
<slider id="volumeControl" emphasis="low" actions="slide" value="50"/>
<switch id="toggleSwitch" emphasis="low" actions="toggle" value="false"/>
<dropdown id="countrySelect" emphasis="high" actions="select">
  <option value="USA"/>
  <option value="Canada"/>
  <option value="UK"/>
</dropdown>
<menu id="mainMenu" type="navigation" emphasis="high">
  <item value="Home" actions="select"/>
  <item value="About" actions="select"/>
  <item value="Contact" actions="select"/>
</menu>
<list id="itemList" emphasis="low" actions="select">
  <item value="List Item 1"/>
  <item value="List Item 2"/>
  <item value="List Item 3"/>
</list>
<grid id="layoutGrid" emphasis="high" actions="">
  <item pos="0,0" value="Grid Item 1"/>
  <item pos="1,0" value="Grid Item 2"/>
</grid>
```

Sandboxing

Linux Namespaces: These are used to provide isolated workspaces called "sandboxes." For instance, the mnt namespace can provide each app with its own filesystem view.
    - Control Groups (cgroups): Useful for resource management and limiting what system resources an app can use.
    - Chroot Jails: Change the root directory for a process, so it cannot see the rest of the filesystem.
    - Seccomp: A kernel feature that can be used to restrict the system calls that applications can make.
    - AppArmor/SELinux Profiles: Define what system resources an app can access.

Permissions System
    - User Space: The user interacts with the LUMOS middleware to set permissions for applications.
    - Middleware: This layer interprets those permissions and enforces them using the underlying security features.
    - Application Space: Each application declares what permissions it needs. LUMOS reviews these requests and grants permissions based on user settings.

Developer Mode
    - Secure Flag: A secure flag could be set in the bootloader that determines whether the device is in developer mode.
    - Permission Override: In developer mode, LUMOS can allow access to otherwise restricted resources, like the Linux terminal or system calls.

Security Considerations
    - No Direct Execution: To prevent apps from downloading and executing arbitrary Linux programs, the OS can forbid execution of any binaries not signed by LUMOS.
    - Filesystem Access: Apps only have write access to their designated space and read access to shared resources as defined by LUMOS policies.

System Implementation
    - For Rust or Julia, you would interface with the Linux kernel using their respective FFI (Foreign Function Interface) features.
    - You would also need to create a set of daemons or services running with higher privileges that manage app lifecycles, permissions, and inter-app communication.

```
+--------------------------------------------------+
|                     LUMOS OS                     |
| +----------------------------------------------+ |
| |              Application Space              | |
| | +--------+ +--------+ +--------+ +--------+ | |
| | | App 1  | | App 2  | | App 3  | | App N  | | |
| | +--------+ +--------+ +--------+ +--------+ | |
| | Each app runs in its own sandboxed          | |
| | environment with restricted permissions.    | |
| +----------------------------------------------+ |
| +----------------------------------------------+ |
| |             LUMOS Middleware                 | |
| | +------------+ +------------+ +------------+ | |
| | | Permission | | UI Catalyst| | App Launch | | |
| | | Manager    | | Engine     | | Manager    | | |
| | +------------+ +------------+ +------------+ | |
| | Manages permissions and inter-app           | |
| | communication based on user settings.       | |
| +----------------------------------------------+ |
| +----------------------------------------------+ |
| |               LUMOS Core Services            | |
| | +------------+ +------------+ +------------+ | |
| | | Filesystem | | Package    | | Security   | | |
| | | Manager    | | Manager    | | Components | | |
| | +------------+ +------------+ +------------+ | |
| | Provides essential OS-level services,       | |
| | isolated from user apps.                    | |
| +----------------------------------------------+ |
| +----------------------------------------------+ |
| |                  Driver Space                | |
| | +------------------------------------------+ | |
| | | Linux Kernel with LUMOS modifications   | | |
| | | and drivers for hardware interaction.    | | |
| | +------------------------------------------+ | |
+--------------------------------------------------+
```

# Apps
Reference applications by their AUID (Application Unique Identifier). Delegate all calls from UI Catalyst Engine (/ Runtime) to entry point Act(String id, String action). "id" would be control identifier, while "action" could describe "mouseDown", "onEnter", etc.
