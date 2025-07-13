using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameEnding gameEndingScript;

    [Header("반죽 단계 관련")]
    [SerializeField] GameObject doughGroup; //도우 게임오브젝트 그룹
    [SerializeField] GameObject memoGroup;
    [SerializeField] Image doughTimerBar; //도우 타이머 바
    [SerializeField] float doughTime; //도우 타이머 시간
    [SerializeField] float doughTimer; //도우 타이머 시간
    public bool hasDough1 = false; //재료1 다 모았는지 여부
    public bool hasDough2 = false; //재료2 다 모았는지 여부
    public bool hasDough3 = false; //재료3 다 모았는지 여부
    public bool hasDough4 = false;
    public bool hasDough5 = false;

    [Header("진행도 관련")]
    [SerializeField] Slider progressSlider; //진행도 슬라이더
    [SerializeField] float progressTime; //진행도 시간 = 최대 플레이 시간
    [SerializeField] float progressTimer; //진행도 타이머
    [SerializeField] Animator animator; //진행도 애니메이터
    public int stageID = 1; //스테이지 1, 2, 3
    bool isStage1 = false;
    bool isStage2 = false;
    bool isStage3 = false;
    bool isGameOver = false; //게임종료 여부
    private int previousStageID = 0;

    [Header("체력 및 데미지 관련")]
    [SerializeField] GameObject hpGroup; //체력 게임오브젝트 그룹
    [SerializeField] Image hpBar; //체력바 이미지
    [SerializeField] float maxHp; //최대 체력
    [SerializeField] float currentHp; //현재 체력
    [SerializeField] bool onFire = false; //불 활성화 여부
    [SerializeField] float fireDamage = 0f; //불 데미지
    [SerializeField] public float fireDamageDuration = 5f; //불 데미지 지속시간

    public AudioSource bgm;
    public AudioClip bgm1;
    public AudioClip bgm2;
    public AudioClip bgm3;

    public GameObject setting;

    [Header("스테이지 플랫폼 설정")]
    [SerializeField] GameObject[] stage1Prefabs;
    [SerializeField] GameObject[] stage2Prefabs;
    [SerializeField] GameObject[] stage3Prefabs;

    [Header("스테이지 패턴 설정")]
    [SerializeField] int[] stage1Patterns = { 0, 0, 0 };
    [SerializeField] int[] stage2Patterns = { 0, 0, 0 };
    [SerializeField] int[] stage3Patterns = { 0, 0, 0 };

    private void Awake()
    {
        gameEndingScript = GetComponent<GameEnding>();

        fireDamageDuration = 0f;
        doughTimer = doughTime;
        maxHp = 100;
        currentHp = 100;
        hpBar.fillAmount = 1;

        //// 최초 패턴 랜덤 설정
        //RandomizeStagePatterns();

        //// 이후 20초마다 패턴 변경
        //InvokeRepeating(nameof(RandomizeStagePatterns), 20f, 20f);
    }

    //private void OnEnable()
    //{
    //    curBread = breadList[Random.Range(0, 3)];
    //    Debug.Log($"Misson: {curBread}");
    //}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            setting.SetActive(true);
        }

        if (isGameOver)
            return;

        if (stageID == 1)
            DoughUpdate();

        ProgressUpdate();

        if (Input.GetKeyDown(KeyCode.A)) CharacterInfo.Instance.ActivePlayer();


        //장애물 데미지 테스트용
        if (Input.GetKeyDown(KeyCode.W))
        {
            //장애물 피격 시 -10
            PlayerHpDecrease(10, "Obstacle");
        }

        //불 데미지 테스트용
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //불 데미지 실행
            PlayerHpDecrease(0, "Fire");
        }

        //불 데미지 실행
        if (onFire && fireDamageDuration >= 0)
        {
            FireDamage();
        }

        //게임오버
        if (currentHp <= 0 && !isGameOver)
        {
            isGameOver = true;
            gameEndingScript.EndGame();
        }
    }

    //private void RandomizeStagePatterns()
    //{
    //    if (stageID == 1) stage1Patterns = GenerateRandomPattern(5, 0, 6);
    //    else if (stageID == 2) stage2Patterns = GenerateRandomPattern(5, 0, 6);
    //    else if (stageID == 3) stage3Patterns = GenerateRandomPattern(5, 0, 6);

    //    Debug.Log("Stage 패턴이 변경되었습니다.");
    //}

    //private int[] GenerateRandomPattern(int length, int minValue, int maxExclusive)
    //{
    //    int[] pattern = new int[length];
    //    for (int i = 0; i < length; i++)
    //    {
    //        pattern[i] = Random.Range(minValue, maxExclusive); // min 이상 max 미만
    //    }
    //    return pattern;
    //}

    //반죽 업데이트
    private void DoughUpdate()
    {
        doughTimer -= Time.deltaTime;
        doughTimerBar.fillAmount = doughTimer / doughTime;

        if (doughTimerBar.fillAmount <= 0.001f)
        {
            isGameOver = true;
            gameEndingScript.EndGame();
        }
    }

    //진행도 업데이트
    private void ProgressUpdate()
    {
        progressTimer += Time.deltaTime;
        progressSlider.value = progressTimer / progressTime;

        float progress = progressSlider.value;

        if (progress <= 0.33f)
        {
            stageID = 1;
            if (previousStageID != stageID)
            {
                isStage1 = true;
                previousStageID = stageID;
                ChangeStage(1);
                bgm.clip = bgm1;
            }
        }
        else if (progress <= 0.66f)
        {
            stageID = 2;
            if (previousStageID != stageID)
            {
                memoGroup.SetActive(false);
                doughGroup.SetActive(false);
                hpGroup.SetActive(true);

                isStage2 = true;
                animator.SetInteger("StageID", 2);
                CharacterInfo.Instance.ActivePlayer();
                previousStageID = stageID;
                ChangeStage(2);
                bgm.clip = bgm2;

            }
        }
        else if (progress < 0.99f)
        {
            stageID = 3;
            if (previousStageID != stageID)
            {
                isStage3 = true;
                animator.SetInteger("StageID", 3);
                CharacterInfo.Instance.ActivePlayer();
                previousStageID = stageID;
                ChangeStage(3);
                bgm.clip = bgm3;
            }
        }
        else if (progress >= 0.99f)
        {
            stageID = 4;
            if (previousStageID != stageID)
            {
                gameEndingScript.EndGame();
                previousStageID = stageID;
            }
        }
    }


    private void ChangeStage(int newStageID)
    {
        GameObject[] newPrefabs = null;
        int[] newPattern = null;

        switch (newStageID)
        {
            case 1:
                newPrefabs = stage1Prefabs;
                newPattern = stage1Patterns;
                break;
            case 2:
                newPrefabs = stage2Prefabs;
                newPattern = stage2Patterns;
                break;
            case 3:
                newPrefabs = stage3Prefabs;
                newPattern = stage3Patterns;
                break;
            default:
                return;
        }

        if (newPrefabs != null && newPrefabs.Length > 0)
        {
            SegmentPool.Instance.ChangeStagePrefabs(newPrefabs);

            SegmentGenerator.Instance.ResetGenerator();

            if (newPattern != null && newPattern.Length > 0)
            {
                SegmentGenerator.Instance.SetPlatformPattern(newPattern);
            }
        }

        Debug.Log("Change to Stage" + newStageID);
    }

    //불 데미지
    private void FireDamage()
    {
        currentHp -= fireDamage * Time.deltaTime;
        hpBar.fillAmount = currentHp / maxHp;
        fireDamageDuration -= Time.deltaTime;

        //지속시간 종료
        if (fireDamageDuration <= 0)
        {
            InitOnFire();
        }
    }

    //체력 감소 -> 플레이어  OnTriggerEnter에서 호출
    public void PlayerHpDecrease(float damage, string type)
    {
        if (stageID == 1)
        {
            doughTimer -= damage;
            doughTimerBar.fillAmount = doughTimer / doughTime;
        }

        if (stageID != 1)
        {
            if (type == "Obstacle")
            {
                currentHp -= damage;
                hpBar.fillAmount = currentHp / maxHp;
            }
            else if (type == "Fire")
            {
                onFire = true;
                fireDamage += 2.0f;
                fireDamageDuration = 5.0f;
            }
        }

    }

    //플레이어가 낙하했을 경우에 바로 게임오버.
    //해당 함수를 사용할 필요없이 데미지 함수에서 데미지의 크기를 키워서 줘도 됨.
    public void PlayerDroped()
    {
        isGameOver = true;
        gameEndingScript.EndGame();
    }

    public bool GetOnFire()
    {
        return onFire;
    }

    public void SetOnFire(float duration)
    {
        fireDamageDuration = duration;
    }

    public void InitOnFire()
    {
        onFire = false;
        fireDamage = 0;
        fireDamageDuration = 0;
    }

    public bool AllIngredientsReady()
    {
        return hasDough1 && hasDough2 && hasDough3;
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        gameEndingScript.EndGame();
    }

    public void GameRestart()
    {
        SceneManager.LoadScene("Order");
    }

    public void Roby()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
