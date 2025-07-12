using UnityEngine;
using System.Collections.Generic;

public class BreadSkillManager : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Destroy(collision.gameObject, 0.05f);
            anim.SetBool("isColl", true);
        }
    }
}
