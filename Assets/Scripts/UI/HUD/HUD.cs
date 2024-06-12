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

#if UNITY_EDITOR
    protected float currrentTime = 60;
    protected float maxTime = 60;

    protected int currentEssence = 6;
    protected int maxEssence = 6;
#endif

    public virtual void Awake()
    {
        Instance = this;
    }

    protected virtual void Start()
    {
        gameplayUI = FindObjectOfType<Turn_UI>();

#if UNITY_EDITOR
        SetEssence(currentEssence, maxEssence);
#endif
    }

    protected virtual void Update()
    {
#if UNITY_EDITOR
        //currrentTime -= Time.deltaTime;
        SetTimePoints(Mathf.Round(currrentTime), Mathf.Round(maxTime));

        if (Input.GetKeyDown(KeyCode.U))
        {
            currentEssence--;
            SetEssence(currentEssence, maxEssence);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            currentEssence++;
            SetEssence(currentEssence, maxEssence);
        }
#endif
    }

    public virtual void SetTimePoints(int current, int max)
    {
        SetTimePoints(current, max);
    }

    public virtual void SetTimePoints(float current, float max)
    {
        timePointBar.fillAmount = current / max;
        timePointText.text = string.Format("{0} sec", current);
    }

    public virtual void SetEssence(int current, int max)
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
}