using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationDuration = 0.75f;
    private Rigidbody rb;
    private Transform graphics;
    private bool isRotating = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        graphics = transform.Find("Graphics");
        
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on the GameObject.");
        }

        if (graphics == null)
        {
            Debug.LogError("Graphics GameObject not found. Make sure your character has a child GameObject named 'Graphics' with the sprite renderer.");
        }
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        rb.velocity = moveDirection * moveSpeed;

        if (moveX != 0 && !isRotating)
        {
            StartCoroutine(RotateSprite(moveX));
        }
    }

    IEnumerator RotateSprite(float moveX)
    {
        isRotating = true;

        Quaternion startRotation = graphics.rotation;
        Quaternion endRotation = Quaternion.Euler(0, moveX > 0 ? 0 : 180, 0);

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            graphics.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        graphics.rotation = endRotation;
        isRotating = false;
    }
}
