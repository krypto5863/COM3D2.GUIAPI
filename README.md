# COM3D2.GUIAPI
An API for making the tedious task of modifying COM's NGUI UI a trivial matter only requiring a few lines to complete complex tasks.

COM is one of those games where the UI is never edited and the plugins utilizing NGUI were seen as black magic. The work involved is more-over tedious and modders who are capable of making a GUI API just don't, doing the work only for their own plugins. 

GUIAPI aims to change that, putting powerful functions into the hands of less experienced modders allowing them to make robust changes to Kiss's very own GUI with only a few lines of code. Gone are the days of NGUI UI changes being exclusive to seasoned programmers.

Implemented
- Fully functional configuration menu API. You can create new tabs, add sliders, switches, dropdowns and input fields which you can reference and get changes from.

Planned
- Edit Mode API that allows the addition of new categories, sliders and/or dropdowns and similar controls.
- Plugin API. A simple API that allows you to create NGUI UIs for your plugins with IMGUI-like calls.

Why this API is better than other UI edits.

1. Simple to use. Dirt simple.
2. Changes occur in runtime and are gone when you close your game. No game files are ever modified and removing the plugin is enough to remove it's functionality.
3. We use COM's own sprite data to make UIs that fit seamlessly with the game's own UIs.
4. Way better for performance than IMGUI.

![COM3D2x64_cdvdzY1IlE](https://user-images.githubusercontent.com/29824718/125689541-14028a54-4ee2-4f42-b378-381ba052ab09.png)
