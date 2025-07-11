using UnityEngine;
using System.Collections.Generic;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform player; // 플레이어 Transform 참조

    // 플랫폼간 최대,최소 거리
    private float minDistance = 3.0f;
    private float maxDistance = 6.0f;
    // 플랫폼에 포함될 최대,최소 블록 개수
    private int minBlockCount = 40;
    private int maxBlockCount = 50;

    private Vector3 curPos;
    private float lastPlatformEndX; // 마지막 플랫폼의 끝 X좌표 저장
    private float generateDistance = 20f; // 플레이어 앞에서 이 거리만큼 떨어진 곳에 플랫폼 생성

    private List<Transform> activePlatforms = new List<Transform>(); // 활성화된 플랫폼들 추적
    private List<PlatformInfo> platformInfos = new List<PlatformInfo>(); // 플랫폼 정보들 저장

    private bool isFirst = true;

    // 플랫폼 정보를 저장하는 구조체
    [System.Serializable]
    public struct PlatformInfo
    {
        public Transform firstBlock;
        public Transform lastBlock;
        public int blockCount;

        public PlatformInfo(Transform first, Transform last, int count)
        {
            firstBlock = first;
            lastBlock = last;
            blockCount = count;
        }
    }

    private void Start()
    {
        curPos = startPoint.position;

        // 초기 플랫폼 몇 개 생성
        for (int i = 0; i < 2; i++)
        {
            GeneratePlatform();
        }
    }

    private void Update()
    {
        // 실시간으로 마지막 플랫폼의 끝 위치 업데이트
        UpdateLastPlatformEndPosition();

        // 플랫폼 생성 필요한지 확인
        CheckPlatformGeneration();

        // 화면 밖으로 나간 플랫폼들 정리
        CleanupPlatforms();
    }

    private void UpdateLastPlatformEndPosition()
    {
        // 활성화된 플랫폼 정보들 중에서 가장 오른쪽에 있는 플랫폼의 끝 위치 찾기
        float rightmostX = float.MinValue;

        for (int i = platformInfos.Count - 1; i >= 0; i--)
        {
            if (platformInfos[i].lastBlock != null && platformInfos[i].lastBlock.gameObject.activeInHierarchy)
            {
                float platformEndX = platformInfos[i].lastBlock.position.x + 0.5f; // 블록 크기 고려
                if (platformEndX > rightmostX)
                {
                    rightmostX = platformEndX;
                }
            }
            else
            {
                // 비활성화된 플랫폼 정보 제거
                platformInfos.RemoveAt(i);
            }
        }

        if (rightmostX != float.MinValue)
        {
            lastPlatformEndX = rightmostX;
        }
    }

    private void CheckPlatformGeneration()
    {
        // 마지막 플랫폼의 끝이 플레이어 위치에서 generateDistance만큼 떨어진 곳에 도달하면 새 플랫폼 생성
        if (lastPlatformEndX <= player.position.x + generateDistance)
        {
            GeneratePlatform();
        }
    }

    private void CleanupPlatforms()
    {
        // 활성화된 플랫폼들 중에서 플레이어 뒤로 너무 멀리 간 것들 제거
        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            if (activePlatforms[i] != null && activePlatforms[i].position.x < player.position.x - 15f)
            {
                activePlatforms.RemoveAt(i);
            }
        }
    }

    private void GeneratePlatform()
    {
        // 생성될 플랫폼의 블록 개수와 y축 값 랜덤 설정
        int blockCount = Random.Range(minBlockCount, maxBlockCount + 1);

        float newPlatformStartX;
        float platformY;

        // 첫 번째 플랫폼인 경우 startPoint 위치에서 생성
        if (isFirst)
        {
            newPlatformStartX = startPoint.position.x;
            platformY = startPoint.position.y;
            isFirst = false; // 첫 번째 플랫폼 생성 후 플래그 비활성화
        }
        else
        {
            // 마지막 플랫폼 끝에서 일정 거리 떨어진 곳에 생성
            float distance = Random.Range(minDistance, maxDistance);
            newPlatformStartX = lastPlatformEndX + distance;
            platformY = curPos.y;
        }

        Transform firstBlock = null;
        Transform lastBlock = null;

        for (int i = 0; i < blockCount; i++)
        {
            Vector3 blockPos = new Vector3(newPlatformStartX + i, platformY, 0);
            GameObject block = objectPool.GetObject(blockPos);

            // 첫 번째와 마지막 블록 참조 저장
            if (i == 0)
            {
                firstBlock = block.transform;
                activePlatforms.Add(block.transform);
            }
            if (i == blockCount - 1)
            {
                lastBlock = block.transform;
            }
        }

        // 플랫폼 정보 저장
        if (firstBlock != null && lastBlock != null)
        {
            platformInfos.Add(new PlatformInfo(firstBlock, lastBlock, blockCount));
        }
    }
}