using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField]
    private Vector2 ogPosition;
    [SerializeField]
    private Object holding;
    [SerializeField]
    private float returnSpeed;
    private int touching;
    Rigidbody2D rb;

    private void Awake()
    {
        ogPosition = transform.position;
        touching = 0;
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (touching == 0)
        {
            rb.simulated = false;
            if(transform.position.y>ogPosition.y)
            {
    
                transform.position = new Vector2(ogPosition.x,transform.position.y-(returnSpeed*Time.deltaTime));
            }
            else if(transform.position.y<=ogPosition.y-0.1f)
            {
                transform.position = new Vector2(ogPosition.x, transform.position.y + (returnSpeed * Time.deltaTime));
            }
        }
        else
        {
            rb.simulated = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject!=holding)
        {
            touching++;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject != holding)
        {
            touching--;
        }
    }
}
