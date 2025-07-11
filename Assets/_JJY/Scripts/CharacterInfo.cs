using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public static CharacterInfo Instance { get; private set; }

    public enum PlayerType
    {
        None = -1,
        Dough = 0,
        OveningBread = 1,
        Bread = 2
    };

    [SerializeField]
    private List<GameObject> playerList;

    private PlayerType curPlayerType;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        curPlayerType = PlayerType.None;
    }

    private void OnEnable()
    {
        ActivePlayer();
    }
    public PlayerType GetPlayer()
    {
        return curPlayerType;
    }

    public void ActivePlayer()
    {
        switch (curPlayerType)
        {
            case PlayerType.None:
                curPlayerType = PlayerType.Dough;
                playerList[0].SetActive(true);
                break;
            case PlayerType.Dough:
                curPlayerType = PlayerType.OveningBread;
                playerList[0].SetActive(false);
                playerList[1].SetActive(true);
                break;
            case PlayerType.OveningBread:
                curPlayerType = PlayerType.Bread;
                playerList[1].SetActive(false);
                playerList[2].SetActive(true);
                break;
            default:
                curPlayerType = PlayerType.None;
                playerList[2].SetActive(false);
                break;
        }
    }
}
