using UnityEngine;

[CreateAssetMenu(fileName = "Mine", 
    menuName = "Scriptable Objects/Create Mine")]
public class Mine : Cards_SO
{
    public int maxEssenceStealAmount = 1;
    public override void ApplyCardEffects(Agent host, Agent target)
    {
        base.ApplyCardEffects(host, target);

        host.MaxEssenceGain(maxEssenceStealAmount);
    }
}