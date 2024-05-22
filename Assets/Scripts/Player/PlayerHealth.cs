using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private int health;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(.05f);

        spriteRenderer.color = Color.white;
    }
}
