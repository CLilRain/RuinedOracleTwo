using UnityEngine;

public class CardMotion : MonoBehaviour
{
    public float speed { get; set; } = 10;

    private GameObject[] mainCardHoldPoints;
    public Transform targetPoint {  get;  set; }



    public bool targetReached {  get; set; }    
    public bool isBelongToEnemy {  get; set; }
    public bool goingTowardEnemy {  get; set; } 



    private void Start()
    {
        if (isBelongToEnemy)        // if this is enemy card , we don't need this card to go default hold card position
            return;

        mainCardHoldPoints = new GameObject[6];
        mainCardHoldPoints = GameObject.FindGameObjectsWithTag("Points");

        gettingTargetPosition();
    }


    private void Update()
    {
        if(!targetReached)
        {
            transform.position = 
                Vector2.MoveTowards(transform.position , targetPoint.position , speed * Time.deltaTime);


            if (Vector2.Distance(transform.position, targetPoint.position) < .1f)
            {
                PlayerCardHitEnemy();

                if (isBelongToEnemy)
                {
                    GameManager.instance.turnUI.PlayPlayerTurnAnimation();

                    FindAnyObjectByType<PlayerHealth>().takeDamageOf(GetComponent<Card>().cardData.AttackDamage);
                    GetComponent<Card>().destroyCard();
                }

                if (!isBelongToEnemy && !goingTowardEnemy)
                {
                    GetComponent<Card>().canClickable = true;   //can't click on card until card won't reach his default Position
                }

                targetReached = true;
            }
        }
    }

    private void PlayerCardHitEnemy()
    {
        if (!isBelongToEnemy && goingTowardEnemy)
        {
            GetComponent<Card>().PlayerCardCollideWithEnemy(targetPoint);
        }
    }

    private void gettingTargetPosition()
    {
        for (int i = 0; i < mainCardHoldPoints.Length; i++)
        {
            if (mainCardHoldPoints[i].transform.childCount == 0)
            {
                if (targetPoint == null)
                {
                    targetPoint = mainCardHoldPoints[i].transform;
                    transform.SetParent(targetPoint);
                    return;
                }
            }
            
        }
    }
}

// this is only for card to move to its default position 