using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private int poolSize;
    
    private Queue<GameObject> pool = new Queue<GameObject>(); // ������Ʈ Ǯ���� ���� ť ����

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(platformPrefab, this.transform);
            obj.SetActive(false); // ù ���� �� ��Ȱ��ȭ
            pool.Enqueue(obj); // Pool�� ����
        }
    }

    // Pool���� ������Ʈ�� ��ȯ
    public GameObject GetObject(Vector3 position)
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(platformPrefab, this.transform);
        obj.transform.position = position;
        obj.SetActive(true);

        // PlatformMover�� �ִٸ� ObjectPool�� �Ѱ���
        PlatformMover mover = obj.GetComponent<PlatformMover>();
        if (mover != null)
        {
            mover.Init(this);
        }

        return obj;
    }

    // ������Ʈ�� Pool�� ��ȯ
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
