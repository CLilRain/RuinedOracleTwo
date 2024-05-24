using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class MainMenu_UI : MonoBehaviour
{
    private Animator animator => GetComponent<Animator>();

    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private ParticleSystem clickEffect;
    private ParticleSystem effect;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            effect = Instantiate(clickEffect, mousePosition, clickEffect.transform.rotation);
            
        }
        
        if(effect != null)
            Destroy(effect.gameObject , 2);
    }


    public void onPlayButton()
    {
        //check if you have 4 or 5 cards -- in your collection , go play

        SceneManager.LoadScene(1);  //change later cause prob - scene 1 is going to be collection scene
    }


    public void onCollectionButton()
    {
    }


    public void onSettingButton()
    {
        StartCoroutine(delaySettingsVisuals());
    }


    public void onExitButton()
    {
        Debug.Log("Quit The Game");
        Application.Quit();
    }

    private IEnumerator delaySettingsVisuals()
    {
        animator.SetBool("SHOWMAINPANEL_A", false);

        yield return new WaitForSeconds(.33f);
        
        settingsPanel.SetActive(true);
        settingsPanel.GetComponent<Animator>().SetTrigger("SHOWPANEL_A") ;

        this.gameObject.SetActive(false);
    }
}
