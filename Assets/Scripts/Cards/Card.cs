using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Card : MonoBehaviour, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;
    private CardMotion cardMotion;

    public Cards_SO cardData { get; set; }


    [SerializeField] private Image mainCardImg;
    [SerializeField] private Sprite cardBackSide;       // back side img of a card

    [SerializeField] private GameObject cardDetailBoxPrefab;
    private GameObject cardDetailBox;

    private LineRenderer lineRenderer;

    private bool isFacingMainSide;      // checking which side we facing -- front (main) or back
    private bool isDraging;
    private bool isOnField;

    public bool canClickable { get; set;}


    public int holdPointIndex;  


    private void Awake()
    {
        animator = GetComponent<Animator>();
        cardMotion = GetComponent<CardMotion>();
    }

    private void Start()
    {
        canClickable = false;

        lineRenderer = GameManager.instance.lineRenderer;
        lineRenderer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(isDraging)
        {
            if(Input.GetMouseButtonUp(0))
                isDraging = false;

            if(lineRenderer != null && !isDraging)  // no draging ? set position of line renderer so we can't see
            {
                lineRenderer.SetPosition(0, Vector2.zero);
                lineRenderer.SetPosition(1 , Vector2.zero);
            }
        }

        if(GameManager.instance.isEnemyTurn && !cardMotion.isBelongToEnemy)    // to fix an issue 
        {
            animator.SetBool("SHOWCARD_A", false);

            if (cardDetailBox != null)
                Destroy(cardDetailBox);

            if (!isOnField)
                transform.position = cardMotion.targetPoint.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.instance.isPlayerTurn)  // if this is player turn
        {
            StopAllCoroutines(); // cause we don't want it to flip while draging

            isDraging = true;

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (!isOnField)     // if not on field -- follow mouse 
            {
                transform.position = mousePosition;
            }
            else       //if on field, no need to move card -- just draw arrow
            {

                if (lineRenderer != null)
                {
                    lineRenderer.gameObject.SetActive(true);
                    
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, mousePosition);
                }

                RaycastHit2D hit = 
                    Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);


                if (hit.collider != null)
                {
                    if (hit.collider.GetComponent<EnemyHealth>() != null)
                    {
                        cardMotion.speed = 50;
                        cardMotion.targetReached = false;
                        cardMotion.goingTowardEnemy = true;
                        cardMotion.targetPoint = hit.collider.transform;

                        lineRenderer.gameObject.SetActive(false);   
                        // you can destroy it later but make sure to destory card too
                    }
                }
            }

            if (cardDetailBox != null)
                Destroy(cardDetailBox);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        isDraging = false;

       

        RaycastHit2D hit = 
            Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);


        if(hit.collider != null)
        {
            if(hit.collider.GetComponent<Field>() != null)
            {
                Field field = hit.collider.GetComponent<Field>();

                if (field.cardOnFieldForPlayer.Count >= 3)    // can change field space (later if needed)
                {
                    Debug.LogWarning("No Space On Field !");
                    return;
                }

                field.setCardOnFieldForPlayer(this.gameObject , true);


                isOnField = true;

                transform.parent = null;
                transform.position = field.getCardPositionForPlayerInField().position;

                holdPointIndex = field.getCardHoldPointIndex();

                //check if not enough mana to attack then pass turn to Enemy
                //GameManager.instance.turnUI.PlayEnemyTurnAnimation();

                return;
            }

        }

         transform.position = cardMotion.targetPoint.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.instance.isPlayerTurn && canClickable)
        {
            GetComponentInChildren<Canvas>().sortingOrder = 2;
           
            if (isOnField)
            {
                return;
            }

            animator.SetBool("SHOWCARD_A", true);

            transform.position = 
                new Vector2(cardMotion.targetPoint.position.x, -9.5f);       // make card go UP when we hover


            if (!isDraging && isFacingMainSide)
            {
                summonDetailBox();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.instance.isPlayerTurn && canClickable)
        {
            GetComponentInChildren<Canvas>().sortingOrder = 0;

            animator.SetBool("SHOWCARD_A", false);

            if (!isOnField)
                transform.position = cardMotion.targetPoint.position;


            if (cardDetailBox != null)
                Destroy(cardDetailBox);
        }
    }



    public void PlayerCardCollideWithEnemy(Transform _target) 
    {
        GameManager manager = GameManager.instance;
        Field field = FindAnyObjectByType<Field>();


        _target.gameObject.GetComponent<EnemyHealth>().takeDamageOf(cardData.AttackDamage);

        manager.turnUI.PlayEnemyTurnAnimation();

        field.setCardOnFieldForPlayer(this.gameObject , false);
        field.updateEmptySpace(holdPointIndex);


        destroyCard();
    }

    private void summonDetailBox()
    {
        cardDetailBox =
                Instantiate(cardDetailBoxPrefab, transform.position + new Vector3(0, 5f, 0), Quaternion.identity);

        cardDetailBox.GetComponent<CardDetailBox>().setCardDetailsBox(cardData);
    }


    #region Flip Methods

    // we are using these in animation events

    public void setToMainSide()     
    {
        mainCardImg.sprite = cardData.CardIMG;
    }

    public void setToBackSide()
    {
        //mainCardImg.sprite = cardBackSide;
        mainCardImg.sprite = null;
    }

    #endregion    

    public void setCardToFlipMainSide()
    {
        isFacingMainSide = true;
        animator.SetBool("FLIP_A", isFacingMainSide);
    }

    public void destroyCard()
    {
        animator.SetTrigger("DISAPPER_A");  // card disapper
        Destroy(this.gameObject, .5f);
    }
}
