using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameEnding gameEndingScript;

    [Header("반죽 단계 관련")]
    [SerializeField] GameObject doughGroup; //도우 게임오브젝트 그룹
    [SerializeField] Image doughTimerBar; //도우 타이머 바
    [SerializeField] float doughTime; //도우 타이머 시간
    [SerializeField] float doughTimer; //도우 타이머 시간
    [SerializeField] bool hasDough1 = false; //재료1 다 모았는지 여부
    [SerializeField] bool hasDough2 = false; //재료2 다 모았는지 여부
    [SerializeField] bool hasDough3 = false; //재료3 다 모았는지 여부

    [Header("진행도 관련")]
    [SerializeField] Slider progressSlider; //진행도 슬라이더
    [SerializeField] float progressTime; //진행도 시간 = 최대 플레이 시간
    [SerializeField] float progressTimer; //진행도 타이머
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
    [SerializeField] float fireDamageDuration = 5f; //불 데미지 지속시간


    private void Awake()
    {
        gameEndingScript = GetComponent<GameEnding>();

        doughTimer = doughTime;
        maxHp = 100;
        currentHp = 100;
        hpBar.fillAmount = 1;
    }

    private void Update()
    {
        if (isGameOver)
            return;
        
        if(stageID == 1)
            DoughUpdate();

        ProgressUpdate();

        if (Input.GetKeyDown(KeyCode.A)) CharacterInfo.Instance.ActivePlayer();


        //장애물 데미지 테스트용
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //장애물 피격 시 -10
            PlayerHpDecrease(10, "Obstacle");
        }

        //불 데미지 테스트용
        if (Input.GetKeyDown(KeyCode.Mouse1))
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

    //반죽 업데이트
    private void DoughUpdate()
    {
        doughTimer -= Time.deltaTime;
        doughTimerBar.fillAmount = doughTimer / doughTime;

        if (doughTimerBar.fillAmount <= 0.001f)
        {
            if(!hasDough1 || !hasDough2 || !hasDough3)
            {
                isGameOver = true;
                gameEndingScript.EndGame();
            }
            else
            {
                stageID = 2;
                doughGroup.SetActive(false);
                hpGroup.SetActive(true);
                //스테이지 2로 전환
            }
        }
    }

    //진행도 업데이트
    private void ProgressUpdate()
    {
        progressTimer += Time.deltaTime;
        progressSlider.value = progressTimer / progressTime;

        float progress = progressSlider.value;

        if (progress <= 0.3f)
        {
            stageID = 1;
            if (previousStageID != stageID)
            {
                isStage1 = true;
                previousStageID = stageID;
            }
        }
        else if (progress <= 0.6f)
        {
            stageID = 2;
            if (previousStageID != stageID)
            {
                isStage2 = true;
                CharacterInfo.Instance.ActivePlayer();
                previousStageID = stageID;
            }
        }
        else if (progress < 0.99f)
        {
            stageID = 3;
            if (previousStageID != stageID)
            {
                isStage3 = true;
                CharacterInfo.Instance.ActivePlayer();
                previousStageID = stageID;
            }
        }
        else if (progress <= 0.99f)
        {
            stageID = 4;
            if (previousStageID != stageID)
            {
                gameEndingScript.EndGame();
                previousStageID = stageID;
            }
        }
    }

    //불 데미지
    private void FireDamage()
    {
        currentHp -= fireDamage * Time.deltaTime;
        hpBar.fillAmount = currentHp / maxHp;
        fireDamageDuration -= Time.deltaTime;

        //지속시간 종료
        if(fireDamageDuration <= 0)
        {
            onFire = false;
            fireDamage = 0;
            fireDamageDuration = 0;
        }
    }

    //체력 감소 -> 플레이어  OnTriggerEnter에서 호출
    public void PlayerHpDecrease(float damage, string type)
    {
        if(stageID == 1)
        {
            //재료 잃기?
            //재료 배열 혹은 리스트에서 랜덤으로 해당되는 재료의 갯수를 차감
            //Random.Range(0, );
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
}
