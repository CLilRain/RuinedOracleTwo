using UnityEngine;

[CreateAssetMenu(fileName = "Sap", 
    menuName = "Scriptable Objects/Create Sap")]
public class Sap : Cards_SO
{
    public int healAmount = 1;

    public override void ApplyCardEffects(Agent host, Agent target)
    {
        host.EssenceLoss(ManaCost);
        host.HPGain(healAmount);
        target.HPLoss(damageToTarget);
    }
}