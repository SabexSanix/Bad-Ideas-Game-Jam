using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private bool isPressed{get; set;}
    [SerializeField]
    private Vector2 ogPosition;
    [SerializeField]
    private Object holding;
    [SerializeField]
    private float returnSpeed;
    [SerializeField]
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
            rb.bodyType = RigidbodyType2D.Kinematic;
            if (rb.position.y > ogPosition.y)
            {
                rb.position = ogPosition;
                Debug.Log("Zu weit oben");
            }
            else if (rb.position.y < ogPosition.y)
            {
                rb.MovePosition(rb.position + new Vector2(0, returnSpeed * Time.fixedDeltaTime));
                Debug.Log("Zu weit unten");
            }
            isPressed = false;
        }
        else
        {

            rb.bodyType = RigidbodyType2D.Dynamic;
            isPressed = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
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
