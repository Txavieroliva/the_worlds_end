Important notes:
This tool doesn't work on versions older than Unity 2022 due to a bug in UITK.
After importing a package it is required to restart your editor, for it to properly register Script Templates.
Better not move any folders around, Unity is sensitive for this kind of stuff.

Usage:
Create a controller asset by clicking Create>FSMC>Controller.
Open the editor, add parameters, create states and transitions by opening context menu with right click.
Create a script for your custom behaviour by opening template in Create>FSMC>Behaviour. After that you can add it to your states.
Add FSMC_Executer component to your game object and attach your controller asset to it.
You are ready to go! Manipulate your parameters using getters and setters just like you would do with an Animator.