using System.Collections;
using UnityEngine;
using TMPro;


public class GraphicSetting_UI : MonoBehaviour
{
    private Animator animator => GetComponent<Animator>();

    [SerializeField] private TMP_Dropdown dropDown;
    [SerializeField] private GameObject settingsPanel;


    public void onDropDownValueChange()
    {
        QualitySettings.SetQualityLevel(dropDown.value);
    }

    public void onBackButton()
    {
        StartCoroutine(delaySettingsPanelVisuals());
    }

    private IEnumerator delaySettingsPanelVisuals()
    {
        animator.SetTrigger("CLOSE_A");

        yield return new WaitForSeconds(.167f);

        settingsPanel.SetActive(true);
        settingsPanel.GetComponent<Animator>().SetTrigger("OPENPANELBYFLIP_A");

        this.gameObject.SetActive(false);
    }
}
