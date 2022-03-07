using UnityEngine;
using UnityEngine.Events;
public class Player : MonoBehaviour
{
    [SerializeField] int health = 100;
    [SerializeField] int speed = 20;
    [SerializeField] int jumpForce = 10;
    [SerializeField] int dealDamage = 20;
    [SerializeField] bool isOnGround = true;
    [SerializeField] bool isGameOver = false;
    public UnityEvent OnLanding;

    bool istrue = false;


    Action action;
    Rigidbody2D rb;
    Animator animator;

    private void Start()
    {
        OnLanding = new UnityEvent();
        action = GetComponent<Action>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        Landing();

        if (rb.velocity.y <= 0)
            action.Jump(false);
        

        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.RightArrow))
                Move(speed);
            if (Input.GetKey(KeyCode.LeftArrow))
                Move(-speed);
            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
                Jump();
            if (Input.GetKeyDown(KeyCode.F))
                Attack();
        }
        else
            action.Idle();
    }

    public void Move(int speed)
    {
        action.Move(1);
        transform.Translate(Vector2.right * Mathf.Abs(speed) * Time.fixedDeltaTime);

        if (speed < 0)
            transform.rotation = new Quaternion(0, 180, 0, 0);
        else
            transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void Jump()
    {
        action.Jump(true);
        rb.AddForce(Vector2.up * jumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    public void Landing()
    {
        if (!isOnGround && rb.velocity.y < 0 && istrue)
        {
            var origin = transform.position + Vector3.down;
            var distance = 0.5f;
            var direction = Vector2.down * distance;
            var ray = Physics2D.Raycast(origin, direction, distance);

            if (ray.collider != null)
            {
                if (ray.collider.CompareTag("Ground"))
                {
                    action.Land(true);
                    istrue = false;
                }
            }
            Debug.DrawRay(origin, direction, Color.red);
        }
        if (isOnGround)
        {
            istrue = true;
            action.Land(false);
        }
    }

    public void Attack()
    {
        var origin = transform.position + Vector3.right/1.5f;
        var direction = Vector2.right;
        var distance = 0.2f;
        var ray = Physics2D.Raycast(origin, direction, distance);

        action.Attack();

        if (ray.collider != null)
        {
            Debug.Log(ray.collider.name);
            var enemy = ray.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(dealDamage);
                Debug.Log("Attaking Enemy");
            }
        }
        Debug.DrawRay(origin, direction/5, Color.blue);
    }

    public void TakeDamage(int damage)
    {
        if(health >=0)
            health -= damage;
    }

    public void Die()
    {
        if (health <= 0)
        {
            isGameOver = true;
            action.Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isOnGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isOnGround = false;
    }

}
