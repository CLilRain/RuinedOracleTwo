using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
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
}
