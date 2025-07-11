using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    GameManager_LSU gameManager;

    [Header("스테이지 종료 화면")]
    [SerializeField] GameObject stageResultGroup; //스테이지 게임오브젝트 그룹
    [SerializeField] Image stageImage; //이미지
    [SerializeField] Sprite stage1Ending; //스테이지1 엔딩
    [SerializeField] Sprite stage2Ending; //스테이지2 엔딩
    [SerializeField] Sprite stage3Ending; //스테이지3 엔딩
    [SerializeField] Sprite clearEnding; //클리어 엔딩

    [SerializeField] Text stageText; //스테이지 종료 텍스트
    [SerializeField] string[] texts; //스테이지 종료 텍스트 배열


    private void Awake()
    {
        gameManager = GetComponent<GameManager_LSU>();
    }

    public void EndGame()
    {
        //게임종료 시 해당 상황에 맞는 이미지 넣기
        switch (gameManager.stageID)
        {
            case 1:
                stageImage.sprite = stage1Ending;
                stageText.text = texts[0];
                Debug.Log("스테이지1 게임오버");
                break;

            case 2:
                stageImage.sprite = stage2Ending;
                stageText.text = texts[1];
                Debug.Log("스테이지2 게임오버");
                break;

            case 3:
                stageImage.sprite = stage3Ending;
                stageText.text = texts[2];
                Debug.Log("스테이지3 게임오버");
                break;

            case 4:
                stageImage.sprite = clearEnding;
                stageText.text = texts[3];
                Debug.Log("게임 클리어");
                break;
        }

        stageResultGroup.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene("Game_LSU");
    }

    public void Exit()
    {
        SceneManager.LoadScene("Title_LSU");
        //Application.Quit();
    }
}
