using UnityEngine;

[CreateAssetMenu(fileName = "Hatred", 
    menuName = "Scriptable Objects/Create Hatred")]
public class Hatred : Cards_SO
{
    [Space]
    public int damageToSelf;

    public override void ApplyCardEffects(Agent host, Agent target)
    {
        base.ApplyCardEffects(host, target);

        target.HPLoss(DamageToTarget);
        host.HPLoss(damageToSelf);
    }
}