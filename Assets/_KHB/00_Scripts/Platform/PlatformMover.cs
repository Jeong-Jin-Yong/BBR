using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    private float disableX = -25f;

    [SerializeField]
    private float moveSpeed = 4.0f; // 플랫폼 이동 속도
    private bool isMoving = true;

    private void Update()
    {
        if (isMoving) 
        { 
            MoveSegment();
            CheckDestruction();
        }
    }

    private void MoveSegment()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    private void CheckDestruction()
    {
        if (transform.position.x <= disableX)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        // SegmentGenerator의 activeSegments 큐에서 제거
        if (SegmentGenerator.Instance != null)
        {
            SegmentGenerator.Instance.RemoveSegmentFromQueue(gameObject);
        }

        if (SegmentPool.Instance != null)
        {
            SegmentPool.Instance.ReturnSegment(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void ResetPosition(Vector3 position)
    {
        transform.position = position;
        StartMoving();
    }

    private void OnEnable()
    {
        // 세그먼트가 활성화될 때 이동 시작
        StartMoving();
    }
}
