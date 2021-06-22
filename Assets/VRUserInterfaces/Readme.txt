VR User Interfaces:

Check out the example scenes to test out the given menus and the two PointerModes (Pointer and touch).

Creating a new button:
To make a new button you'll want to decide how it is going to be visualised. The methods currently supported are MeshRenderer, Enabling/Disabling Objects, UnityEngine.UI.Image and UnityEngine.UI.Text.
Once decided make your new object with a MeshRenderer or UI component, add a collider then add the VRButton script. The first thing you'll want to do is supply
Material/Sprites/Colors to the Default, Hover, Clicked and optional Locked variables. At this point it should flick through those different visualisations
when pointing and clicking.
If you want to enable and disable objects you'll want to tick the Use Toggleable Objects box and supply any or all of the references.
To have the button do something you can chose one or both of the event methods, tick Use Send Message to show the options for using the SendMessage method,
Supply a target object, method name and string parameter.
Ticking Use Unity Event will show the UnityEvent dialogue where you can specify a target object and method.
For reference check the example scene buttons to see how they are set up.

Creating a new toggle:
The toggle is very similar to the button but adds the inactive state visualisations. You can set the StartEnabled boolean to chose whether it will
start enabled or disabled. There are enabling and disabling events. Use the two toggle examples in the example scenes for reference on how these look
when set up.

Creating a new slider:
The slider requires a parent object but has the same basis as the button. You can set the min and max positions on any of the axis that are unlocked,
these are visualised in the scene by arrows (you can lower the size of the arrows depending on the scale you are working at) using the Editor Arrow Size
slider.
There are three examples of the slider, in the menu is a 1D and 2D slider and in the TouchExampleScene a 3D slider is used to configure the interaction slider
position (attached directly to the interaction sphere).

VRInteractor:
The VRInteractor inherits from VRInput which is used to process controls from both the Vive and Oculus and convert them into method calls.
The Interactor expands upon this to add an interaction collider and the ability to process objects touching the sphere then send them method
calls. Feel free to reposition and scale the collider to what best suits your application.

VRPointer:
The VRPointer script is attached to the controllers in the SteamVR camera rig along with a VRInteractor script. It implements the ACTION method
and uses it to send receivers commands. The Pointer can be set to point or to touch and each is shown in seperate example scenes. You can press
a button assigned to ACTION when hovering over or press and hold the button then move the pointer or toucher over the receiver.

When set to point it uses a raycast and linerenderer to fire a line from the interaction collider in the forward direction of the
interaction collider (this allows you to set the origin position and direction of the pointer). When in this mode you can choose the layer
the raycast will look for as well as the line material and a prefab that will be shown at the hit point (use this to show a cursor).
When set to touch mode it will use the interaction collider directly to interact with receiver objects.

VRPointerReceiver:
This is the base class for the button, toggle and slider scripts and implements the hover and click transitions. Any receiver can use a Sprite,
Mesh Renderer or Text component for the visualisation and the inspector will give options relating to the chosen component.
-Start Locked: Any receiver can be locked which will make it uninteractable, another script will then use the setter method in the receiver script
to unlock the receiver.
-Disable Object On Start: This will call gameObject.SetActive(false); in the Start method.
-Use Toggleable Objects: This can always be used, mainly if you want your visualisation to be enabling and disabling other objects (example in the
PointerExampleScene settings menu)
-Default: The default sprite, material, object and colour. This is shown as the default state.
-Hover: This is show when the pointer or interaction collider touches the receiver but the ACTION button is not being held down.
-Clicked: When the ACTION button is being held down.
-Locked: When the object is locked

VRButton:
The button inherits from VRPointerReceiver and implements the Activate method that either uses SendMessage or invokes a unity event.
-On Activate Anim State Name: This will only show up if there is an Animator component attached. This allows you to enter the string name
of an animation state that will then play when the button is pressed.
-Use Send Message: When ticked you will be able to assign an object, method name and string parameter, the target object must have a 
script attached with a method of the same name and take a string parameter.
-Use Unity Event: Click the + button, assign a target gameObject and chose the method you want to call or add a listener from a script
by calling unityEvent.AddListener(methodName);

VRToggle:
The Toggle has an enabled boolean that will switch each time it is pressed. The component adds deactivation variables for most fields.
-On Activate Anim State Name: This will only show up if there is an Animator component attached. This allows you to enter the string name
of an animation state that will then play when the toggle is activated.
-On Deactivate Anim State Name: This will play when the toggle is deactivated. (An example of this is in the Settings menu)
-When Inactive: These are the same as the base but only display when inactive. An Animator will override these variables in most
instances if one is attached.
-Use Send Message: A second message can be sent when deactivating.
-Use Unity Event: A second event can be invoke when deactivating.

VRSlider:
The Slider can be used to make a simple 1 dimensional slide but can also be two and even three dimensional draggable slide.
The slider has getters that return between 0 and 1 the current position between the min and max, you can also listen to a
delegate that will be called every time the slider is being moved. Example found in the VRSliderReadout script used in the settings menu.
The slider is used by the Change Touch Position button in the TouchExampleScene to change the position of the interaction collider.
-Editor Arrow Size: Arrows in the scene view allow for easy visualisation of the limits the slider will be constrained to.
-ConstrainX,Y,Z: Ticked to lock the dimension
-Min, Max: The min and max local position the object can be moved between.

What's New:
1.0:
-Updated to new VR Interaction
0.1:
-Initial release