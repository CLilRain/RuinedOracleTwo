using UnityEngine;

[CreateAssetMenu(fileName = "Sap", 
    menuName = "Scriptable Objects/Create Sap")]
public class Sap : Cards_SO
{
    public override void ApplyCardEffects(Agent host, Agent target)
    {
        base.ApplyCardEffects(host, target);

        // Heals the same amount as damage done
        host.HPGain(damageToTarget);
    }
}