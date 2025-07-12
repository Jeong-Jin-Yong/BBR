using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;



[System.Serializable]
public class FailRoundData
{
    public int roundIndex;  // ���� ���� �ε��� (0~2)
    public Sprite breadImage;  // �� ���� �̹��� (����� ����)

    public List<string> girlLines;
    public List<string> boyLines;
    public List<string> grandmaLines;
    public List<string> kidLines;
}

[System.Serializable]
public class BreadFailData
{
    public string breadName;  // "��ī��", "�Ļ�", "�Ҽ�����"
    public List<FailRoundData> rounds;  // �� ���庰 ���� ������
}



public class FailSceneManager : MonoBehaviour
{
    [Header("UI")]
    public Image backgroundImage;
    public Image breadImage;
    public TextMeshProUGUI failText;

    [Header("�մԺ� ���")]
    public Sprite boyBackground;
    public Sprite girlBackground;
    public Sprite kidBackground;
    public Sprite grandmaBackground;

    [Header("���� ���� ������")]
    public List<BreadFailData> breadFailDataList;

    public Button button1;
    public Button button2;
    public Button button3;

    void Start()
    {
        // ��ư �̺�Ʈ ���
        button1.onClick.AddListener(() => SceneManager.LoadScene("Start"));
        button2.onClick.AddListener(() => SceneManager.LoadScene("Order"));
        button3.onClick.AddListener(() => Application.Quit());

        // ��� ���� (�մԺ�)
        switch (GameState.lastCustomerType)
        {
            case CustomerType.Boy: backgroundImage.sprite = boyBackground; break;
            case CustomerType.Girl: backgroundImage.sprite = girlBackground; break;
            case CustomerType.Kid: backgroundImage.sprite = kidBackground; break;
            case CustomerType.Grandma: backgroundImage.sprite = grandmaBackground; break;
        }

        // ���� ������ ã��
        BreadFailData breadData = breadFailDataList.Find(b => b.breadName == GameState.lastTargetBread);
        if (breadData == null) { failText.text = "�����߾��..."; return; }

        FailRoundData roundData = breadData.rounds.Find(r => r.roundIndex == GameState.failedRoundIndex);
        if (roundData == null) { failText.text = "�����߾��..."; return; }

        // �� ���� �̹��� (���� ����)
        breadImage.sprite = roundData.breadImage;

        // ��Ʈ ���� (�մ� ����)
        failText.text = GameState.lastCustomerType switch
        {
            CustomerType.Girl => GetRandomLine(roundData.girlLines),
            CustomerType.Boy => GetRandomLine(roundData.boyLines),
            CustomerType.Grandma => GetRandomLine(roundData.grandmaLines),
            CustomerType.Kid => GetRandomLine(roundData.kidLines),
            _ => "�����߾��..."
        };
    }

    string GetRandomLine(List<string> lines)
    {
        if (lines == null || lines.Count == 0) return "�����߾��...";
        return lines[Random.Range(0, lines.Count)];
    }
}
