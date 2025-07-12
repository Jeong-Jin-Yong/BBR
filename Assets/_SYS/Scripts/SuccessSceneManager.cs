using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;



[System.Serializable]
public class BreadLineData
{
    public string breadName;
    public List<string> lines;
}

[System.Serializable]
public class CustomerLineGroup
{
    public CustomerType type;
    public List<BreadLineData> breadLines;
}

public class SuccessSceneManager : MonoBehaviour
{
    [Header("UI 연결")]
    public Image backgroundImage;
    public Image breadImage;
    public TextMeshProUGUI successText;

    [Header("손님별 배경")]
    public Sprite boyBackground;
    public Sprite girlBackground;
    public Sprite kidBackground;
    public Sprite grandmaBackground;

    [Header("빵 이미지")]
    public Sprite mochaBreadImage;
    public Sprite whiteBreadImage;
    public Sprite sausageBreadImage;

    [Header("손님별 빵 멘트 리스트")]
    public List<CustomerLineGroup> customerLineGroups;

    [Header("버튼 연결")]
    public Button button1;
    public Button button2;
    public Button button3;

    void Start()
    {
        // 버튼 이벤트 연결
        button1.onClick.AddListener(() => SceneManager.LoadScene("Start"));
        button2.onClick.AddListener(() => SceneManager.LoadScene("Order"));
        button3.onClick.AddListener(() => Application.Quit());

        // 배경 이미지 설정
        switch (GameState.lastCustomerType)
        {
            case CustomerType.Boy: backgroundImage.sprite = boyBackground; break;
            case CustomerType.Girl: backgroundImage.sprite = girlBackground; break;
            case CustomerType.Kid: backgroundImage.sprite = kidBackground; break;
            case CustomerType.Grandma: backgroundImage.sprite = grandmaBackground; break;
        }

        // 빵 이미지 설정
        switch (GameState.lastTargetBread)
        {
            case "moca": breadImage.sprite = mochaBreadImage; break;
            case "white": breadImage.sprite = whiteBreadImage; break;
            case "sausage": breadImage.sprite = sausageBreadImage; break;
        }

        // 멘트 설정
        successText.text = GetRandomLineByCustomerAndBread(GameState.lastCustomerType, GameState.lastTargetBread);
    }

    string GetRandomLineByCustomerAndBread(CustomerType type, string breadName)
    {
        foreach (var group in customerLineGroups)
        {
            if (group.type == type)
            {
                foreach (var bread in group.breadLines)
                {
                    Debug.Log($"{breadName}");
                    if (bread.breadName == breadName)
                    {
                        //Debug.Log($"{bread.lines} {bread.lines.Count}");
                        if (bread.lines != null && bread.lines.Count > 0)
                            return bread.lines[Random.Range(0, bread.lines.Count)];
                    }
                }
            }
        }
        return "맛있게 드셨어요!";
    }
}
