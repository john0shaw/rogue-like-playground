using System;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class DialogResource : Resource
{
    [Export] public string Speaker;
    [Export] public Array<string> Dialog = new Array<string>();
}
