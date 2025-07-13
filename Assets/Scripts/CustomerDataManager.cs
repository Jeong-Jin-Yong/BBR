using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum CustomerType { Girl, Boy, Grandma, Kid }

[System.Serializable]
public class OrderLineData
{
    public string line;
    public string targetBread;
}

[System.Serializable]
public class CustomerData
{
    public CustomerType type;
    public Sprite characterSprite;
    public List<OrderLineData> orderLines;
}

public class CustomerDataManager : MonoBehaviour
{
    [Header("손님 데이터")]
    public List<CustomerData> customers;

    [Header("UI 연결")]
    public Image characterImage;
    public TextMeshProUGUI balloonText;
    public RectTransform balloonTextRect;

    public RectTransform button1Rect; // 첫 번째 버튼 위치용
    public RectTransform button2Rect; // 두 번째 버튼 위치용

    public Button button1;
    public Button button2;
  

    public TextMeshProUGUI button1Text;

    private string currentBreadTarget;

    void Start()
    {
        ShowRandomCustomer();

        button1.onClick.AddListener(OnStartBreadGame);
        button2.onClick.AddListener(OnStartBreadGame);

    }

    public void ShowRandomCustomer()
    {
        // 1. 손님 랜덤 선택
        CustomerData selected = customers[Random.Range(0, customers.Count)];

        // 2. 랜덤 멘트 선택
        OrderLineData selectedLine = selected.orderLines[Random.Range(0, selected.orderLines.Count)];

        // 3. UI에 적용
        characterImage.sprite = selected.characterSprite;
        balloonText.text = selectedLine.line;
        currentBreadTarget = selectedLine.targetBread;


        // 버튼 텍스트 설정
        string breadText = "";
        switch (currentBreadTarget)
        {
            case "moca":
                breadText = "모카빵";
                break;
            case "white":
                breadText = "식빵";
                break;
            case "sausage":
                breadText = "소세지빵";
                break;
            default:
                breadText = "빵 만들기";
                break;
        }
        button1Text.text = breadText;


        // 추가: GameState에 저장
        GameState.lastTargetBread = selectedLine.targetBread;
        GameState.lastCustomerType = selected.type;

        // 4. 손님 타입에 따라 UI 위치 조정
        switch (selected.type)
        {
            case CustomerType.Girl:
                balloonTextRect.anchoredPosition = new Vector2(460f, 260f);
                button1Rect.anchoredPosition = new Vector2(680f, 10f);
                button2Rect.anchoredPosition = new Vector2(350f, 10f);

                break;

            case CustomerType.Boy:
                balloonTextRect.anchoredPosition = new Vector2(460f, 250f);
                button1Rect.anchoredPosition = new Vector2(660f, 10f);
                button2Rect.anchoredPosition = new Vector2(330f, 10f);

                break;

            case CustomerType.Grandma:
                balloonTextRect.anchoredPosition = new Vector2(440f, 230f);
                button1Rect.anchoredPosition = new Vector2(630f, -15f);
                button2Rect.anchoredPosition = new Vector2(300f, -15f);
  
                break;

            case CustomerType.Kid:
                balloonTextRect.anchoredPosition = new Vector2(410f, 230f);
                button1Rect.anchoredPosition = new Vector2(610f, -15f);
                button2Rect.anchoredPosition = new Vector2(280f, -15f);
       
                break;
        }
    }

    public void OnStartBreadGame()
    {
 

        SceneManager.LoadScene(2);

      
    }
}
