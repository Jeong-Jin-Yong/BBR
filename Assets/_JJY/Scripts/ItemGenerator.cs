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
    private float itemSpawnInterval = 5f;

    private ItemData itemData;

    [Header("Stage 1 Item")]
    [SerializeField]
    private List<GameObject> stage1Items;

    [Header("Stage 2 Item")]
    [SerializeField]
    private List<GameObject> stage2Items;


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
                if (gm.stageID == 1 && IsValid(itemSpawnpoint[i].transform))
                {
                    var spawnPos = itemSpawnpoint[Random.Range(0, itemSpawnpoint.Count)].transform.position;
                    var obj = Instantiate(stage1Items[Random.Range(0, stage1Items.Count)], spawnPos, Quaternion.identity);
                    break;
                }
                if (gm.stageID == 2 && IsValid(itemSpawnpoint[i].transform))
                {
                    var spawnPos = itemSpawnpoint[Random.Range(0, itemSpawnpoint.Count)].transform.position;
                    var obj = Instantiate(stage1Items[Random.Range(0, stage1Items.Count)], spawnPos, Quaternion.identity);
                    break;
                }
            }

            yield return new WaitForSeconds(itemSpawnInterval);
        }
    }

    private bool IsValid(Transform itemSpawnpoint)
    {
        var allObjects = Physics2D.OverlapBoxAll(itemSpawnpoint.position, new Vector2(2f, 2f), 0f);
        if (allObjects.Length > 1)
        {
            return false;
        }
        return true;
    }
}
