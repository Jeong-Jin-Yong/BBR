using UnityEngine;
using UnityEngine.SceneManagement;

public class TapToNextScene : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ȭ���� ������
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}