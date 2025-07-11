using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private int poolSize;
    
    private Queue<GameObject> pool = new Queue<GameObject>(); // 오브젝트 풀링을 위해 큐 선언

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(platformPrefab, this.transform);
            obj.SetActive(false); // 첫 생성 시 비활성화
            pool.Enqueue(obj); // Pool에 삽입
        }
    }

    // Pool에서 오브젝트를 반환
    public GameObject GetObject(Vector3 position)
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(platformPrefab, this.transform);
        obj.transform.position = position;
        obj.SetActive(true);

        // PlatformMover가 있다면 ObjectPool을 넘겨줌
        PlatformMover mover = obj.GetComponent<PlatformMover>();
        if (mover != null)
        {
            mover.Init(this);
        }

        return obj;
    }

    // 오브젝트를 Pool로 반환
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
