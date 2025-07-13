using System.Collections;
using UnityEngine;

public class WaterObj : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 lastPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) Destroy(gameObject);
        else if (collision.gameObject.CompareTag("Ground"))
        {
            transform.position = lastPos - new Vector3(1f, 0f, 0f);
            lastPos = transform.position;
            rb.linearVelocity = new Vector2(0, 0);
        }
        else if (collision.gameObject.CompareTag("BreadSkill"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
