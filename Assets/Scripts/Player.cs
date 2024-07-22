using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationDuration = 0.75f;
    public Texture2D idleTexture;
    public Texture2D walkTexture;
    private Rigidbody rb;
    private Transform graphics;
    private bool isRotating = false;
    private bool isWalking = false;
    public Renderer frontRenderer;
    public Renderer backRenderer;
    public float animationTimer = 0f;
    public float animationInterval = 0.2f;

    float moveX, moveZ;

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

        if (frontRenderer == null || backRenderer == null)
        {
            Debug.LogError("FrontQuad or BackQuad not found. Make sure your character has child GameObjects named 'FrontQuad' and 'BackQuad' with the mesh renderer.");
        }

        SetIdleTexture();
    }

    void Update()
    {
        GetInput();
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        Move();
    }

    void GetInput()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
    }

    void Move()
    {
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        rb.velocity = (moveDirection * moveSpeed) + new Vector3(0f, rb.velocity.y, 0f);

        if (moveX != 0 && !isRotating)
        {
            StartCoroutine(RotateSprite(moveX));
        }

        if (moveX == 0f && moveZ == 0)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            isWalking = false;
        }
        else
        {
            isWalking = true;
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

    void UpdateAnimation()
    {
        if (isWalking)
        {
            animationTimer += Time.deltaTime;
            if (animationTimer >= animationInterval)
            {
                SetWalkTexture();
                if(animationTimer >= animationInterval * 2f)
                {
                    animationTimer = 0f;
                }
            }
            else
            {
                SetIdleTexture();
            }
        }
        else
        {
            SetIdleTexture();
        }
    }

    void SetIdleTexture()
    {
        frontRenderer.material.mainTexture = idleTexture;
        backRenderer.material.mainTexture = idleTexture;
    }

    void SetWalkTexture()
    {
        frontRenderer.material.mainTexture = walkTexture;
        backRenderer.material.mainTexture = walkTexture;
    }
}
