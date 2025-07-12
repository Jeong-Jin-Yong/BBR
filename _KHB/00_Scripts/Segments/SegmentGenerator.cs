using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SegmentPattern
{
    public string patternName;
    public int[] segmentIndices; // 세그먼트 프리팹 인덱스 배열
    public int difficulty; // 1(쉬움) ~ 5(어려움)
    public float weight = 1f; // 패턴 선택 확률 가중치
}

public class SegmentGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    public float segmentWidth = 10f;
    public int maxActiveSegments = 3;
    public float patternSpawnInterval = 2f; // 패턴 생성 간격 (초)

    public float spawnAheadDistance = 30f;

    [Header("Platform Pattern")]
    public int startPlatformIndex = 0; // 시작 플랫폼 프리팹 인덱스
    public int jumpPlatformIndex = 1; // 점프 플랫폼 프리팹 인덱스
    public int slidingPlatformIndex = 2; // 슬라이딩 플랫폼 프리팹 인덱스
    public int doubleJumpPlatformIndex = 3; // 더블점프 플랫폼 프리팹 인덱스
    public int doubleJumpPlatform2Index = 4; // 더블점프 플랫폼2 프리팹 인덱스
    public int jumpAndSlidingPlatformIndex = 5;

    private Queue<GameObject> activeSegments;
    private Vector3 nextSpawnPosition;
    private float lastSegmentX;
    private bool isFirstSegment = true; // 첫 번째 세그먼트 여부 확인
    private int patternIndex = 0; // 패턴 순서 인덱스
    private int totalSegmentsGenerated = 0; // 총 생성된 세그먼트 수
    private float lastPatternSpawnTime; // 마지막 패턴 생성 시간
    private bool isGeneratingPattern = false; // 패턴 생성 중인지 확인

    [SerializeField] private Transform player; // Player의 위치를 저장
    [SerializeField] private float segmentSpawnDistance = 20f;

    // 점프-슬라이딩-더블점프-점프 패턴
    private int[] platformPattern = { 0, 0, 0, 0 }; // 점프, 슬라이딩, 더블점프, 점프

    public static SegmentGenerator Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeGenerator();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 초기 세그먼트 생성
        GenerateInitialSegments();
        lastPatternSpawnTime = Time.time;
    }

    private void Update()
    {
        // MoveSegments();
        CheckPatternCreation();
    }

    private void InitializeGenerator()
    {
        activeSegments = new Queue<GameObject>();
        nextSpawnPosition = Vector3.zero;
        lastSegmentX = 0f;
        isFirstSegment = true;
        patternIndex = 0;
        totalSegmentsGenerated = 0;
        lastPatternSpawnTime = 0f;
        isGeneratingPattern = false;
    }

    private void GenerateInitialSegments()
    {
        // 시작 플랫폼 생성
        GenerateStartPlatform();
        
        // 초기 패턴들 생성 (maxActiveSegments - 1개만큼)
        for (int i = 0; i < maxActiveSegments - 1; i++)
        {
            GenerateNextPattern();
        }
    }

    private void CheckPatternCreation()
    {
        // 가장 앞에 있는 플랫폼(오른쪽 끝)의 x좌표와 플레이어 사이의 거리 계산
        // 플랫폼이 왼쪽으로 이동하므로, 마지막 생성된 플랫폼의 현재 위치를 확인
        float rightmostPlatformX = GetRightmostPlatformX();
        float distanceFromPlayer = rightmostPlatformX - player.position.x;

        // 가장 앞에 있는 플랫폼이 플레이어로부터 충분히 가까워지면 새로운 패턴 생성
        if (!isGeneratingPattern && distanceFromPlayer <= spawnAheadDistance)
        {
            GenerateNextPattern();
        }
    }

    private float GetRightmostPlatformX()
    {
        float rightmostX = float.MinValue;

        // 활성화된 모든 세그먼트 중에서 가장 오른쪽에 있는 것의 x좌표를 찾음
        foreach (GameObject segment in activeSegments)
        {
            if (segment != null && segment.transform.position.x > rightmostX)
            {
                rightmostX = segment.transform.position.x;
            }
        }

        // 활성 세그먼트가 없으면 마지막 생성 위치 반환
        return rightmostX != float.MinValue ? rightmostX : lastSegmentX;
    }

    private void GenerateNextPattern()
    {
        isGeneratingPattern = true;

        // 패턴의 모든 세그먼트를 순서대로 생성
        for (int i = 0; i < platformPattern.Length; i++)
        {
            GeneratePatternSegment();
        }

        int[] nextPattern = { 1, 2, 3, 1, 1};
        SetPlatformPattern(nextPattern);

        isGeneratingPattern = false;
    }

    private void GenerateStartPlatform()
    {
        GameObject segment = SegmentPool.Instance.GetSpecificSegment(startPlatformIndex);
        if (segment != null)
        {
            segment.transform.position = new Vector3(nextSpawnPosition.x, -5.5f, nextSpawnPosition.z);
            activeSegments.Enqueue(segment);
            nextSpawnPosition += Vector3.right * segmentWidth;
            lastSegmentX = nextSpawnPosition.x;
            totalSegmentsGenerated++;
        }
    }

    private void GeneratePatternSegment()
    {
        // 점프-슬라이딩-더블점프-점프 패턴에서 현재 인덱스 가져오기
        int currentPatternIndex = platformPattern[patternIndex];

        // 해당하는 플랫폼 타입의 인덱스 결정
        int segmentIndex = GetSegmentIndexByPattern(currentPatternIndex);

        GameObject segment = SegmentPool.Instance.GetSpecificSegment(segmentIndex);
        if (segment != null)
        {
            // 가장 오른쪽 플랫폼의 위치를 기준으로 새로운 위치 계산
            float rightmostX = GetRightmostPlatformX();
            Vector3 spawnPosition = new Vector3(rightmostX + segmentWidth, -5.5f, nextSpawnPosition.z);

            segment.transform.position = spawnPosition;
            activeSegments.Enqueue(segment);

            // nextSpawnPosition과 lastSegmentX 업데이트
            nextSpawnPosition.x = spawnPosition.x;
            lastSegmentX = spawnPosition.x;
            totalSegmentsGenerated++;
        }

        // 다음 패턴 인덱스로 이동
        patternIndex = (patternIndex + 1) % platformPattern.Length;
    }

    private int GetSegmentIndexByPattern(int patternIndex)
    {
        switch (patternIndex)
        {
            case 0: // 스타트 플랫폼
                return startPlatformIndex;
            case 1: // 점프
                return jumpPlatformIndex;
            case 2: // 슬라이딩
                return slidingPlatformIndex;
            case 3: // 더블점프
                return doubleJumpPlatformIndex;
            case 4: // 더블점프2
                return doubleJumpPlatform2Index;
            case 5:
                return jumpAndSlidingPlatformIndex;
            default:
                return jumpPlatformIndex; // 기본값
        }
    }

    public void ResetGenerator()
    {
        // 모든 활성 세그먼트 반환
        while (activeSegments.Count > 0)
        {
            GameObject segment = activeSegments.Dequeue();
            SegmentPool.Instance.ReturnSegment(segment);
        }

        // 초기 상태로 리셋
        nextSpawnPosition = Vector3.zero;
        lastSegmentX = 0f;
        isFirstSegment = true;
        patternIndex = 0;
        totalSegmentsGenerated = 0;
        lastPatternSpawnTime = Time.time;
        isGeneratingPattern = false;

        // 초기 세그먼트 다시 생성
        GenerateInitialSegments();
    }

    // 패턴 변경 메서드
    public void SetPlatformPattern(int[] newPattern)
    {
        if (newPattern != null && newPattern.Length > 0)
        {
            platformPattern = newPattern;
            patternIndex = 0; // 패턴 인덱스 리셋
        }
    }

    // PlatformMover에서 호출할 메서드 - activeSegments 큐에서 세그먼트 제거
    public void RemoveSegmentFromQueue(GameObject segment)
    {
        if (activeSegments.Contains(segment))
        {
            // 큐에서 해당 세그먼트를 찾아서 제거
            Queue<GameObject> tempQueue = new Queue<GameObject>();
            bool found = false;
            
            while (activeSegments.Count > 0)
            {
                GameObject currentSegment = activeSegments.Dequeue();
                if (currentSegment == segment && !found)
                {
                    found = true;
                    // 제거된 세그먼트는 tempQueue에 추가하지 않음
                }
                else
                {
                    tempQueue.Enqueue(currentSegment);
                }
            }
            
            // tempQueue의 세그먼트들을 다시 activeSegments에 추가
            while (tempQueue.Count > 0)
            {
                activeSegments.Enqueue(tempQueue.Dequeue());
            }
        }
    }
}