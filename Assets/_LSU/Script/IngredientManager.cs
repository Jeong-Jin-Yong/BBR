using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public GameManager gameManager;

    public ItemData[] itemDatas;
    ItemData target;

    // 수집된 재료 개수를 추적하는 배열
    int[] collectedCounts;
    int[] deadCollectedCounts;

    private void Start()
    {
        target = itemDatas[Random.Range(0, itemDatas.Length)];
        collectedCounts = new int[target.ingredients.needIngredients.Length];
        deadCollectedCounts = new int[target.ingredients.deadIngredients.Length];
    }

    public void IngredientCheck(GameObject obj)
    {
        var ingredients = target.ingredients;

        // 필요한 재료 검사
        for (int i = 0; i < ingredients.needIngredients.Length; i++)
        {
            if (obj.name == ingredients.needIngredients[i].name)
            {
                collectedCounts[i]++;
                Debug.Log($"Collected {ingredients.needIngredients[i].name}: {collectedCounts[i]}/{ingredients.needCounts[i]}");

                if (collectedCounts[i] >= ingredients.needCounts[i])
                {
                    Debug.Log($"{ingredients.needIngredients[i].name} is fully collected!");

                    switch (i)
                    {
                        case 0: gameManager.hasDough1 = true; break;
                        case 1: gameManager.hasDough2 = true; break;
                        case 2: gameManager.hasDough3 = true; break;
                        default: Debug.LogWarning("No matching hasDough variable for index " + i); break;
                    }
                }

                return;
            }
        }

        // ❗️죽은 재료 검사
        for (int i = 0; i < ingredients.deadIngredients.Length; i++)
        {
            if (obj.name == ingredients.deadIngredients[i].name)
            {
                deadCollectedCounts[i]++;
                Debug.LogWarning($"❌ Wrong ingredient eaten: {ingredients.deadIngredients[i].name} - {deadCollectedCounts[i]}/{ingredients.deadCounts[i]}");

                // 누적이 기준 이상이면 게임오버
                if (deadCollectedCounts[i] >= ingredients.deadCounts[i])
                {
                    Debug.LogError($"{ingredients.deadIngredients[i].name} was collected too much! Game Over.");
                    gameManager.GameOver(); // GameManager에서 게임 종료 처리
                }

                return;
            }
        }

        Debug.Log("This item is not needed.");
    }


}
