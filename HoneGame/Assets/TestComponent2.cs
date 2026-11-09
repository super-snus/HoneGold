using System.ComponentModel;
using System.Numerics;

public class TestComponent2 : Actor
{
    private SpriteRenderer _spriteRenderer;
    private Sprite _sprite;
    private Texture _texture;
    public override void Start()
    {
        _texture = new Texture();
        _texture.Load("mem.png");
        
        _sprite = GameObject.GetComponent<Sprite>();
        _sprite.texture = _texture;
        //_spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
        //_spriteRenderer.sprite = _sprite;
    }
}