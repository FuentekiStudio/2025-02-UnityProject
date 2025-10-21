using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PositionMemento
{
    public readonly Vector2 Position;
    public readonly Vector2 Velocity;

    public PositionMemento(Vector2 pos, Vector2 vel)
    {
        Position = pos;
        Velocity = vel;
    }
}

