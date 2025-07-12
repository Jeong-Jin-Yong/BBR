using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CharacterSelector : MonoBehaviour
{
    public GameManager gm;
    public IngredientManager im;

    [SerializeField]
    private List<GameObject> stage2Chars;
    [SerializeField]
    private List<GameObject> stage3Chars;

    private void OnEnable()
    {
        if (gm == null || im == null) return;

        if (gm.stageID == 2)
        {
            if (stage2Chars == null) return;
            Debug.Log(im.GetTarget().name);
            
            foreach (var character in stage2Chars)
            {
                if (character.name == im.GetTarget().name)
                {
                    character.SetActive(true);
                }
            }
        }
        else if (gm.stageID == 3)
        {
            if (stage3Chars == null) return;

            foreach (var character in stage3Chars)
            {
                if (character.name == im.GetTarget().name)
                {
                    character.SetActive(true);
                }
            }
        }
    }
}
