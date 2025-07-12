using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ItemGenerator : MonoBehaviour
{
    public GameManager gm;

    [SerializeField]
    private List<GameObject> itemSpawnpoint;

    [Header("Item Generate Time")]
    [SerializeField]
    private float itemSpawnInterval = 1f;


    private void OnEnable()
    {
        if (itemSpawnpoint == null) return;

        StartCoroutine(GenerateItem());
    }

    private IEnumerator GenerateItem()
    {
        while(gm.stageID != 4)
        {
            for (int i = 0; i < itemSpawnpoint.Count; i++)
            {
                if (IsValid(itemSpawnpoint[i].transform))
                {
                    // 아이템 스폰
                    Debug.Log("Item Spawned");
                }
            }

            yield return new WaitForSeconds(itemSpawnInterval);
        }
    }

    private bool IsValid(Transform itemSpawnpoint)
    {
        var allObjects = Physics2D.OverlapBoxAll(itemSpawnpoint.position, new Vector2(1f, 1f), 0f);
        if (allObjects.Length > 1)
        {
            return false;
        }
        return true;
    }
}
