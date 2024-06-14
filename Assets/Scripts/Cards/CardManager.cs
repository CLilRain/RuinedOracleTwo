using UnityEngine;
using System.Collections;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    public bool isCardsBusy {  get; set; }    // this is for -- when card is doing some activity we don't need to selected or drag card so we using this variable

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
        }

        isCardsBusy = true;
    }

    public void delayCardBusy()
    {
        StartCoroutine(delayIsCardBusyToFalse());
    }

    private IEnumerator delayIsCardBusyToFalse()
    {
        yield return new WaitForSeconds(2.5f);
        isCardsBusy = false;
    }
}
