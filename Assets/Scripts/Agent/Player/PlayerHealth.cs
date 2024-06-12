using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private Player player;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private int health;

    private bool hit;

    private void Awake()
    {
        player = GetComponent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (hit && !player.animator.GetCurrentAnimatorStateInfo(0).IsName("playerhitAnimation"))
        {
            hit = false;
            player.animator.SetBool("Hit_A" , hit);
        }
    }

    public void takeDamageOf(int damageAmount)
    {
        health -= damageAmount;

        StartCoroutine(hitEffect());

        if (health < 0)
        {
            // what we should do when enemy died
        }
    }

    private IEnumerator hitEffect()
    {
        hit = true;
        player.animator.SetBool("Hit_A", hit);

        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(.5f);

        spriteRenderer.color = Color.white;
    }
}
