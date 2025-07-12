using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    public GameManager gm;

    [SerializeField]
    private int doughMaxSkillCount = 3;
    private int doughCurSkillCount;


    [Header("Jump")]
    [SerializeField]
    private float jumpPower = 250f;
    [SerializeField]
    private int jumpMaxCount = 2;
    private int jumpCount;

    [Header("Skill")]
    [SerializeField]
    public List<GameObject> skillObject;
    [SerializeField]
    private float doughSkillDistance = 400f;
    [SerializeField]
    private float breadSkillDistance = 300f;

    private Rigidbody2D rb;

    [SerializeField]private Animator animator;

    #region Unity Life Style

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        doughCurSkillCount = 0;
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
                    if (skillObject != null && doughCurSkillCount < doughMaxSkillCount && !skillObject[0].activeSelf)
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
                    if (skillObject != null && !skillObject.All(obj => obj.activeSelf))
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
        skillObject[0].SetActive(true);
        doughCurSkillCount++;

        var dir = (this.transform.right + this.transform.up).normalized * doughSkillDistance;
        skillObject[0].GetComponent<Rigidbody2D>().AddForce((Vector2)dir);
        StartCoroutine(ActiveSkillEnd(1, 1.1f));
    }

    void ActiveBreadSkill()
    {
        var pos1 = Vector2.right * breadSkillDistance;
        var pos2 = (Vector2.right * 15 + Vector2.up * 3).normalized * breadSkillDistance;
        var pos3 = (Vector2.right * 2 + Vector2.up).normalized * breadSkillDistance;
        Vector2[] allPos = {pos1, pos2, pos3};
        for (int i = 0; i < allPos.Length; i++)
        {
            skillObject[i].gameObject.SetActive(true);
            skillObject[i].GetComponent<Rigidbody2D>().AddForce(allPos[i]);
        }
        StartCoroutine(ActiveSkillEnd(3, 3f));
    }

    IEnumerator ActiveSkillEnd(int id, float waiting)
    {
        yield return new WaitForSeconds(waiting);

        if (id == 1)
        {
            var allObjects = Physics2D.OverlapCircleAll(skillObject[0].transform.position, 4f);
            foreach (var obj in allObjects)
            {
                if (obj.gameObject.CompareTag("Ground") || obj.gameObject == this.gameObject) continue;
                obj.gameObject.SetActive(false);
            }
            skillObject[0].transform.localPosition = new Vector3(4.5f, 4f, 0f);
            skillObject[0].SetActive(false);
        }
        else if (id == 3)
        {
            foreach (var obj in skillObject)
            {
                obj.transform.localPosition = new Vector3(4.5f, 4f, 0f);
                obj.SetActive(false);
            }
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
