using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        // forward(=Z) ���� �������� ������ 1¥�� ��
        Handles.DrawWireCube(transform.position, new Vector2(1f, 1f));
    }
}
