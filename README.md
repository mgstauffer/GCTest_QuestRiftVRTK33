# GCTest_QuestRiftVRTK33

This is a test project for building to Oculus Quest using VRTK 3.3, Oculus Integration 1.37, and Unity 2018.4.9f1

# What Works So Far?

## Building from Unity

With Unity 2018, it's highly important to install the proper Android SDK tools, otherwise Unity will not even recognize the Quest. [Following this article](https://circuitstream.com/blog/oculus-quest-unity-setup/), you can use Android Studio package manager to download the proper SDKs and SDK Tools. For Player, Build, and Quality settings, use the settings listed [here](https://developer.oculus.com/documentation/unity/unity-conf-settings/).

Oculus Integration 1.37 will need to be installed from the [Unity Integration Archive](https://developer.oculus.com/downloads/package/unity-integration-archive/1.37.0/). This was simply imported and the OVRCameraRig was used.

An important note is that an older version of Unity 2018, such as 2018.4.9f1, is completely necessary. For whatever reason, building with the LTS version (2018.4.32f1 at the time) will always build with a black screen.

## Building with VRTK 3.3

Following the steps from the [VRTK 3.3 Docs](https://vrtoolkit.readme.io/docs/getting-started), the OVRCameraRig is added to the VRTK SDKManager. For Quest building, the best SDK to put in SDKSetup is GearVR (Oculus:Android). This will automatically install the Oculus Android plugin and keep it there, even if it's deleted.

## UIPointers

The VRTK UIPointer and UICanvas is fully supported. Using a pointer, you can point at a UI with sliders and change the colors of a cube. This works on both desktop and Quest.

## Controller Event Handling

When a controller input is read, it will display on the in-game VRTK ConsoleViewer, which mimics the Unity console. This works on desktop and Quest.

## Tooltips

Tooltips are supported on both desktop and Quest. Since I wasn't able to get actual virtual hands shaped like the oculus controllers, I couldn't get the tooltips to point to specific buttons. However, the tooltips are there, and their origin point can easily be changed, meaning pointing to buttons on a virtual controller is feasible.
