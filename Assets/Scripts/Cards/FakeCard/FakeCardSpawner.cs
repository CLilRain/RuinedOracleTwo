using System.Collections.Generic;
using UnityEngine;

public class FakeCardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fakeCardPrefab;


    [SerializeField] private List<Cards_SO> TotalCards = new List<Cards_SO>();

     private Transform spawnPointForPlayer;
     private Transform targetPointForPlayer;

     private Vector2 spawnPoint;
     private Vector2 targetPoint;

    private void Awake()
    {
        spawnPointForPlayer = transform.GetChild(0).transform;
        targetPointForPlayer = transform.GetChild(1).transform;
    }


    private void Start()
    {
        spawnPoint = spawnPointForPlayer.position;
        targetPoint = targetPointForPlayer.position;

        //fake cards being spawn

        spawnFakeCardsForPlayer();
       // spawnFakeCardsForEnemy();
    }

    #region spawn FakeCards

    private void spawnFakeCardsForPlayer()
    {
        for (int i = 0; i < TotalCards.Count; i++)
        {
            spawnFakeCards(spawnPoint, true, i, targetPoint);
        }
    }

    private void spawnFakeCardsForEnemy()
    {
        for (int i = 0; i < TotalCards.Count; i++)
        {
            spawnFakeCards(-spawnPoint, false, i, new Vector2(-targetPoint.x, targetPoint.y));
        }
    }

    private void spawnFakeCards(Vector2 spawnPoint , bool isPlayerCard , int i , Vector2 targetPoint)
    {
        GameObject card = 
            Instantiate(fakeCardPrefab, spawnPoint, Quaternion.identity, transform);

        FakeCard fakeCard = card.GetComponent<FakeCard>();

        fakeCard.isPlayersCard = isPlayerCard;

        fakeCard.moveSpeed = 20f * (i + 1);
        fakeCard.target = targetPoint;
    }


    #endregion
}
