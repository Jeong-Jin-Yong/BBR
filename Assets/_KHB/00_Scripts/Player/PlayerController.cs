using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int jumpCount = 0;
    private int maxJumpCount = 0;
    private bool isGround = true;

    [SerializeField] private Vector2 jumpPower = new Vector2(0f, 1000f);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(jumpPower);
        }
    }
}
