using UnityEngine;

public class DeadZone : MonoBehaviour
{
    // ���� �÷��̾� ����ĳ��Ʈ�� �ٽ� ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("�÷��̾� ����" + collision.collider.name);
            // ���� �� �й� ���� 
            // ���ӸŴ����� isDead = true�� ����
        }
    }
}
