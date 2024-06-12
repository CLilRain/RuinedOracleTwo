using UnityEngine;

[CreateAssetMenu(fileName = "new card" , menuName = "Cards / card")]
public class Cards_SO : ScriptableObject
{
    [SerializeField] protected CardTypes typeOfCard; 

    public CardTypes TypeOfCard
    {
        get { return typeOfCard; }
        set { typeOfCard = value; }
    }

    [SerializeField] protected string cardName;

    public string CardName
    {
        get { return cardName; }
    }

    [SerializeField] protected Sprite cardImg;

    public Sprite CardIMG
    {
        get { return cardImg; }
    }

    [SerializeField] protected Sprite smallImg;

    public Sprite smallIMG
    {
        get { return smallImg; }
    }

    [TextArea(2 , 5)]
    [SerializeField] protected string description;

    public string Description
    {
        get { return description; }
    }

    [SerializeField] protected int damageToTarget;

    public int DamageToTarget
    {
        get { return damageToTarget; }
        set { damageToTarget = value; }
    }
    
    [SerializeField] protected int manaCost;

    public int ManaCost
    {
        get { return manaCost; }
    }

    [SerializeField] protected GameObject cardAttackEffect;

    public GameObject CardAttackEffect
    {
        get { return cardAttackEffect; }
    }

    public virtual void ApplyCardEffects(Agent host, Agent target)
    {
        target.EssenceLoss(ManaCost);
        target.HPLoss(damageToTarget);
    }
}

public enum CardTypes
{
    Hatred,
    Mine,
    Reset,
    Sap,
    ThirdTimeTheCharm,

    EssenceShatter,
    TimeShock,
    TimesUp,
    Wisdom,
    EssenceBlast,

    TimeHealAllWounds,
}