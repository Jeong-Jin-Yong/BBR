using System.Collections;
using Unity.Properties;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    public GameManager gm;

    [SerializeField]
    private float doughSkillDistance = 250f;
    [SerializeField]
    private float breadSkillDistance = 500f;

    [Header("Jump")]
    [SerializeField]
    private float jumpPower = 250f;
    [SerializeField]
    private int jumpMaxCount = 2;
    private int jumpCount;

    [Header("Skill Object")]
    [SerializeField]
    public GameObject skillObject;

    private Vector2 originScale;

    private Rigidbody2D rb;

    [SerializeField]private Animator animator;

    #region Unity Life Style

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        originScale = transform.localScale;
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsSlide", false);
    }

    private void OnDisable()
    {
        //CharacterInfo.Instance.ActivePlayer();
    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch ((int)CharacterInfo.Instance.GetPlayer())
            {
                case -1:
                    return;
                case 0:
                    if (skillObject != null && !skillObject.activeSelf)
                    {
                        ActiveDoughSkill();
                        animator.SetBool("IsAttack", true);
                    }
                    break;
                case 1:
                    if (gm.GetOnFire())
                    {
                        gm.InitOnFire();
                    }
                    break;
                case 2:
                    if (skillObject != null && !skillObject.activeSelf)
                    {
                        ActiveBreadSkill();
                    }
                    break;
                default:
                    break;
            }
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            switch ((int)CharacterInfo.Instance.GetPlayer())
            {
                case -1:
                    return;
                case 0:
                        animator.SetBool("IsAttack", false);
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }

        if (Input.GetMouseButtonDown(0) && !animator.GetBool("IsSlide"))
        {
            Jump();
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("IsSlide", true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("IsSlide", false);
        }
    }

    void Jump()
    {
        if (rb == null) return;

        if (jumpCount < jumpMaxCount)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(this.transform.position.x, jumpPower));
            jumpCount++;
        }
    }

    void ActiveDoughSkill()
    {
        skillObject.SetActive(true);

        var dir = (this.transform.right + this.transform.up).normalized * doughSkillDistance;
        skillObject.GetComponent<Rigidbody2D>().AddForce((Vector2)dir);
        StartCoroutine(ActiveSkillEnd(1, 1.8f));
    }

    void ActiveBreadSkill()
    {
        skillObject.SetActive(true);

        var dir = this.transform.right * breadSkillDistance;
        skillObject.GetComponent<Rigidbody2D>().AddForce((Vector2)dir);
        StartCoroutine(ActiveSkillEnd(3, 2f));
    }

    IEnumerator ActiveSkillEnd(int id, float waiting)
    {
        yield return new WaitForSeconds(waiting);

        if (id == 1)
        {

            var allObjects = Physics2D.OverlapCircleAll(skillObject.transform.position, 2f);
            foreach (var obj in allObjects)
            {
                if (obj.gameObject.CompareTag("Ground")) break;
                obj.gameObject.SetActive(false);
            }
            skillObject.transform.localPosition = new Vector3(4.5f, 4f, 0f);
            skillObject.SetActive(false);
        }
        else if (id == 3)
        {
            skillObject.transform.localPosition = new Vector3(4.5f, 4f, 0f);
            skillObject.SetActive(false);
            // 플라잉 몬스터에서 충돌 감지
        }
    }

    #region Unity Physics Detection

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Obstacle"))
        {
            gm.PlayerHpDecrease(10, "Obstacle");
        }

        if(other.CompareTag("Fire"))
        {
            gm.PlayerHpDecrease(0, "Fire");
        }
    }

#endregion

}
