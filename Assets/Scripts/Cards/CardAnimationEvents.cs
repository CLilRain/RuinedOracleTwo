using UnityEngine;

public class CardAnimationEvents : MonoBehaviour
{
    private Card card => GetComponent<Card>();

    public void flipToMainSide()
    {
        card.setToMainSide();
    }

    public void flipToBackSide()
    {
        card.setToBackSide();
    }
}
