using UnityEngine;
using System.Collections;

public class Settings_UI : MonoBehaviour
{
    private Animator animator => GetComponent<Animator>();

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject soundSettingPanel;
    [SerializeField] private GameObject graphicSettingPanel;



    public void onSoundSettingButton()
    {
        StartCoroutine(delaySoundSettingsVisuals());
    }

    public void onGraphicSettingButton()
    {
        StartCoroutine(delayGraphicPanelVisuals());
    }

    public void onBackButton()
    {
        StartCoroutine(delayMainPanelVisuals());
    }

    private IEnumerator delayMainPanelVisuals()
    {
        animator.SetTrigger("CLOSEPANEL_A");

        yield return new WaitForSeconds(.33f);

        this.gameObject.SetActive(false);
        mainPanel.SetActive(true);
    }

    private IEnumerator delaySoundSettingsVisuals()
    {
        animator.SetTrigger("CLOSEPANELBYFLIP_A");

        yield return new WaitForSeconds(.083f);

        this.gameObject.SetActive(false);
        soundSettingPanel.SetActive(true);
    }

    private IEnumerator delayGraphicPanelVisuals()
    {
        animator.SetTrigger("CLOSEPANELBYFLIP_A");

        yield return new WaitForSeconds(.083f);

        this.gameObject.SetActive(false);
        graphicSettingPanel.SetActive(true);
    }
}
