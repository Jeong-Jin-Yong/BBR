using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientManager : MonoBehaviour
{
    public GameManager gameManager;

    public ItemData[] itemDatas;
    [SerializeField] ItemData target;

    private List<Text> ingredientTexts = new List<Text>();

    int[] collectedCounts;
    int[] deadCollectedCounts;

    [SerializeField] Transform currentIngredientGroup;
    public Transform[] groups;

    private void Start()
    {
        int ran = Random.Range(0, itemDatas.Length);
        target = itemDatas[ran];
        currentIngredientGroup = groups[ran];
        currentIngredientGroup.gameObject.SetActive(true);

        collectedCounts = new int[target.ingredients.needIngredients.Length];
        deadCollectedCounts = new int[target.ingredients.deadIngredients.Length];

        MapTextUI();
        UpdateIngredientUI();
    }

    private void MapTextUI()
    {
        ingredientTexts.Clear();

        for (int i = 0; i < target.ingredients.needIngredients.Length; i++)
        {
            string ingredientName = target.ingredients.needIngredients[i].name;

            Transform ingredientObj = currentIngredientGroup.Find(ingredientName);
            if (ingredientObj != null)
            {
                Transform textObj = ingredientObj.Find("Text");
                if (textObj != null)
                {
                    Text txt = textObj.GetComponent<Text>();
                    if (txt != null)
                    {
                        ingredientTexts.Add(txt);
                    }
                    else
                    {
                        Debug.LogWarning($"⚠️ Text 컴포넌트가 {ingredientName}에 존재하지 않습니다.");
                    }
                }
                else
                {
                    Debug.LogWarning($"⚠️ '{ingredientName}' 오브젝트에 'Text' 자식이 없습니다.");
                }
            }
            else
            {
                Debug.LogWarning($"⚠️ 그룹에 '{ingredientName}' 오브젝트가 존재하지 않습니다.");
            }
        }

        // 배열 길이와 텍스트 개수가 일치하지 않으면 경고
        if (ingredientTexts.Count != collectedCounts.Length)
        {
            Debug.LogError("❌ UI 텍스트 수와 필요 재료 수가 일치하지 않습니다.");
        }
    }

    private void UpdateIngredientUI()
    {
        for (int i = 0; i < ingredientTexts.Count; i++)
        {
            int current = collectedCounts[i];
            int max = target.ingredients.needCounts[i];
            ingredientTexts[i].text = $"{current} / {max}";
        }
    }

    public bool IngredientCheck(GameObject obj)
    {
        var ingredients = target.ingredients;
        string cleanedName = obj.name.Replace("(Clone)", "").Trim();

        // 필요한 재료 검사
        for (int i = 0; i < ingredients.needIngredients.Length; i++)
        {
            if (cleanedName == ingredients.needIngredients[i].name)
            {
                collectedCounts[i]++;
                Debug.Log($"✅ Collected {cleanedName}: {collectedCounts[i]}/{ingredients.needCounts[i]}");

                if (collectedCounts[i] >= ingredients.needCounts[i])
                {
                    switch (i)
                    {
                        case 0: gameManager.hasDough1 = true; break;
                        case 1: gameManager.hasDough2 = true; break;
                        case 2: gameManager.hasDough3 = true; break;
                        case 3: gameManager.hasDough4 = true; break;
                        case 4: gameManager.hasDough5 = true; break;
                    }
                }

                UpdateIngredientUI();
                return true; // ✅ 필요한 재료였다
            }
        }

        // 죽은 재료 검사 (스테이지 1 전용)
        if (gameManager.stageID == 1)
        {
            for (int i = 0; i < ingredients.deadIngredients.Length; i++)
            {
                if (cleanedName == ingredients.deadIngredients[i].name)
                {
                    deadCollectedCounts[i]++;
                    Debug.LogWarning($"❌ Wrong ingredient: {cleanedName} - {deadCollectedCounts[i]}/{ingredients.deadCounts[i]}");

                    if (deadCollectedCounts[i] >= ingredients.deadCounts[i])
                    {
                        Debug.LogError($"{cleanedName} too much! Game Over.");
                        gameManager.GameOver();
                    }

                    return false;
                }
            }
        }

        Debug.Log($"⚠️ {cleanedName} is not needed.");
        return false; // ❌ 필요 없는 재료
    }

    public ItemData GetTarget()
    {
        return target;
    }
}