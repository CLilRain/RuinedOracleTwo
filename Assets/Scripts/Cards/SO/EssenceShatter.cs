using UnityEngine;

[CreateAssetMenu(fileName = "EssenceShatter", 
    menuName = "Scriptable Objects/Create EssenceShatter")]
public class EssenceShatter : Cards_SO
{
    public int maxEssenceStealAmount = 2;
    public override void ApplyCardEffects(Agent host, Agent target)
    {
        base.ApplyCardEffects(host, target);

        host.MaxEssenceGain(maxEssenceStealAmount);
        target.MaxEssenceLoss(maxEssenceStealAmount);
    }
}