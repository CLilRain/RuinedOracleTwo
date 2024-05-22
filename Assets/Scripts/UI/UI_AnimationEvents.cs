using UnityEngine;

public class UI_AnimationEvents : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public void setIsPlayerTurnToTrue()
    {
        gameManager.Player.GetComponent<Player>().cardDrawnAtPlayerTurn = false;
        gameManager.isPlayerTurn = true;
    }

    public void setIsPlayerTurnToFalse()
    {
        gameManager.isPlayerTurn = false;
    }

    public void setIsEnemyTurnToTrue()
    {
        gameManager.Enemy.GetComponent<Enemy>().cardDrawLeft = true;   // so when enemy turn start -- she / he can draw card again 
        gameManager.isEnemyTurn = true;
    }

    public void setIsEnemyTurnToFalse()
    {
        gameManager.isEnemyTurn = false;
    }
}
