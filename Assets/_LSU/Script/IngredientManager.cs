using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientManager : MonoBehaviour
{
    public GameManager gameManager;

    public ItemData[] itemDatas;
    [SerializeField] ItemData target;

    //private List<Text> ingredientTexts = new List<Text>();
    private Dictionary<string, Text> ingredientTexts = new Dictionary<string, Text>();

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

        // 필요한 재료 매핑
        foreach (var ing in target.ingredients.needIngredients)
        {
            string name = ing.name;
            Transform ingredientObj = currentIngredientGroup.Find(name);
            if (ingredientObj != null)
            {
                Transform textObj = ingredientObj.Find("Text");
                if (textObj != null)
                {
                    Text txt = textObj.GetComponent<Text>();
                    ingredientTexts[name] = txt;
                }
            }
        }

        // 죽은 재료도 매핑
        foreach (var ing in target.ingredients.deadIngredients)
        {
            string name = ing.name;
            Transform ingredientObj = currentIngredientGroup.Find(name);
            if (ingredientObj != null)
            {
                Transform textObj = ingredientObj.Find("Text");
                if (textObj != null)
                {
                    Text txt = textObj.GetComponent<Text>();
                    ingredientTexts[name] = txt;
                }
            }
        }
    }

    private struct IngredientInfo
    {
        public string name;
        public bool isDead;

        public IngredientInfo(string name, bool isDead)
        {
            this.name = name;
            this.isDead = isDead;
        }
    }


    private void UpdateIngredientUI()
    {
        // 수집해야 할 재료
        for (int i = 0; i < target.ingredients.needIngredients.Length; i++)
        {
            string name = target.ingredients.needIngredients[i].name;
            if (ingredientTexts.ContainsKey(name))
            {
                int current = collectedCounts[i];
                int max = target.ingredients.needCounts[i];
                ingredientTexts[name].text = $"{current} / {max}";

                if (current >= max)
                    ingredientTexts[name].color = Color.green;
            }
        }

        // 죽으면 안 되는 재료
        for (int i = 0; i < target.ingredients.deadIngredients.Length; i++)
        {
            string name = target.ingredients.deadIngredients[i].name;
            if (ingredientTexts.ContainsKey(name))
            {
                int max = target.ingredients.deadCounts[i];
                int used = deadCollectedCounts[i];
                int remaining = Mathf.Max(0, max - used);
                ingredientTexts[name].text = $"{remaining}";
                ingredientTexts[name].color = Color.red;
            }
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

                // 수량 달성 여부만 체크하고, 전체 완료 여부는 따로 함수에서 확인
                UpdateIngredientUI();
                CheckAllDoughCollected(); // ← 여기서 전체 완료 검사
                return true;
            }
        }

        // 죽은 재료 검사
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

                    UpdateIngredientUI(); // 즉시 갱신
                    return false;
                }
            }
        }

        Debug.Log($"⚠️ {cleanedName} is not needed.");
        return false;
    }

    private void CheckAllDoughCollected()
    {
        int completeCount = 0;

        for (int i = 0; i < collectedCounts.Length; i++)
        {
            if (collectedCounts[i] >= target.ingredients.needCounts[i])
            {
                completeCount++;
            }
        }

        // 모두 모은 경우
        if (completeCount == target.ingredients.needIngredients.Length)
        {
            gameManager.hasDough1 = true;
            gameManager.hasDough2 = true;
            gameManager.hasDough3 = true;
            gameManager.hasDough4 = true;
            gameManager.hasDough5 = true;
        }

        Debug.Log("✅ All needed ingredients collected! All hasDough flags set.");
    }

    public ItemData GetTarget()
    {
        return target;
    }
}