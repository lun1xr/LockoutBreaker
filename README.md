## App Docs

I made this app mainly as a passion project/to learn how to use C# and the WPF framework after dealing with mistimed or completely erroneous Windows Parental Controls enforcement for a good portion of my childhood. Here's a list of the main functions and how they work:

* Launch settings: Pretty self explanatory, launch on startup adds a link to the startup folder, launch minimized tells the program to refrain from showing the main window on launch.

* Main function: The main purpose of this app is to package and automate the several steps involved in disabling parental controls on a Windows PC. This can be done either automatically (by checking if the process linked to Parental Controls is open every few seconds and if it is, disabling it) or manually (by linking the disable process to a keybind, which will work even from the *Time's Up*/*Time for a Break* lockscreen). This disabling process really only needs to be run a single time to effectively remove the issue, but Windows update sometimes, in checking Windows for issues, will repair the altered files and restart Parental Controls. This can become especially irritating if it occurs during a lockout period, and is why the launch on startup and autorun options are included. The app is super lightweight, so it really shouldn't have any startup impact if you just want to leave it running in the background. ***Please note that the app will not function properly from the lockscreen if you do not accept the UAC prompt.***

* Extra Features: Figured I'd add some extra things for fun. These include a keybind activated mock *Time for a Break* lockscreen and a safe mode option, which will change the icon of the process in the taskbar to the WpcMon icon. The *Time for Break* lockscreen is at the moment nothing more than a display window with click responsive buttons, but I'm considering making it fully functional in the future. Open the extra features Window by clicking on the logo in the app.

* Perms Wizard: The app needs elevated permissions to run properly, so I added an interactive wizard that explains how to give your account the proper administrative priveleges from the Windows Recovery Environment, and makes the process as painless as possible (explaining WRE, writing a batch file with the proper commands, and instructing the user how to run it). It runs if the app detects insufficient priveleges or if you ignore the initial UAC prompt.

I will in the future be making the app look nicer and I'm open to suggestions on functionality.
