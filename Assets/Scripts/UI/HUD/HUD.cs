using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text timePointText;

    [SerializeField]
    protected TMP_Text essenceText;

    [SerializeField]
    protected Image timePointBar;

    [SerializeField]
    protected Image essenceBar;

    protected Turn_UI gameplayUI;

#if UNITY_EDITOR
    protected float currrentTime = 60;
    protected float maxTime = 60;

    protected int currentEssence = 6;
    protected int maxEssence = 6;
#endif

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
        currrentTime -= Time.deltaTime;
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

    public virtual void ClickedEndTurn()
    {
        if (!gameplayUI)
        {
            Debug.Log("Turn_UI (Max's class) is not connected.");
        }

        gameplayUI.onPressedEndTurn();
    }
}