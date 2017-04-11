# unity-inspector-help
*Easily add the help text info box to your Unity3d inspector properties.*

---

Unity has these awesome little info boxes you may see popping up in the IDE, they can have a little icon and display helpful information for the IDE user to see.

![Alt text](/screenshots/ss-unity-console.png?raw=true)

![Alt text](/screenshots/ss-unity-inspector-1.png?raw=true)

![Alt text](/screenshots/ss-unity-inspector-2.png?raw=true)

I thought it would be nice for the Unity guys to make it publicly available to use as an `Attribute`. I couldn't find it anywhere so I rolled my own for personal use and I'm releasing it under MIT license. Feel free to use it in your projects commercial or non-commercial. Using this little helper class you can apply help text or other important information to any property or field made visible in the Unity inspector from inside your `UnityBehavior` class.

The benefit of using the standard Unity info box from Attribute is that it makes it clear to the IDE user if any instructions are to be followed and is the ultimate addition or alternative to tooltip, which can be easily overlooked when configuring inspector properties. Since it uses EditorGUI.HelpBox, it keeps your inspector looking simple and fits in with the standard unity UI design. Using this custom `PropertyAttribute` along with the built in `[Space(#)]` or `[SpaceAttribute(#)]` and `[Header($$)]` or `[HeaderAttribute($$)]` allows you to create a nice layout, with detailed developer/user notes, for the Unity property window within your `UnityBehavior` scripts.

## Installation

Drop the `HelpAttribute.cs` file anywhere in your project's Assets, I personally keep it located at `Assets/_InspectorExtensions/HelpAttribute.cs`

**No further dependencies are required!**


## Usage

The default message icon is set to `UnityEditor.MessageType.Info` which displays a speech bubble icon to the left of the help text. Usage can be seen below:

```c#
[SerializeField]
[Help("This is some help text!")]
float inspectorField = 1440f;
```

The message icon can be set using the second (optional) parameter in the `[HelpAttribute]` constructor and allows the following values:

`UnityEditor.MessageType.Error`
`UnityEditor.MessageType.Info`
`UnityEditor.MessageType.None`
`UnityEditor.MessageType.Warning`

 If this second parameter is used, wrap the `[HelpAttribute]` in an `#if UNITY_EDITOR` to ensure no errors when building to your chosen platform.

```c#
[SerializeField]
#if UNITY_EDITOR
[Help("This is some help text!", UnityEditor.MessageType.None)]
#endif
float inspectorField = 1440f;
```

An example screenshot shows some usage:

![Alt text](/screenshots/ss-unity-help-attribute.png?raw=true)


## Limitations

As with other custom, and even some built in `PropertyAttributes`, there are conflicts:

1. `[Range(#,#)]` or `[RangeAttribute(#,#)]` did not work with `[HelpAttribute]` by default and required reimplementing inside `[HelpAttribute]`'s `PropertyDrawer`. Due to this it is required that `[Range]` is added to an inspector property before `[Help]`.
2. I also found that `[Help]` does not work with nested inspector properties which display a dropdown arrow with child elements such as an `array[]` or `List<>`.
3. Reimplementation of `[Multiline]` or `[MultilineAttribute]` was also required and causes the property to be drawn with the label above and the textbox below, rather than side by side.
4. Most primitives should display correctly but some will require reimplementation in the `[HelpAttribute]` `PropertyDrawer`. It's likely that other custom `PropertyDrawer` will conflict.


## Collaboration

Please feel free to fork and push back if you make any changes which would benefit the community:

1. Fork the target repo to your own account.
2. Clone the repo to your local machine.
3. Check out a new "topic branch" and make changes.
4. Push your topic branch to your fork.
5. Use the diff viewer on GitHub to create a pull request via a discussion.
6. Make any requested changes.
