using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public int essence;
    public int maxessence;

    protected HUD hud;

    protected virtual void Awake()
    {
        // Temporary values
        health = maxHealth = 100;
        essence = maxessence = 100;
    }

    protected virtual void Start()
    {
        hud = HUD.Instance;
        UpdateHealthUI();
        UpdateEssenceUI();
    }

    public virtual void HPGain(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        UpdateHealthUI();
    }

    public virtual void HPLoss(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        UpdateHealthUI();
    }

    public virtual void EssenceGain(int amount)
    {
        essence = Mathf.Clamp(essence + amount, 0, maxessence);
        UpdateEssenceUI();
    }

    public virtual void EssenceLoss(int amount)
    {
        essence = Mathf.Clamp(essence - amount, 0, maxessence);
        UpdateEssenceUI();
    }

    public virtual void MaxEssenceGain(int amount)
    {
        maxessence += amount;
        UpdateEssenceUI();
    }

    public virtual void MaxEssenceLoss(int amount)
    {
        maxessence = Mathf.Clamp(maxessence - amount, 0, maxessence);
        UpdateEssenceUI();
    }

    public virtual void DrawCards(int amount)
    {
        // TODO
    }

    // TODO: Other effects
    protected virtual void UpdateHealthUI()
    {
        hud.SetTimePoints(health, maxHealth);
    }

    protected virtual void UpdateEssenceUI()
    {
        hud.SetEssence(essence, maxessence);
    }
}