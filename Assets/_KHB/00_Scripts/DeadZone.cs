using UnityEngine;

public class DeadZone : MonoBehaviour
{
    // 추후 플레이어 레이캐스트로 다시 구현
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("플레이어 감지" + collision.collider.name);
            // 낙하 시 패배 조건 
            // 게임매니저의 isDead = true로 설정
        }
    }
}
