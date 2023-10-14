using Godot;
using System;

public partial class InteractableComponent : Node
{
    [Signal] public delegate void InteractedEventHandler();
    
	public void Interact()
    {
        EmitSignal("Interacted");
    }
}
