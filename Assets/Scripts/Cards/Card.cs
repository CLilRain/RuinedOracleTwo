using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Card : MonoBehaviour, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;
    private CardMotion cardMotion;

    public Cards_SO cardData { get; set; }

    [SerializeField] private Image mainCardImg;
    [SerializeField] private Sprite cardBackSide;       // back side img of a card

    public GameObject smallCard;


    [SerializeField] private GameObject cardDetailBoxPrefab;
    private GameObject cardDetailBox;

    private LineRenderer lineRenderer;

    private bool isFacingMainSide;      // checking which side we facing -- front (main) or back
    private bool isDraging;
    private bool isOnField;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        cardMotion = GetComponent<CardMotion>();
    }

    private void Start()
    {
        lineRenderer = GameManager.instance.lineRenderer;
        lineRenderer.gameObject.SetActive(false);

    }

    private void Update()
    {
        if(isDraging)
        {
            if (Input.GetMouseButtonUp(0))
                isDraging = false;
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (CardManager.instance.isCardsBusy || cardMotion.isBelongToEnemy)
            return;

        Debug.Log("DRAG");


        if (GameManager.instance.isPlayerTurn)  // if this is player turn
        {
            StopAllCoroutines(); // cause we don't want it to flip while draging

            isDraging = true;

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position = mousePosition;
           
            if (cardDetailBox != null)
                Destroy(cardDetailBox);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (CardManager.instance.isCardsBusy || cardMotion.isBelongToEnemy)
            return;
            
        Debug.Log("DROP");

        if (!GameManager.instance.isEnemyTurn)
        {
            isDraging = false;


            RaycastHit2D hit =
                Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);


            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<Field>() != null)
                {
                    Field field = hit.collider.GetComponent<Field>();

                    if (field.cardOnFieldForPlayer.Count >= 3)    // can change field space (later if needed)
                    {
                        Debug.LogWarning("No Space On Field !");
                        return;
                    }

                    field.setCardOnFieldForPlayer(this.gameObject, true);

                    isOnField = true;
                    transform.parent = null;

                    GameObject _smallCard = Instantiate(smallCard);

                    _smallCard.GetComponent<SmallCard>().
                        smallCardSetup(cardData, this.gameObject, cardData.smallIMG, field.getCardPositionForPlayerInField().position, Vector2.zero , field.getCardHoldPointIndexForPlayer() , false , false);

                    GameManager.instance.Player.GetComponent<Player>().cardsInPlayerHand.Remove(cardData);
                    this.gameObject.SetActive(false);

                    //check if not enough mana to attack then pass turn to Enemy
                    //GameManager.instance.turnUI.PlayEnemyTurnAnimation();

                    return;
                }
            }
            // if you not droping on field -- it return to it original position
            transform.position = cardMotion.targetPoint.position;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CardManager.instance.isCardsBusy || cardMotion.isBelongToEnemy)
            return;

            Debug.Log("ENTER");

        if (GameManager.instance.isPlayerTurn)
        {
            GetComponentInChildren<Canvas>().sortingOrder = 2;
           
            if (isOnField)
                 return;    
           

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
        if (CardManager.instance.isCardsBusy || cardMotion.isBelongToEnemy)
            return; 

        Debug.Log("EXIT");

        if (GameManager.instance.isPlayerTurn)
        {
            GetComponentInChildren<Canvas>().sortingOrder = 0;

            animator.SetBool("SHOWCARD_A", false);

            if (!isOnField)
                transform.position = cardMotion.targetPoint.position;


            if (cardDetailBox != null)
                Destroy(cardDetailBox);
        }
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
