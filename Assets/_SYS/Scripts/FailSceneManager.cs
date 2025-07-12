using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;



[System.Serializable]
public class FailRoundData
{
    public int roundIndex;  // 실패 라운드 인덱스 (0~2)
    public Sprite breadImage;  // 빵 실패 이미지 (사람과 무관)

    public List<string> girlLines;
    public List<string> boyLines;
    public List<string> grandmaLines;
    public List<string> kidLines;
}

[System.Serializable]
public class BreadFailData
{
    public string breadName;  // "모카빵", "식빵", "소세지빵"
    public List<FailRoundData> rounds;  // 각 라운드별 실패 데이터
}



public class FailSceneManager : MonoBehaviour
{
    [Header("UI")]
    public Image backgroundImage;
    public Image breadImage;
    public TextMeshProUGUI failText;

    [Header("손님별 배경")]
    public Sprite boyBackground;
    public Sprite girlBackground;
    public Sprite kidBackground;
    public Sprite grandmaBackground;

    [Header("빵별 실패 데이터")]
    public List<BreadFailData> breadFailDataList;

    public Button button1;
    public Button button2;
    public Button button3;

    void Start()
    {
        // 버튼 이벤트 등록
        button1.onClick.AddListener(() => SceneManager.LoadScene("Start"));
        button2.onClick.AddListener(() => SceneManager.LoadScene("Order"));
        button3.onClick.AddListener(() => Application.Quit());

        // 배경 설정 (손님별)
        switch (GameState.lastCustomerType)
        {
            case CustomerType.Boy: backgroundImage.sprite = boyBackground; break;
            case CustomerType.Girl: backgroundImage.sprite = girlBackground; break;
            case CustomerType.Kid: backgroundImage.sprite = kidBackground; break;
            case CustomerType.Grandma: backgroundImage.sprite = grandmaBackground; break;
        }

        // 실패 데이터 찾기
        BreadFailData breadData = breadFailDataList.Find(b => b.breadName == GameState.lastTargetBread);
        if (breadData == null) { failText.text = "실패했어요..."; return; }

        FailRoundData roundData = breadData.rounds.Find(r => r.roundIndex == GameState.failedRoundIndex);
        if (roundData == null) { failText.text = "실패했어요..."; return; }

        // 빵 실패 이미지 (라운드 기준)
        breadImage.sprite = roundData.breadImage;

        // 멘트 선택 (손님 기준)
        failText.text = GameState.lastCustomerType switch
        {
            CustomerType.Girl => GetRandomLine(roundData.girlLines),
            CustomerType.Boy => GetRandomLine(roundData.boyLines),
            CustomerType.Grandma => GetRandomLine(roundData.grandmaLines),
            CustomerType.Kid => GetRandomLine(roundData.kidLines),
            _ => "실패했어요..."
        };
    }

    string GetRandomLine(List<string> lines)
    {
        if (lines == null || lines.Count == 0) return "실패했어요...";
        return lines[Random.Range(0, lines.Count)];
    }
}
