using System;
using UnityEngine;

public class PlatformerPlayer : MonoBehaviour
{
    public float speed = 4.5f;
    public float jumpForce = 12.0f;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D box;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        var deltaX = Input.GetAxis("Horizontal") * speed;
        var movement = new Vector2(deltaX, body.velocity.y);
        body.velocity = movement;
        
        anim.SetFloat("speed", Mathf.Abs(deltaX));
        if (!Mathf.Approximately(deltaX, 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
        }
        
        var max = box.bounds.max;
        var min = box.bounds.min;
        var corner1 = new Vector2(max.x, min.y - 0.1f);
        var corner2 = new Vector2(min.x, min.y - 0.2f);
        var hit = Physics2D.OverlapArea(corner1, corner2);

        var grounded = hit != null;
        
        body.gravityScale = (grounded && Mathf.Approximately(deltaX, 0) ? 0 : 1);

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }


    }
}
