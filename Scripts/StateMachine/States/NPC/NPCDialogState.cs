using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class NPCDialogState : State
{
    [Export] public Array<DialogResource> DialogResources = new Array<DialogResource>();
}
