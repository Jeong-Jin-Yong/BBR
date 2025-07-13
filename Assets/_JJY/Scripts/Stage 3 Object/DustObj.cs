using UnityEngine;

public class DustObj : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(gameObject, 30f);
    }

    private void Update()
    {
        Movement();
    }

    void Movement()
    {
        var x = Random.Range(-2f, -1f);
        var y = Random.Range(-10f, 10f);
        transform.Translate(new Vector2(x, y) * Time.deltaTime * 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) Destroy(gameObject);
        else if (collision.gameObject.CompareTag("BreadSkill"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
