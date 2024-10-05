using UnityEngine;

[CreateAssetMenu(fileName = "BubbleColor", menuName = "ScriptableObjects/BubbleColor", order = 1)]
public class BubbleColors: ScriptableObject
{
    public Color[] Color;
    public char[] ColorId;
}
