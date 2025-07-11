using System.Collections.Generic;
using UnityEngine;

public class SegmentPool : MonoBehaviour
{
    [Header("Segment Prefabs")]
    public GameObject[] segmentPrefabs;

    [Header("Pool Settings")]
    public int poolSize = 10;

    private Queue<GameObject> segmentPool;
    private List<GameObject> activeSegments;
    private Dictionary<int, Queue<GameObject>> typedPools; // 타입별 풀 관리

    public static SegmentPool Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        segmentPool = new Queue<GameObject>();
        activeSegments = new List<GameObject>();
        typedPools = new Dictionary<int, Queue<GameObject>>();

        // 풀 초기화 - 각 세그먼트 프리팹 타입별로 별도 풀 생성
        for (int i = 0; i < poolSize; i++)
        {
            for (int prefabIndex = 0; prefabIndex < segmentPrefabs.Length; prefabIndex++)
            {
                GameObject segment = Instantiate(segmentPrefabs[prefabIndex]);
                segment.SetActive(false);
                segment.transform.SetParent(transform);
                
                // 타입별 풀에 추가
                if (!typedPools.ContainsKey(prefabIndex))
                {
                    typedPools[prefabIndex] = new Queue<GameObject>();
                }
                typedPools[prefabIndex].Enqueue(segment);
            }
        }
    }

    public GameObject GetSegment()
    {
        GameObject segment = null;

        if (segmentPool.Count > 0)
        {
            segment = segmentPool.Dequeue();
        }
        else
        {
            // 풀에 여유가 없으면 새로 생성
            segment = Instantiate(segmentPrefabs[Random.Range(0, segmentPrefabs.Length)]);
        }

        segment.SetActive(true);
        activeSegments.Add(segment);
        return segment;
    }

    public GameObject GetSpecificSegment(int prefabIndex)
    {
        if (prefabIndex < 0 || prefabIndex >= segmentPrefabs.Length)
        {
            Debug.LogWarning("Invalid prefab index: " + prefabIndex);
            return GetSegment();
        }

        GameObject segment = null;

        // 해당 타입의 풀에서 가져오기
        if (typedPools.ContainsKey(prefabIndex) && typedPools[prefabIndex].Count > 0)
        {
            segment = typedPools[prefabIndex].Dequeue();
        }
        else
        {
            // 해당 타입의 풀에 없으면 새로 생성
            segment = Instantiate(segmentPrefabs[prefabIndex]);
        }

        segment.SetActive(true);
        activeSegments.Add(segment);
        return segment;
    }

    public void ReturnSegment(GameObject segment)
    {
        if (segment != null && activeSegments.Contains(segment))
        {
            segment.SetActive(false);
            activeSegments.Remove(segment);
            
            // 세그먼트의 타입을 찾아서 해당 풀에 반환
            int segmentType = GetSegmentType(segment);
            if (typedPools.ContainsKey(segmentType))
            {
                typedPools[segmentType].Enqueue(segment);
            }
            else
            {
                // 타입을 찾을 수 없으면 일반 풀에 반환
                segmentPool.Enqueue(segment);
            }
        }
    }

    private int GetSegmentType(GameObject segment)
    {
        // 세그먼트의 이름을 기반으로 프리팹 인덱스 찾기
        for (int i = 0; i < segmentPrefabs.Length; i++)
        {
            if (segment.name.Contains(segmentPrefabs[i].name))
            {
                return i;
            }
        }
        return 0; // 기본값
    }

    public void ReturnAllSegments()
    {
        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            ReturnSegment(activeSegments[i]);
        }
    }

    public int GetActiveSegmentCount()
    {
        return activeSegments.Count;
    }

    public List<GameObject> GetActiveSegments()
    {
        return new List<GameObject>(activeSegments);
    }

    // 특정 타입의 세그먼트 개수 반환
    public int GetSegmentTypeCount(int prefabIndex)
    {
        if (typedPools.ContainsKey(prefabIndex))
        {
            return typedPools[prefabIndex].Count;
        }
        return 0;
    }
}