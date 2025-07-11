using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [SerializeField] private float disableX = -10f;

    private float moveSpeed = 5.0f; // 게임이 진행될 수록 점점 빨라지도록 설정

    private ObjectPool objectPool;

    public void Init(ObjectPool pool)
    {
        objectPool = pool;
    }

    private void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        
        if (transform.position.x < disableX && objectPool != null)
        {
            objectPool.ReturnObject(gameObject);
        }
    }
}
