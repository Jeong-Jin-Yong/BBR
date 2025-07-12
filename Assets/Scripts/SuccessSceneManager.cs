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
    [Header("UI ����")]
    public Image backgroundImage;
    public Image breadImage;
    public TextMeshProUGUI successText;

    [Header("�մԺ� ���")]
    public Sprite boyBackground;
    public Sprite girlBackground;
    public Sprite kidBackground;
    public Sprite grandmaBackground;

    [Header("�� �̹���")]
    public Sprite mochaBreadImage;
    public Sprite whiteBreadImage;
    public Sprite sausageBreadImage;

    [Header("�մԺ� �� ��Ʈ ����Ʈ")]
    public List<CustomerLineGroup> customerLineGroups;

    [Header("��ư ����")]
    public Button button1;
    public Button button2;
    public Button button3;

    void Start()
    {
        // ��ư �̺�Ʈ ����
        button1.onClick.AddListener(() => SceneManager.LoadScene("Start"));
        button2.onClick.AddListener(() => SceneManager.LoadScene("Order"));
        button3.onClick.AddListener(() => Application.Quit());

        // ��� �̹��� ����
        switch (GameState.lastCustomerType)
        {
            case CustomerType.Boy: backgroundImage.sprite = boyBackground; break;
            case CustomerType.Girl: backgroundImage.sprite = girlBackground; break;
            case CustomerType.Kid: backgroundImage.sprite = kidBackground; break;
            case CustomerType.Grandma: backgroundImage.sprite = grandmaBackground; break;
        }

        // �� �̹��� ����
        switch (GameState.lastTargetBread)
        {
            case "moca": breadImage.sprite = mochaBreadImage; break;
            case "white": breadImage.sprite = whiteBreadImage; break;
            case "sausage": breadImage.sprite = sausageBreadImage; break;
        }

        // ��Ʈ ����
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
        return "���ְ� ��̾��!";
    }
}
