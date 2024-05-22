using TMPro;
using UnityEngine;

public class CardDetailBox : MonoBehaviour
{
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardDescriptionText;

    public void setCardDetailsBox(Cards_SO _cardData)
    {
        cardNameText.text = _cardData.CardName;
        cardDescriptionText.text = _cardData.Description;
    }
}
