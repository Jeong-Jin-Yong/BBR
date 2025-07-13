using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] int currentRoundIndex;


    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    public void EndGame()
    {
        //게임종료 시 해당 상황에 맞는 이미지 넣기
        switch (gameManager.stageID)
        {
            case 1:
                //stageImage.sprite = stage1Ending;
                //stageText.text = texts[0];
                currentRoundIndex = 0;

                GameState.failedRoundIndex = currentRoundIndex;
                SceneManager.LoadScene("Fail");
                Debug.Log("스테이지1 게임오버");
                break;

            case 2:

                currentRoundIndex = 1;

                GameState.failedRoundIndex = currentRoundIndex;
                SceneManager.LoadScene("Fail");
                Debug.Log("스테이지2 게임오버");
                break;

            case 3:
                currentRoundIndex = 2;

                GameState.failedRoundIndex = currentRoundIndex;
                SceneManager.LoadScene("Fail");
                Debug.Log("스테이지3 게임오버");
                break;

            case 4:
                SceneManager.LoadScene("SuccessEnd");
                Debug.Log("게임 클리어");
                break;
        }
    }
}
