using UnityEngine;
using UnityEngine.SceneManagement;

public class TapToNextScene : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 화면을 누르면
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}