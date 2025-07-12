using UnityEngine;

public class Dust : MonoBehaviour
{
    [SerializeField] SpriteRenderer dustRenderer;

    private void Update()
    {
        if(dustRenderer.color.a == 0)
        {
            Destroy(gameObject);
        }
    }
}
