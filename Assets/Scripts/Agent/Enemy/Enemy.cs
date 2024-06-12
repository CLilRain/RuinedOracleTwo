using UnityEngine;
using System.Collections.Generic;

public class Enemy : Agent
{
    private EnemyAI enemyAI;
    public Animator animator {  get; private set; }

    public List<GameObject> cardsInEnemyHand = new List<GameObject>();
   
    public bool cardDrawLeft { get; set; } = true;


    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.instance.isEnemyTurn && cardDrawLeft)
        {
            enemyAI.spawnCardForEnemy();
        }
    }

    #region Card affect functions
    public override void HPGain(int amount)
    {
    }

    public override void HPLoss(int amount)
    {
    }

    public override void EssenceGain(int amount)
    {
    }

    public override void EssenceLoss(int amount)
    {
    }

    public override void MaxEssenceGain(int amount)
    {
    }

    public override void MaxEssenceLoss(int amount)
    {
    }

    public override void DrawCards(int amount)
    {
    }
    #endregion
}
