using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private void Update()
    {
        if(Input.anyKey)
        {
            SceneManager.LoadScene("Game_LSU"); //�� �ε� ���
            //titleUI.SetActive(false); //UI ������Ʈ ��Ȱ��ȭ
            Debug.Log("����ȭ������ �̵�");
        }
    }
}
