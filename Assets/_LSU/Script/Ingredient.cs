using UnityEngine;

public class Ingredient : MonoBehaviour
{
    IngredientManager IngredientManager;

    private void Awake()
    {
        IngredientManager = GameObject.Find("GameManager").GetComponent<IngredientManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            IngredientManager.IngredientCheck(gameObject);
            
    }
}
