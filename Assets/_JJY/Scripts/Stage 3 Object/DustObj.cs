using UnityEngine;

public class SmoothMover : MonoBehaviour
{
    private Vector2 direction;
    private float changeInterval = 2f;
    private float timer = 0f;

    void Start()
    {
        SetNewDirection();
    }

    void Update()
    {
        transform.Translate(direction * Time.deltaTime * 3f);

        timer += Time.deltaTime;
        if (timer > changeInterval)
        {
            SetNewDirection();
            timer = 0f;
        }
    }

    void SetNewDirection()
    {
        direction = new Vector2(Random.Range(-2f, -1f), Random.Range(-1f, 1f)).normalized;
    }
}
