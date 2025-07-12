using UnityEngine;

public class Ingredient : MonoBehaviour
{
    IngredientManager IngredientManager;
    public GameObject dust;
    SpriteRenderer dustRederer;
    Transform magnetTransform;
    bool onMagnet = false;
    float attractTimer = 0.0f;
    float currentSpeed = 0.0f;
    float baseSpeed = 10f;
    float accelerationRate = 16f;
    float maxSpeed = 50f;

    private void Awake()
    {
        IngredientManager = GameObject.Find("GameManager").GetComponent<IngredientManager>();
        magnetTransform = Camera.main.transform.GetChild(0).GetComponent<Transform>();

        dustRederer = dust.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (onMagnet)
        {
            attractTimer += Time.deltaTime; //�ڼ� Ÿ�̸� �ð� ����
            currentSpeed = Mathf.Min(baseSpeed + (accelerationRate * attractTimer), maxSpeed); //�ð��� ���� ���� �ӵ� ����
            transform.position = Vector2.MoveTowards(transform.position, magnetTransform.position, currentSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bool isNeeded = IngredientManager.IngredientCheck(gameObject);

            if (isNeeded)
            {
                onMagnet = true; // �ڼ� ȿ�� ����
            }
            else
            {
                GameObject temp = Instantiate(dust); // ���ʿ��� ���� ��� ����
                temp.transform.position = transform.position;
                Destroy(gameObject);
            }
        }

        if (collision.CompareTag("Magnet"))
        {
            onMagnet = false;
            Destroy(gameObject);
        }
    }
}
