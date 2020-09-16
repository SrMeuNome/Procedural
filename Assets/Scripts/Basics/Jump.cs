using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private float JumpStrong { get; set; } = 20;
    private string JumpKey { get; set; } = "space";
    private bool isJump { get; set; } = false;

    public Vector2 StartPoint;
    public Vector2 EndPoint;

    private Rigidbody2D rb { get; set; }

    //Key Codes to String https://docs.unity3d.com/Manual/class-InputManager.htm
    // Start is called before the first frame updatel
    void Start()
    {
        //Pegando RigidBody2D
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D componentBody))
        {
            rb = componentBody;
        }
        else
        {
            gameObject.AddComponent<Rigidbody2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        //Modificando o RigidBody2D do objeto
        rb.gravityScale = 9.8f;
        rb.angularDrag = 50;
        rb.mass = 1;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.interpolation = RigidbodyInterpolation2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(JumpKey) && isJump)
        {
            rb.velocity = new Vector2(0, JumpStrong);
            //rb.AddRelativeForce(new Vector2(0, JumpStrong * Time.smoothDeltaTime), ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        ColisionJump("chao");
        ColisionJump("agua");
    }

    private void ColisionJump(string tag)
    {
        Debug.DrawLine(new Vector2(transform.position.x + StartPoint.x, transform.position.y + StartPoint.y), new Vector2(transform.position.x + EndPoint.x, transform.position.y + EndPoint.y), Color.red);
        RaycastHit2D Hits = Physics2D.Linecast(new Vector2(transform.position.x + StartPoint.x, transform.position.y + StartPoint.y), new Vector2(transform.position.x + EndPoint.x, transform.position.y + EndPoint.y));

        if (Hits.collider != null)
        {
            if (Hits.collider.CompareTag(tag))
            {
                isJump = true;
            }
        }
        else
        {
            isJump = false;
        }
    }
}
