using UnityEngine;

public class FlyObj: MonoBehaviour
{
    private float curTime = 3f;

    private void OnEnable()
    {
        Destroy(gameObject, 30f);
    }

    private void Update()
    {
        Movement();
        transform.Translate(Vector3.left * 2.5f * Time.deltaTime);
    }

    void Movement()
    {
        curTime -= Time.deltaTime;

        if (curTime <= 0f)
        {
            curTime = 3f;
        }
        if (curTime <= 1.5f)
        {
            this.gameObject.transform.Translate(Vector3.up * 5f * Time.deltaTime);
        }
        else if (curTime <= 3f)
        {
            this.gameObject.transform.Translate(Vector3.down * 5f * Time.deltaTime);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) Destroy(gameObject);
    }
}
