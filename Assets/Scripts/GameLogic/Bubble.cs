using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Bubble : MonoBehaviour
{
    public Color Color { get; private set; }
    public char ColorId { get; private set; }

    public void Initialize(Color color, char colorId)
    {
        Color = color;
        ColorId = colorId;
        SetColor(color); 
    }

    private void SetColor(Color color)
    {
        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            spriteRenderer.color = color; 
        }
    }


}
