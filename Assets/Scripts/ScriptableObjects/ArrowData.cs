using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Object")]
public class ArrowData : ScriptableObject
{
    public string idName = "";
    public float damage = 50f;
}