using UnityEngine;
using System.Collections;


public class SoundSetting_UI : MonoBehaviour
{

    private Animator animator => GetComponent<Animator>();

    [SerializeField] private GameObject settingsPanel;

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
