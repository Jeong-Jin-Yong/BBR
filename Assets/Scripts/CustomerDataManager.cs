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
    [Header("�մ� ������")]
    public List<CustomerData> customers;

    [Header("UI ����")]
    public Image characterImage;
    public TextMeshProUGUI balloonText;
    public RectTransform balloonTextRect;

    public RectTransform button1Rect; // ù ��° ��ư ��ġ��
    public RectTransform button2Rect; // �� ��° ��ư ��ġ��

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
        // 1. �մ� ���� ����
        CustomerData selected = customers[Random.Range(0, customers.Count)];

        // 2. ���� ��Ʈ ����
        OrderLineData selectedLine = selected.orderLines[Random.Range(0, selected.orderLines.Count)];

        // 3. UI�� ����
        characterImage.sprite = selected.characterSprite;
        balloonText.text = selectedLine.line;
        currentBreadTarget = selectedLine.targetBread;


        // ��ư �ؽ�Ʈ ����
        string breadText = "";
        switch (currentBreadTarget)
        {
            case "moca":
                breadText = "��ī��";
                break;
            case "white":
                breadText = "�Ļ�";
                break;
            case "sausage":
                breadText = "�Ҽ�����";
                break;
            default:
                breadText = "�� �����";
                break;
        }
        button1Text.text = breadText;


        // �߰�: GameState�� ����
        GameState.lastTargetBread = selectedLine.targetBread;
        GameState.lastCustomerType = selected.type;

        // 4. �մ� Ÿ�Կ� ���� UI ��ġ ����
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
