using UnityEngine;
using System.Collections.Generic;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform player; // �÷��̾� Transform ����

    // �÷����� �ִ�,�ּ� �Ÿ�
    private float minDistance = 3.0f;
    private float maxDistance = 6.0f;
    // �÷����� ���Ե� �ִ�,�ּ� ��� ����
    private int minBlockCount = 40;
    private int maxBlockCount = 50;

    private Vector3 curPos;
    private float lastPlatformEndX; // ������ �÷����� �� X��ǥ ����
    private float generateDistance = 20f; // �÷��̾� �տ��� �� �Ÿ���ŭ ������ ���� �÷��� ����

    private List<Transform> activePlatforms = new List<Transform>(); // Ȱ��ȭ�� �÷����� ����
    private List<PlatformInfo> platformInfos = new List<PlatformInfo>(); // �÷��� ������ ����

    private bool isFirst = true;

    // �÷��� ������ �����ϴ� ����ü
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

        // �ʱ� �÷��� �� �� ����
        for (int i = 0; i < 2; i++)
        {
            GeneratePlatform();
        }
    }

    private void Update()
    {
        // �ǽð����� ������ �÷����� �� ��ġ ������Ʈ
        UpdateLastPlatformEndPosition();

        // �÷��� ���� �ʿ����� Ȯ��
        CheckPlatformGeneration();

        // ȭ�� ������ ���� �÷����� ����
        CleanupPlatforms();
    }

    private void UpdateLastPlatformEndPosition()
    {
        // Ȱ��ȭ�� �÷��� ������ �߿��� ���� �����ʿ� �ִ� �÷����� �� ��ġ ã��
        float rightmostX = float.MinValue;

        for (int i = platformInfos.Count - 1; i >= 0; i--)
        {
            if (platformInfos[i].lastBlock != null && platformInfos[i].lastBlock.gameObject.activeInHierarchy)
            {
                float platformEndX = platformInfos[i].lastBlock.position.x + 0.5f; // ��� ũ�� ���
                if (platformEndX > rightmostX)
                {
                    rightmostX = platformEndX;
                }
            }
            else
            {
                // ��Ȱ��ȭ�� �÷��� ���� ����
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
        // ������ �÷����� ���� �÷��̾� ��ġ���� generateDistance��ŭ ������ ���� �����ϸ� �� �÷��� ����
        if (lastPlatformEndX <= player.position.x + generateDistance)
        {
            GeneratePlatform();
        }
    }

    private void CleanupPlatforms()
    {
        // Ȱ��ȭ�� �÷����� �߿��� �÷��̾� �ڷ� �ʹ� �ָ� �� �͵� ����
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
        // ������ �÷����� ��� ������ y�� �� ���� ����
        int blockCount = Random.Range(minBlockCount, maxBlockCount + 1);

        float newPlatformStartX;
        float platformY;

        // ù ��° �÷����� ��� startPoint ��ġ���� ����
        if (isFirst)
        {
            newPlatformStartX = startPoint.position.x;
            platformY = startPoint.position.y;
            isFirst = false; // ù ��° �÷��� ���� �� �÷��� ��Ȱ��ȭ
        }
        else
        {
            // ������ �÷��� ������ ���� �Ÿ� ������ ���� ����
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

            // ù ��°�� ������ ��� ���� ����
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

        // �÷��� ���� ����
        if (firstBlock != null && lastBlock != null)
        {
            platformInfos.Add(new PlatformInfo(firstBlock, lastBlock, blockCount));
        }
    }
}