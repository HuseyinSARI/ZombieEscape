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

   
    //fixed update �al��mak i�in fiziksel i�lemlerin tamamlanmas�n� bekler. update den sonra �al���r.
    private void FixedUpdate()
    {
        Move();

        Rotate();
    }

    private void Rotate()
    {
        //Mouseun sa� sol hareketini playera veren kod.  x ve z a��lar�n� aynen ald�k. y a��s�na mousedan gelen x de�erini ekledik.gelen art� veya eksi x de�erine g�re kamera sa�a sola d�necek.
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + MouseInput().x, transform.eulerAngles.z);

        if (mainCamera != null)
        {   //Mouseun yukar� a�a�� hareketini kameraya veren kod.
            mainCamera.rotation = Quaternion.Euler(mainCamera.rotation.eulerAngles + new Vector3( -MouseInput().y, 0f, 0f));

            //karakterin yukar� a�a�� bakma a��s�n� limitedik.
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
        //global hareket eden vekt�rler bakt���m�z y�ne gitmek i�in local vect�r haline �evirdik.
        Vector3 localVerticalVector = transform.forward * verticalInput;
        Vector3 localHorizontalVector = transform.right * horizontalInput;

        Vector3 movementVector = localHorizontalVector + localVerticalVector;

        movementVector.Normalize();                             //�apraz gitme komutu verildi�inde vekt�r toplamas� sonucu karakterin oldu�undan daha fazla h�zl� gitme
        movementVector *= currentSpeed * Time.deltaTime;        //problemini, vekt�r� �nce normalize edip daha sonra h�z bilgisini ekleyerek ��zd�k.

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

    //�stedi�imiz yerden mouse input de�erlerine ula�mak i�in yaz�lan fonksiyon.
    private Vector2 MouseInput()
    {
        return new Vector2 (Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensivity;
    }

}

