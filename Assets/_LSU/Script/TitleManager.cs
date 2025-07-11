using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private void Update()
    {
        if(Input.anyKey)
        {
            SceneManager.LoadScene("GameScene"); //씬 로드 방식
            //titleUI.SetActive(false); //UI 오브젝트 비활성화
            Debug.Log("게임화면으로 이동");
        }
    }
}
