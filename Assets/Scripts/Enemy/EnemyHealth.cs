using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Enemy enemy;

    public int health { get; private set; } = 100;

    private bool hit;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();  
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (hit && !enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("enemyHitAnimation"))
        {
            hit = false;
            enemy.animator.SetBool("Hit_A", hit);
        }
    }

    public void takeDamageOf(int damageAmount)
    {
        health -= damageAmount;

        StartCoroutine(hitEffect());

        if(health < 0)
        {
            // what we should do when enemy died
        }
    }

    private IEnumerator hitEffect()
    {
        hit = true;

        enemy.animator.SetBool("Hit_A", hit);
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(.5f);

        spriteRenderer.color = Color.white;
    }
}
