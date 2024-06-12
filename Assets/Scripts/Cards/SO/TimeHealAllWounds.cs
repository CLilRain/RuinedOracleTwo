using UnityEngine;

[CreateAssetMenu(fileName = "TimeHealAllWounds", 
    menuName = "Scriptable Objects/Create TimeHealAllWounds")]
public class TimeHealAllWounds : Cards_SO
{
    public int healAmount = 4;
    public override void ApplyCardEffects(Agent host, Agent target)
    {
        base.ApplyCardEffects(host, target);

        host.HPGain(healAmount);
    }
}