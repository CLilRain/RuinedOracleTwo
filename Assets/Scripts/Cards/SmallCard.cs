using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SmallCard : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler , IPointerClickHandler
{
    private Cards_SO cardData;
    private Animator animator;

    private Field field;

    [SerializeField] private float rotateSpeed = 3.5f;

    [SerializeField] private Image mainImgHolder; 

    private GameObject mainCard;    // from which card this card created
    private int holdPointIndexInField;  // so we know , when field is empty

    private LineRenderer line;  

    private bool isDraging;

    private bool aimingTowardEnemy;
    private Vector2 mousePosition;

    private bool attackEnemy; 
    private Vector2 backPointPosition;
    private Vector2 playerCardTargetPosition;

    private Vector2 enemyCardFieldPosition;
    private Vector2 enemyCardTargetPosition;
    private bool isBelongToEnemy;
    private bool isEnemyCardReachedBattleField;

    private Agent host;
    private Agent target;

    public bool attackPlayer { get; set; }

    private void Start()
    {
        field = FindAnyObjectByType<Field>();

        animator = GetComponent<Animator>();
        line = GameManager.instance.lineRenderer;
    }

    private void Update()
    {
        if(isBelongToEnemy)
        {
            if(!isEnemyCardReachedBattleField)
            {
                enemyCardGoingToBattleField();
            }

            if(isEnemyCardReachedBattleField && attackPlayer)
            {
                StartCoroutine(AttackPlayer());
            }

            return;
        }

        if (isDraging)
        {
            if(Input.GetMouseButtonUp(0))
            {
                isDraging = false;

                SettingLineToZero();

                //check if you are targeting enemy -- then rotate other wise no need to prob
                aimingTowardEnemy = true;
            }
        }

        if (aimingTowardEnemy)
        {
            StartCoroutine(rotateCardTowardEnemy());
        }

        if(attackEnemy)
        {
            transform.position = 
                Vector2.MoveTowards(transform.position , playerCardTargetPosition , 100 * Time.deltaTime);

            if(Vector2.Distance(transform.position , playerCardTargetPosition) < 3f)
            {
                if(playerCardTargetPosition == backPointPosition)
                {
                    attackEnemy = false;
                    field.setCardOnFieldForPlayer(mainCard, false);
                    animator.SetTrigger("Disapper_A");
                    Destroy(mainCard);  // now we don't neeed that cause we can't hover now
                }
                else
                {
                    giveDamageToEnemy();
                }

                playerCardTargetPosition = backPointPosition;
            }
        }
    }
     

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.instance.isEnemyTurn || isBelongToEnemy)
            return;

        if (CardManager.instance.isCardsBusy)
            return;

        isDraging = true;

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        LineFollowMouse();

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity);

        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<EnemyHealth>() != null)
            {
                CardManager.instance.isCardsBusy = true;
                aimingTowardEnemy = true;   
                line.gameObject.SetActive(false);

                playerCardTargetPosition = mousePosition;
            }
        }
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)   // disable big card 
    {
        if (isBelongToEnemy)
            return;


        if(mainCard != null)
            mainCard.SetActive(false);
    }
  
    public void OnPointerClick(PointerEventData eventData)  // show Big Card -- so we can read details
    {
        if (GameManager.instance.isEnemyTurn || isBelongToEnemy) 
            return;

        if(mainCard != null)    
        {
            mainCard.SetActive(true);
            mainCard.transform.position = transform.position + new Vector3(4, 1, 0);
        }
    }

    public void smallCardSetup
        ( Cards_SO _cardData , GameObject _mainCard , Sprite _sprite , Vector3 _spawnPosition , Vector3 _targetPosition , int _holdPointIndex , bool _isBelongToEnemy , bool _attackPlayer, Agent host, Agent target)
    {
        cardData = _cardData;

        mainCard = _mainCard;
        mainImgHolder.sprite = _sprite;
        transform.position = _spawnPosition;

        enemyCardFieldPosition = _targetPosition;

        holdPointIndexInField = _holdPointIndex;

        isBelongToEnemy = _isBelongToEnemy;
        attackPlayer = _attackPlayer;

        backPointPosition = Camera.main.transform.position;

        this.host = host;
        this.target = target;

        host.EssenceLoss(cardData.ManaCost);
    }

    private void LineFollowMouse()
    {
        line.gameObject.SetActive(true);

        line.SetPosition(0, transform.position);
        line.SetPosition(1, mousePosition);
    }

    private IEnumerator rotateCardTowardEnemy()
    {
        Vector2 direction = (Vector2)transform.position - mousePosition;

        //backPointPosition = new Vector2(direction.x / 6, direction.y / 6);  

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle + 90f);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        yield return new WaitForSeconds(1.5f);        
        
        aimingTowardEnemy = false;
        attackEnemy = true; 
        
    }
   
    private void SettingLineToZero()
    {
        if (line != null && !isDraging)  // no draging ? set position of line renderer so we can't see
        {
            line.SetPosition(0, Vector2.zero);
            line.SetPosition(1, Vector2.zero);
        }
    }

    private void giveDamageToEnemy()
    {
        EnemyHealth enemyHealth = GameManager.instance.Enemy.GetComponent<EnemyHealth>();
        //enemyHealth.takeDamageOf(cardData.DamageToTarget);

        if (host != null && target != null)
        {
            cardData.ApplyCardEffects(host, target);
        }
        else
        {
            Debug.Log("One of these is null. Host " + host + ", target " + target);
        }

        GameObject _cardAttackEffect = Instantiate(cardData.CardAttackEffect, GameManager.instance.Enemy.position, Quaternion.identity);
        Destroy(_cardAttackEffect, 2f);
    }

    private IEnumerator AttackPlayer()
    {
        //rotate before attacking
        rotateEnemyCard();

        yield return new WaitForSeconds(1f);

        transform.position = Vector2.MoveTowards(transform.position, enemyCardTargetPosition, 100 * Time.deltaTime);

        if (Vector2.Distance(transform.position, enemyCardTargetPosition) < .5f)
        {
            if (enemyCardTargetPosition == (Vector2)Camera.main.transform.position)
            {
                Destroy(mainCard);
                field.setCardOnFieldForEnemy(mainCard, false);
                animator.SetTrigger("Disapper_A");
                attackPlayer = false;
            }
            else
            {
                giveDamageToPlayer();
                enemyCardTargetPosition = Camera.main.transform.position;
            }
        }
    }

    private void giveDamageToPlayer()
    {
        PlayerHealth playerHealth = GameManager.instance.Player.GetComponent<PlayerHealth>();
        //playerHealth.takeDamageOf(cardData.DamageToTarget);

        if (host != null && target != null)
        {
            cardData.ApplyCardEffects(host, target);
        }
        else
        {
            Debug.Log("One of these is null. Host " + host + ", target " + target);
        }

        GameObject _cardAttackEffect = Instantiate(cardData.CardAttackEffect, GameManager.instance.Player.position , Quaternion.identity);
        Destroy(_cardAttackEffect , 2f);
    }

    private void rotateEnemyCard()
    {
        Vector2 direction = ((Vector2)transform.position - enemyCardTargetPosition).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle + 90f);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }

    private void enemyCardGoingToBattleField()
    {
        transform.position =
                   Vector2.MoveTowards(transform.position, enemyCardFieldPosition, 20 * Time.deltaTime);

        if (Vector2.Distance(transform.position, enemyCardFieldPosition) < .5f)
        {
            enemyCardTargetPosition = GameManager.instance.Player.position;
            isEnemyCardReachedBattleField = true;
         
            if(!attackPlayer)
                GameManager.instance.turnUI.PlayPlayerTurnAnimation();
        }
    }

    public void destroyThisCard()  // i use this function for animation events -- small card disapper animation
    {
        GameManager gameManager = GameManager.instance;

        if (!isBelongToEnemy)
        {
            field.updateEmptySpaceForPlayer(holdPointIndexInField);
            gameManager.turnUI.PlayEnemyTurnAnimation();
        }
        else
        {
            gameManager.Enemy.GetComponent<EnemyAI>().smallCardsInFields.Remove(this.gameObject);

            field.updateEmptySpaceForEnemy(holdPointIndexInField);
            gameManager.turnUI.PlayPlayerTurnAnimation();
        }

        Destroy(this.gameObject);
    }
}
