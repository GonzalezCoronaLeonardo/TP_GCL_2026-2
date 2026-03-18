using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;


public class Flight : MonoBehaviour
{
    public float speed = 50f;
    public float rotationSpeed = 100f;
    public Transform cameraTransform;
    public Vector2 movementInput;

    //Controles e iteraciones
    public int turbulenceiterations = 1000000;

    //lista de vectores de posición calculados 
    private List<Vector3> turbulenceForces = new List<Vector3>();

    //Metodo para mover la nave

    public void OnMovement(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraTransform == null)
        {
            Debug.LogError("No hay cámara asignada");
            return;
        }

        //Actividad 1: Proceso pesado que consume recursos
        SimulateTurbulence();

        //Movimiento de la nave
        Vector3 moveDirection = cameraTransform.forward * movementInput.y * speed * Time.deltaTime;
        this.transform.position += moveDirection;

        //Rotación de la nave
        float yaw = movementInput.x * rotationSpeed * Time.deltaTime;
        this.transform.Rotate(0, yaw, 0);
    }

    public void SimulateTurbulence()
    {
        turbulenceForces.Clear();

        //Repetitciones

        for (int i = 0; i < turbulenceiterations; i++)
        {
            //Generar una fuerza de turbulencia aleatoria
            Vector3 turbulenceForce = new Vector3(
                Mathf.PerlinNoise(i * 0.001f, Time.time) * 2 - 1,
                Mathf.PerlinNoise(i * 0.002f, Time.time) * 2 - 1,
                Mathf.PerlinNoise(i * 0.003f, Time.time) * 2 - 1
            );

            turbulenceForces.Add(turbulenceForce);
        }
    }
}
