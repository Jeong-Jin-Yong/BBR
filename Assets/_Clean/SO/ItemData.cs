using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [System.Serializable]
    public class Ingredients
    {
        public string name;
        public GameObject[] spawnIngredients;
        public GameObject[] needIngredients;
        public GameObject[] deadIngredients;
    }

    public Ingredients ingredients;
}
