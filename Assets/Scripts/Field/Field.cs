using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public List<GameObject> cardOnFieldForPlayer = new List<GameObject>();    


    private Transform playerPoint;
    [SerializeField] private Transform[] playerPoints;


    [SerializeField] private float cardSpacing;

    private bool[] emptySpace;
    private int emptyIndex;


    private void Awake()
    {
        playerPoint = transform.GetChild(0);
    }

    private void Start()
    {
        //playerPoints = new Transform[playerPoint.childCount];
        //playerPoints = playerPoint.GetComponentsInChildren<Transform>();

        emptySpace = new bool[playerPoints.Length];

        for (int i = 0; i < playerPoints.Length; i++)
        {
            emptySpace[i] = true;
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

    public Transform getCardPositionForPlayerInField()
    {
        for(int i = 0; i < playerPoints.Length; i++)
        {
            if (emptySpace[i] == true)
            {
                emptyIndex = i;

                Debug.Log(emptyIndex);

                emptySpace[i] = false;
                return playerPoints[i];
            }
        }

        return null;
    }

    public int getCardHoldPointIndex() 
    {
        return emptyIndex;
    }

    public void updateEmptySpace(int _index)
    {
        for(int i = 0; i < playerPoints.Length; i++)
        {
            if(i == _index)
            {
                emptySpace[i] = true;
            }
        }
    }
}
