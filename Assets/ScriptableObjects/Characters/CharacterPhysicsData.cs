using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Physics Data", menuName = "Data/CharacterPhysics")]
public class CharacterPhysicsData : ScriptableObject
{
    public float speed = 50.0f;
    public float maxSpeed = 50.0f;
    public float speedKickoff = 2f;

    public float jumpForce = 7.0f;
    public float jumpColdTime = 0.25f;
    public float fuerzaExtra = 1.1f;

    public float coyoteTime = 0.25f;

    public float fallDamage = 1f;
    public float fallDamageMultiplier = 0.5f;
    public float fallDamageDistance = 4f;
}
