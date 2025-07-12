using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Ingredient : MonoBehaviour
{
    IngredientManager IngredientManager;

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
            IngredientManager.IngredientCheck(gameObject);
            onMagnet = true;
        }
        if(collision.CompareTag("Magnet"))
        {
            onMagnet = false;
            Destroy(gameObject);
        }
            
    }
}
