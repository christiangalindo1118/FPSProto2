Military Bunker Construction Pack

Thank you for purchasing the asset!


Render settings
===============
Since there is no 'exterior', make sure to use a Solid black color clear flag for the rendering camera to avoid artifacts.

Performance and lighting
========================
The scene is lit by point/spot lights (or area lights) and therefore, close attention must be paid when setting up lights/shadows and configuring lightmap settings.
The most important of all is to bake occlusion data to get the fastest performance possible. (Demo scene doesn't have that, so bake it before trying it)
Demo scene draw calls don't exceed 580 in the main hangar (average draw calls 100-350), so if you end up getting 1000+ draw calls, double check the settings.
Abovementioned performance info measured with SSAO Pro (Ultra quality), SSAA (Lanczos High 2x sampling scale), Tonemapping, Bloom and Motion blur applied.

Elevator
========
Hardcoded to demonstrate, but I suggest you do it your way (state machine). Easy to do so because the elevator follows the concept of the entire level design.
One floor is 4 units (4 meters) high. Simply give Elevator.cs a value from your respective class, and it will move it accordingly.
There are 2 animators - one on the shaft, one on the elevator itself. They both move the 2 doors. (Elevator's animation triggers the shaft door's animator event
a little delayed, so they look more realistic not opening together.)

Doors
=====
Doors are animated already, the script adjusts the lod model's door accordingly (only LOD0 moves). The animator is attached too LOD0 door object itself.

Building
========
I put the bundle together in a way that everything not related to the construction pack (eg. standard assets, DemoFPSPlayer class) can be removed
simply by deleting the 'Demo' folder. All other classes will work, you will only lose the demo scene

Find the detailed assembly guide in a separate file!

Any questions/update requests are welcome at gabromedia@gmail.com


Happy level building, hope you'll have as much fun designing your own level as I did creating the asset!
Rate the asset if you like it!


GabroMedia






