using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    GameObject GameWonScreen;

    [SerializeField] 
    GameObject GameLostScreen;

    protected void Awake()
    {
        Instance = this;
    }

    public void ShowGameLost()
    {
        GameLostScreen.SetActive(true);
    }

    public void ShowGameWon()
    {
        GameWonScreen.SetActive(true);
    }
}
