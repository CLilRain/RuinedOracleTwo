using UnityEngine;
using UnityEngine.EventSystems;

public class FakeCard : MonoBehaviour, IPointerDownHandler
{
    public float moveSpeed { get; set; }
    public Vector2 target { get; set; }

    private bool targetReached;

    public bool isPlayersCard;

    private void Update()
    {
        if (!targetReached)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target) < .1f && !targetReached)
            {
                targetReached = true;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //if you want - card draw by clicking

      /*  if (isPlayersCard && GameManager.instance.isPlayerTurn)
        {
            GameManager.instance.drawCardFromCardCollection();
        }*/
    }
}
