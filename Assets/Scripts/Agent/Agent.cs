using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public int essence;
    public int maxEssence;

    protected bool isDead;

    protected HUD hud;
    protected UIManager uiManager;

    protected virtual void Awake()
    {
        // Temporary values
        health = maxHealth = 8;
        essence = maxEssence = 6;
    }

    protected virtual void Start()
    {
        hud = HUD.Instance;
        uiManager = UIManager.Instance;

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
        DeathCheck();
    }

    public virtual void EssenceGain(int amount)
    {
        essence = Mathf.Clamp(essence + amount, 0, maxEssence);
        UpdateEssenceUI();
    }

    public virtual void EssenceLoss(int amount)
    {
        essence = Mathf.Clamp(essence - amount, 0, maxEssence);
        UpdateEssenceUI();
    }

    public virtual void MaxEssenceGain(int amount)
    {
        if (essence == maxEssence)
        {
            essence = maxEssence;
        }

        maxEssence += amount;
        UpdateEssenceUI();
    }

    public virtual void MaxEssenceLoss(int amount)
    {
        essence = Mathf.Clamp(essence, 0, maxEssence);
        maxEssence = Mathf.Clamp(maxEssence - amount, 0, maxEssence);
        UpdateEssenceUI();
    }

    public virtual void DrawCards(int amount)
    {
        // TODO
    }

    // TODO: Other effects
    protected virtual void UpdateHealthUI()
    {
        hud.SetPlayerTimePoints(health, maxHealth);
    }

    protected virtual void UpdateEssenceUI()
    {
        hud.SetPlayerEssence(essence, maxEssence);
    }

    protected virtual void DeathCheck()
    {
        if (health <= 0)
        {
            AgentDead();
        }
    }

    protected virtual void AgentDead()
    {

    }

    protected virtual void PlayerWon()
    {
        uiManager.ShowGameWon();
    }

    protected virtual void PlayerLost()
    {
        uiManager.ShowGameLost();
    }
}