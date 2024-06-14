using UnityEngine;
using System.Collections.Generic;

public class Enemy : Agent
{
    public static Enemy Instance;

    private EnemyAI enemyAI;
    public Animator animator {  get; private set; }

    public List<GameObject> cardsInEnemyHand = new List<GameObject>();
   
    public bool cardDrawLeft { get; set; } = true;

    protected override void Awake()
    {
        base.Awake();

        enemyAI = GetComponent<EnemyAI>();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        Instance = this;
    }

    private void Update()
    {
        if (GameManager.instance.isEnemyTurn && cardDrawLeft)
        {
            enemyAI.spawnCardForEnemy();
        }
    }

    protected override void UpdateHealthUI()
    {
        hud.SetEnemyHealth(health, maxHealth);
    }

    protected override void UpdateEssenceUI()
    {
        hud.SetEnemyEssence(essence, maxessence);
    }

    protected override void AgentDead()
    {
        uiManager.ShowGameWon();
    }
}