using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 100;
    [SerializeField] int dealDamage = 10;
    [SerializeField] int speed = 20;
    [SerializeField] int xAngle = 180;
    [SerializeField] EnemyState currentState;
    [SerializeField] bool isFacingRight;

    SpriteRenderer spriteRenderer;
    Action action;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        action = GetComponent<Action>();
    }

    private void Update()
    {
        Die();
        ChangeState();

        if (currentState == EnemyState.Patrol)
            Move();
        if (currentState == EnemyState.Attack)
            Attack();
    }

    public void ChangeState()
    {
        Vector2 leftRayOffset, rightRayOffset;

        rightRayOffset = transform.position + Vector3.right / 1.5f;
        leftRayOffset = transform.position + Vector3.left / 1.5f;

        var leftOrigin = leftRayOffset;
        var rightOrigin = rightRayOffset;
        var distance = 0.3f;
        var direction = Vector2.right * distance;
        var rightRay = Physics2D.Raycast(rightOrigin, direction, distance);
        var leftRay = Physics2D.Raycast(leftOrigin, -direction, distance);
        Debug.DrawRay(rightOrigin, direction, Color.red);
        Debug.DrawRay(leftOrigin, -direction, Color.yellow);

        if (rightRay.collider != null)
        {
            if (rightRay.collider.CompareTag("Player"))
                currentState = EnemyState.Attack;
            else
            {
                currentState = EnemyState.Patrol;
                isFacingRight = !isFacingRight;
            }
        }
        else if (leftRay.collider != null)
        {
            if (leftRay.collider.CompareTag("Player"))
                currentState = EnemyState.Attack;
            else
            {
                currentState = EnemyState.Patrol;
                isFacingRight = true;
            }
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }

    public void Move()
    {
        action.Move(2);
        if (isFacingRight)
        {
            spriteRenderer.flipX = false;
            //transform.rotation = new Quaternion(0, 0, 0, 0);
            transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
        }
        else
        {
            spriteRenderer.flipX = true;
            //transform.rotation = new Quaternion(0,xAngle,0,0);
            transform.Translate(Vector2.left * speed * Time.fixedDeltaTime);
        }
    }

    public virtual void Attack()
    {
        action.Attack();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //if (collision.gameObject.CompareTag("Player"))
    //    //    Attack();
    //    //else if(!collision.gameObject.CompareTag("Ground"))
    //    //    isFacingRight = !isFacingRight;
    //}

    public virtual void TakeDamage(int damage)
    {
        if (health >= 0)
            health -= damage;
    }

    public void Die()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            action.Die();
        }
    }

    enum EnemyState
    {
        Patrol,
        Attack,
    }
}
