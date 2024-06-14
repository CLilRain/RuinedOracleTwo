using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public static HUD Instance;

    [SerializeField]
    protected TMP_Text timePointText;

    [SerializeField]
    protected Image timePointBar;

    [Space]
    [SerializeField]
    protected TMP_Text essenceText;

    [SerializeField]
    protected Image essenceBar;

    [Space]
    [SerializeField]
    protected Image enemyHPBar;

    [SerializeField]
    protected TMP_Text enemyHPText;

    [Space]
    [SerializeField]
    protected TMP_Text enemyEssenceText;

    protected Turn_UI gameplayUI;

    public virtual void Awake()
    {
        Instance = this;
    }

    protected virtual void Start()
    {
        gameplayUI = FindObjectOfType<Turn_UI>();
    }

    public virtual void SetPlayerTimePoints(float current, float max)
    {
        timePointBar.fillAmount = current / max;
        timePointText.text = string.Format("{0} / {1}", current, max);
    }

    public virtual void SetPlayerEssence(int current, int max)
    {
        essenceBar.fillAmount = (float)current / (float)max;
        essenceText.text = string.Format("{0} / {1}", current, max);
    }

    public virtual void SetEnemyHealth(int current, int max)
    {
        enemyHPBar.fillAmount = (float)current / (float)max;
        enemyHPText.text = string.Format("{0} / {1}", current, max);
    }

    public virtual void SetEnemyEssence(int current, int max)
    {
        enemyEssenceText.text = string.Format("{0} / {1}", current, max);
    }

    public virtual void ClickedEndTurn()
    {
        if (!gameplayUI)
        {
            Debug.Log("Turn_UI (Max's class) is not connected.");
        }

        gameplayUI.onPressedEndTurn();
    }

    public virtual void PressedGameWonButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public virtual void PressedGameLostButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}