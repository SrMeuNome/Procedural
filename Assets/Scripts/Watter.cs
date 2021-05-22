using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watter : MonoBehaviour
{
    private Rigidbody2D rb { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        jumpWatter(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        jumpWatter(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        rb.gravityScale = 9.8f;
        rb = new Rigidbody2D();
    }

    private void jumpWatter(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D componentBody))
            {
                rb = componentBody;
                rb.velocity = new Vector2(0, 0);
                rb.gravityScale = 2;
            }
        }

    }
}
