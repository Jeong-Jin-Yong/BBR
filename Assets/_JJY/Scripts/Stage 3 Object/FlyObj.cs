using UnityEngine;

public class FlyObj: MonoBehaviour
{
    private float curTime = 4f;

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
            curTime = 4f;
        }
        if (curTime <= 2f)
        {
            this.gameObject.transform.Translate(Vector3.up * 5f * Time.deltaTime);
        }
        else if (curTime <= 4f)
        {
            this.gameObject.transform.Translate(Vector3.down * 5f * Time.deltaTime);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) Destroy(gameObject);
    }
}
