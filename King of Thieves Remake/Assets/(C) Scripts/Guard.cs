using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] private Vector2 pointA, pointB;
    private Vector2 currentPoint;
    private bool toPointB = true;

    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentPoint = pointB;
    }

    private void FixedUpdate()
    {
        rb.position = Vector2.MoveTowards(transform.position, currentPoint, moveSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, currentPoint) < 0.1f)
        {
            if (toPointB)
            {
                currentPoint = pointB;
                sr.flipX = false;
            }
            else
            {
                currentPoint = pointA;
                sr.flipX = true;
            }

            toPointB = !toPointB;
        }
    }
}