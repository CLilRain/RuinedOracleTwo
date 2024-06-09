using UnityEngine;

[CreateAssetMenu(fileName = "new card" , menuName = "Cards / card")]
public class Cards_SO : ScriptableObject
{
    [SerializeField] private cardType typeOfcard; 

    public cardType TypeOfCard
    {
        get { return typeOfcard; }
        set { typeOfcard = value; }
    }

    [SerializeField] private string cardName;

    public string CardName
    {
        get { return cardName; }
    }

    [SerializeField] private Sprite cardImg;

    public Sprite CardIMG
    {
        get { return cardImg; }
    }

    [SerializeField] private Sprite smallImg;

    public Sprite smallIMG
    {
        get { return smallImg; }
    }

    [TextArea(2 , 5)]
    [SerializeField] private string description;

    public string Description
    {
        get { return description; }
    }

    [SerializeField] private int attackDamage;

    public int AttackDamage
    {
        get { return attackDamage; }
        set { attackDamage = value; }
    }

    [SerializeField] private int manaCost;

    public int ManaCost
    {
        get { return manaCost; }
    }

    [SerializeField] private GameObject cardAttackEffect;

    public GameObject CardAttackEffect
    {
        get { return cardAttackEffect; }
    }

}

public enum cardType
{
    Hatred,
    Mine,
    Reset,
    Sap,
    ThirdTimeTheCharm
}