using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using System.Threading;
using Unity.VisualScripting;

public class FlightThread : MonoBehaviour
{
    public float speed = 50f;
    public float rotationSpeed = 100f;
    public Transform cameraTransform;
    public Vector2 movementInput;

    //Controles e iteraciones
    public int turbulenceiterations = 1000000;

    //lista de vectores de posición calculados 
    private List<Vector3> turbulenceForces = new List<Vector3>();

    //Variable para el hilo secundario
    private Thread turbulenceThread; //La instancia del hilo secundario
    private bool isTurbulenceRunning = false; //Bandera para saber si sigue el calculo
    private bool stopTurbulenceThread = false; //Bandera para detener el hilo
    private float capturedTime; //Variable para capturar el tiempo en el hilo

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

        //Actividad 1: Proceso en hilo secundario

        //Tiempo transcurrido
        capturedTime = Time.time;

        //Proceso pesado en hilo secundario
        if (!isTurbulenceRunning)
        {
            isTurbulenceRunning = true;
            stopTurbulenceThread = false;
            turbulenceThread = new Thread(() =>
            SimulateTurbulence(capturedTime));
            turbulenceThread.Start();

        }

        SimulateTurbulence(capturedTime);

        //Movimiento de la nave
        Vector3 moveDirection = cameraTransform.forward * movementInput.y * speed * Time.deltaTime;
        this.transform.position += moveDirection;

        //Rotación de la nave
        float yaw = movementInput.x * rotationSpeed * Time.deltaTime;
        this.transform.Rotate(0, yaw, 0);
    }

    public void SimulateTurbulence(float time)
    {
        turbulenceForces.Clear();

        //Repetitciones

        for (int i = 0; i < turbulenceiterations; i++)
        {
            //Verificar si se tiene que detener el hilo
            if (stopTurbulenceThread)
            {
                break;
            }
            Vector3 turbulenceForce = new Vector3(
                Mathf.PerlinNoise(i * 0.001f, time) * 2 - 1,
                Mathf.PerlinNoise(i * 0.002f, time) * 2 - 1,
                Mathf.PerlinNoise(i * 0.003f, time) * 2 - 1
            );

            turbulenceForces.Add(turbulenceForce);
        }

        //Señal en consola de inicio del hilo
        Debug.Log("Hilo de turbulencia iniciado en: ");

        //Simulacion completa
        isTurbulenceRunning = false;
    }

    private void OnDestroy()
    {
        //Detener el hilo al destruir el objeto
        stopTurbulenceThread = true;
        if (turbulenceThread != null && turbulenceThread.IsAlive)
        {
            turbulenceThread.Join();
        }
    }
}
