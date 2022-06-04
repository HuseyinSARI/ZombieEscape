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

   
    //fixed update �al��mak i�in fiziksel i�lemlerin tamamlanmas�n� bekler. update den sonra �al���r.
    private void FixedUpdate()
    {
        //global hareket eden vekt�rler bakt���m�z y�ne gitmek i�in local vect�r haline �evirdik.
        Vector3 localVerticalVector = transform.forward * verticalInput;
        Vector3 localHorizontalVector = transform.right * horizontalInput;

        Vector3 movementVector = localHorizontalVector + localVerticalVector;

        movementVector.Normalize();                             //�apraz gitme komutu verildi�inde vekt�r toplamas� sonucu karakterin oldu�undan daha fazla h�zl� gitme
        movementVector *= currentSpeed * Time.deltaTime;        //problemini, vekt�r� �nce normalize edip daha sonra h�z bilgisini ekleyerek ��zd�k.

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
