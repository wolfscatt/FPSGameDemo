using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [Header("Movement")]
    public float speed = 12f;
    public float jumpSpeed = 5f;
    // Gravity fields
    public float gravity = -15f;
    private Vector3 gravityVector;

    [Header("GroundCheck")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.4f;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("Health")]
    public int playerHealth = 100;
    public Slider healthBar;
    public TextMeshProUGUI healthText;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
        GroundCheck();
        JumpAndGravity();
    }

    private void MovePlayer()
    {
        // Move the player
        Vector3 moveVector = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;
        if(!isGrounded)
        {
            characterController.Move(moveVector * Mathf.Sqrt(speed*2) * Time.deltaTime);
        }
        else
        {
            characterController.Move(moveVector * speed * Time.deltaTime);
        }
    }
    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }
    private void JumpAndGravity()
    {
        // Apply gravity
        gravityVector.y += gravity * Time.deltaTime;
        characterController.Move(gravityVector * Time.deltaTime);

        if (isGrounded && gravityVector.y < 0)
        {
            gravityVector.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            gravityVector.y = Mathf.Sqrt(jumpSpeed * -2f * gravity);
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        playerHealth -= damage;
        healthBar.value -= damage;
        healthText.text = playerHealth.ToString();
        if(playerHealth <= 0)
        {
            PlayerDeath();
            healthBar.value = 0;
            healthText.text = "0";
        }
    }

    private void PlayerDeath()
    {
        // Ölüm paneli açılacak
        Debug.Log("Player is dead");
    }
}
