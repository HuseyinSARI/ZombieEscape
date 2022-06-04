using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control Settings")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 12f;

    private CharacterController characterController_ref;

    private float currentSpeed = 8f;
    private float horizontalInput;
    private float verticalInput;

    private void Awake()
    {
        characterController_ref = GetComponent<CharacterController>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        KeyboardInput();   
    }

   
    //fixed update çalýþmak için fiziksel iþlemlerin tamamlanmasýný bekler. update den sonra çalýþýr.
    private void FixedUpdate()
    {
        //global hareket eden vektörler baktýðýmýz yöne gitmek için local vectör haline çevirdik.
        Vector3 localVerticalVector = transform.forward * verticalInput;
        Vector3 localHorizontalVector = transform.right * horizontalInput;

        Vector3 movementVector = localHorizontalVector + localVerticalVector;

        movementVector.Normalize();                             //çapraz gitme komutu verildiðinde vektör toplamasý sonucu karakterin olduðundan daha fazla hýzlý gitme
        movementVector *= currentSpeed * Time.deltaTime;        //problemini, vektörü önce normalize edip daha sonra hýz bilgisini ekleyerek çözdük.

        characterController_ref.Move(movementVector );
    } 
    private void KeyboardInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

}
