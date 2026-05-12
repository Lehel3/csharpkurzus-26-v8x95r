# Blob World

> A rule-based, step-by-step simulation inspired by PrimerBlob-style experiments.  
> Built in C# (.NET), with optional Unity visualization support via DLL import.

---

## About the Project

Blob World is a simple program that simulates an artificial ecosystem made up of autonomous agents called **blobs**. The simulation models how blobs move, search for food, consume resources, and reproduce over time. It is aimed at students, researchers, and developers who want to study emergent behavior such as population dynamics and resource competition without needing real-time graphics.

Instead of focusing primarily on graphics, the project emphasizes **simulation logic, system architecture, and emergent behavior**. The core logic is written as a clean C# library that can run standalone from the command line, or be imported into Unity as a `.dll` for optional visual playback — without duplicating any code.


### Unity Build Controls

- Middle Mouse Btn - Pan
- Right Mouse Btn - Rotate around
- Mouse Scroll - Zoom
- Rest of the the controls are on the control panel at the bottom of the screen


---

## File Structure

- **BlobSimulation** - Core Simulation Logic, ConsoleRunner, Tests
- **UnityBlobSimulation** - Unity Visualization Project Files
- **UnityBuilds** - Available Builds of Unity Visualization Project

---

## Prerequisites

- [Git](https://git-scm.com/)
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Unity 6.3 LTS (6000.3.10f1)](https://unity.com/releases/editor/whats-new/6000.3.10)
  *(only required if you want to open the Unity visualization project)*

The simulation library will be available immediately, and updated on building the Core project — no manual DLL copying required.

> Make sure you are using exactly **Unity 6000.3.10f1** to avoid compatibility issues.

---

## Possible Future Work

Possible extensions of the simulation include:

- unity project switch between loading config from json / scriptableobject ( it would probably be smart to use the same json for both of the runners)
- inheritance, mutation of blob traits
- multiple parent breeding
- more advanced blob behavior
- spatial grid optimization
- blob soft collisions (blobs pushing each other aparts)
- additions to the world, such as tiles
- improved visualization
- statistics and data visualization
- save, load simulation state
- reproducible simulation state with only changed data saved per tick, instead all simulation states

---

*Last updated: 2026-05-12
