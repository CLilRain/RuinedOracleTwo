using System.Collections;
using UnityEngine;

public class Turn_UI : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayPlayerTurnAnimation()
    {
        CardManager.instance.delayCardBusy();

        animator.SetBool("EnemyTurn_A", false);
        animator.SetBool("PlayerTurn_A" , true);
    }

    public void PlayEnemyTurnAnimation()
    {
        CardManager.instance.isCardsBusy = true;

        animator.SetBool("PlayerTurn_A", false);
        animator.SetBool("EnemyTurn_A", true);
    }

    public void onPressedEndTurn()
    {
        if(GameManager.instance.isPlayerTurn) 
            PlayEnemyTurnAnimation();
    }
}
