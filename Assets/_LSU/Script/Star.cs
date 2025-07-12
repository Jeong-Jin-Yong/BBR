using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    public GameObject starGroup;
    public Animator starAnimator;
    public Image starImage;
    [SerializeField]bool on_;
    [SerializeField]float timer = 0.0f;
    [SerializeField] float time = 0.1f;

    private void Start()
    {
        starGroup.SetActive(false);
    }

    private void Update()
    {
        if (on_)
        {
            timer += Time.deltaTime;

            if (timer >= time)
            {
                on_ = false;
                timer = 0.0f;

                // �ִϸ��̼� ���� �� ������Ʈ ����
                starGroup.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ingredient"))
        {
            on_ = true;
            timer = 0f;

            starGroup.SetActive(true);
            starAnimator.Play("StarUIAnim", -1, 0f); // �ִϸ��̼� �̸��� ��Ȯ�� �Է�
        }
    }
}
