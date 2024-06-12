using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public abstract void HPGain(int amount);
    public abstract void HPLoss(int amount);

    public abstract void EssenceGain(int amount);
    public abstract void EssenceLoss(int amount);

    public abstract void MaxEssenceGain(int amount);
    public abstract void MaxEssenceLoss(int amount);

    public abstract void DrawCards(int amount);

    // TODO: Other effects
}