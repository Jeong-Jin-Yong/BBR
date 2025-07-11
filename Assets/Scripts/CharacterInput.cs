using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    [SerializeField]
    private float jumpPower = 250f;

    [SerializeField]
    private int jumpMaxCount = 2;
    private int jumpCount;

    private Vector2 originScale;

    private Rigidbody2D rb;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        originScale = transform.localScale;
    }

    private void OnDisable()
    {
        CharacterInfo.Instance.ActivePlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseSkill();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Jump();
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            this.transform.localScale = originScale - new Vector2(0, 0.1f);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            this.transform.localScale = originScale;
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

    void UseSkill()
    {
        switch ((int)CharacterInfo.Instance.GetPlayer())
        {
            case -1:
                return;
            case 0:
                Debug.Log($"{CharacterInfo.Instance.GetPlayer()}: Use Skill");
                break;
            case 1:
                Debug.Log($"{CharacterInfo.Instance.GetPlayer()}: Use Skill");
                break;
            case 2:
                Debug.Log($"{CharacterInfo.Instance.GetPlayer()}: Use Skill");
                break;
            default:
                return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
}
