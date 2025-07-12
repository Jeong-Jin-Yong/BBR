using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        // forward(=Z) 방향 법선으로 반지름 1짜리 원
        Handles.DrawWireCube(transform.position, new Vector2(1f, 1f));
    }
}
