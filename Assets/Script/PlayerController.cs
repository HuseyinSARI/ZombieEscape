using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control Settings")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 12f;
    [Header("Mouse Control Options")]
    [SerializeField] float mouseSensivity = 1f;
    [SerializeField] float maxViewAngle = 60f;

    private CharacterController characterController_ref;

    private float currentSpeed = 8f;
    private float horizontalInput;
    private float verticalInput;

    private Transform mainCamera;

    private void Awake()
    {
        characterController_ref = GetComponent<CharacterController>();

        //Main kamerada camera controller scriptinin olmama durumunda ona camera controller scriptini ekliyoruz.
        if (Camera.main.GetComponent<CameraController>() == null)
        {
            Camera.main.gameObject.AddComponent<CameraController>();
        }
        mainCamera = GameObject.FindWithTag("CameraPoint").transform;
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
        Move();

        Rotate();
    }

    private void Rotate()
    {
        //Mouseun sað sol hareketini playera veren kod.  x ve z açýlarýný aynen aldýk. y açýsýna mousedan gelen x deðerini ekledik.gelen artý veya eksi x deðerine göre kamera saða sola dönecek.
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + MouseInput().x, transform.eulerAngles.z);

        if (mainCamera != null)
        {   //Mouseun yukarý aþaðý hareketini kameraya veren kod.
            mainCamera.rotation = Quaternion.Euler(mainCamera.rotation.eulerAngles + new Vector3( -MouseInput().y, 0f, 0f));

            //karakterin yukarý aþaðý bakma açýsýný limitedik.
            print(mainCamera.eulerAngles.x);
            if (mainCamera.eulerAngles.x >maxViewAngle && mainCamera.eulerAngles.x < 180f)
            {
                mainCamera.rotation = Quaternion.Euler(maxViewAngle, mainCamera.eulerAngles.y, mainCamera.eulerAngles.z);

            }else if (mainCamera.eulerAngles.x > 180f && mainCamera.eulerAngles.x < 360f - maxViewAngle)
            {
                mainCamera.rotation = Quaternion.Euler(360 - maxViewAngle, mainCamera.eulerAngles.y, mainCamera.eulerAngles.z);
            }
        }
    }

    private void Move()
    {
        //global hareket eden vektörler baktýðýmýz yöne gitmek için local vectör haline çevirdik.
        Vector3 localVerticalVector = transform.forward * verticalInput;
        Vector3 localHorizontalVector = transform.right * horizontalInput;

        Vector3 movementVector = localHorizontalVector + localVerticalVector;

        movementVector.Normalize();                             //çapraz gitme komutu verildiðinde vektör toplamasý sonucu karakterin olduðundan daha fazla hýzlý gitme
        movementVector *= currentSpeed * Time.deltaTime;        //problemini, vektörü önce normalize edip daha sonra hýz bilgisini ekleyerek çözdük.

        characterController_ref.Move(movementVector);
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

    //Ýstediðimiz yerden mouse input deðerlerine ulaþmak için yazýlan fonksiyon.
    private Vector2 MouseInput()
    {
        return new Vector2 (Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensivity;
    }

}

