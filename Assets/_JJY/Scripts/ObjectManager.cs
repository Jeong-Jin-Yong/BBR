using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        transform.Translate(Vector3.left * 5.4f * Time.deltaTime);
    }
}
