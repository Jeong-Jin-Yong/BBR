using UnityEngine;

public class FireDetection : MonoBehaviour
{
    private GameManager gm;

    private void OnEnable()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gm.SetOnFire(5f);
        }
    }
}
