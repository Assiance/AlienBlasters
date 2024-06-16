using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WaterFlowAnimation : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    [SerializeField] float _scrollSpeed = 1f;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        var x = Mathf.Repeat(Time.time * _scrollSpeed, 1);
        var offset = new Vector2(x, 0);
        _spriteRenderer.material.SetTextureOffset("_MainTex", offset);
    }
}