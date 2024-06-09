using UnityEngine;
using System.Collections.Generic;

public class Field : MonoBehaviour
{
    public List<GameObject> cardOnFieldForPlayer = new List<GameObject>();
    public List<GameObject> cardOnFieldForEnemy = new List<GameObject>();

    [SerializeField] private Transform[] playerPoints;
    [SerializeField] private Transform[] enemyPoints;

    [SerializeField] private float cardSpacing;

    private bool[] emptySpaceOnPlayerPoint;
    private int emptyIndexForPlayer;
    
    private bool[] emptySpaceOnEnemyPoint;
    private int emptyIndexForEnemy;


    private void Start()
    {
        updatingEmptySpaceForPlayer();
        updatingEmptySpaceForEnemy();
    }

    private void updatingEmptySpaceForPlayer()
    {
        emptySpaceOnPlayerPoint = new bool[playerPoints.Length];

        for (int i = 0; i < playerPoints.Length; i++)
        {
            emptySpaceOnPlayerPoint[i] = true;
        }
    }

    private void updatingEmptySpaceForEnemy()
    {
        emptySpaceOnEnemyPoint = new bool[enemyPoints.Length];

        for(int i = 0; i < enemyPoints.Length; i ++)
        {
            emptySpaceOnEnemyPoint[i] = true;
        }
    }

    public void setCardOnFieldForPlayer(GameObject _card , bool _addCard)
    {
        if(_addCard)
        {
            cardOnFieldForPlayer.Add(_card);
        }
        else
        {
            cardOnFieldForPlayer.Remove(_card);
        }
    }

    public void setCardOnFieldForEnemy(GameObject _card , bool _addCard)
    {
        if(_addCard)
        {
            cardOnFieldForEnemy.Add(_card);
        }
        else
        {
            cardOnFieldForEnemy.Remove(_card);
        }
    }

    public Transform getCardPositionForPlayerInField()
    {
        for(int i = 0; i < playerPoints.Length; i++)
        {
            if (emptySpaceOnPlayerPoint[i] == true)
            {
                emptyIndexForPlayer = i;

                Debug.Log(emptyIndexForPlayer);

                emptySpaceOnPlayerPoint[i] = false;
                return playerPoints[i];
            }
        }
        return null;
    }

    public int getCardHoldPointIndexForPlayer() 
    {
        return emptyIndexForPlayer;
    }

    public void updateEmptySpaceForPlayer(int _index)
    {
        for(int i = 0; i < playerPoints.Length; i++)
        {
            if(i == _index)
            {
                emptySpaceOnPlayerPoint[i] = true;
            }
        }
    }

    public Transform getCardPositionForEnemyInField()
    {
        for(int i = 0; i < enemyPoints.Length; i++)
        {
            if (emptySpaceOnEnemyPoint[i] == true)
            {
                emptyIndexForEnemy = i;
                Debug.Log("ENEMY : " + emptyIndexForEnemy);

                emptySpaceOnEnemyPoint[i] = false;

                return enemyPoints[i];
            }
        }
        return null;
    }

    public int getCardHoldPointIndexForEnemy()
    {
        return emptyIndexForEnemy;
    }

    public void updateEmptySpaceForEnemy(int _index)   
    {
        for(int i = 0; i < enemyPoints.Length; i++)
        {
            if(i == _index)
            {
                emptySpaceOnEnemyPoint[i] = true;
            }
        }
    }
}
