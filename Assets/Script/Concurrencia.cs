using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class Concurrencia : MonoBehaviour
{

    [Header("Activa los métodos")]
    public bool useSincrono;
    public bool useThread;
    public bool usetask;
    public bool useCorutine; 

    [Header("Esfera a mover")]
    public Transform SincronoSphere;
    public Transform ThreadSphere;
    public Transform taskSphere;
    public Transform CoroutineSphere; 
    public Transform mainCube;

    //Acciones a ejecutar en el hilo secuandario

    private Queue<Action> mainThreadActions = new Queue<Action>();



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(useSincrono) MoveSincrono();
        if(useThread) MoveWithThread();
        if(usetask) MoveWithTask();
        if(useCorutine) StartCoroutine(MoveWithCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        //Siempre gira el cubo de referencia
        mainCube.Rotate(Vector3.up, 50*Time.deltaTime);

        //Ejeecuta las accoines en el hilo principal
        lock (mainThreadActions)
        {
            while (mainThreadActions.Count > 0)
            {
                mainThreadActions.Dequeue().Invoke();
            }
        }
    }

    //Método para herramientas de concurrencia

    void MoveSincrono()
    {
        for(int i = 0; i <= 100; i ++)
        {
            SincronoSphere.position += Vector3.right * 0.05f;
        }
        Thread.Sleep(50);
    }

    //Movimiento con hilo secndario
    void MoveWithThread()
    {
        new Thread(() =>
        {
            for(int i = 0; i <= 100; i ++)
            {
                Thread.Sleep(50);
                lock (mainThreadActions)
                {
                    mainThreadActions.Enqueue(() =>
                    {
                        ThreadSphere.position += Vector3.right *0.05f;
                    });
                }
            }
        }).Start();

    }

    //Task

    async void MoveWithTask()
    {
        await Task.Run(() =>
        {
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(50);

                lock (mainThreadActions)
                {
                    mainThreadActions.Enqueue(() =>
                    {
                        taskSphere.position += Vector3.right * 0.05f;
                    });
                }
            }
        });

    }

    //Corrutina

    IEnumerator MoveWithCoroutine()
    {
        for(int i = 0; i <= 100; i ++)
        {
            CoroutineSphere.position += Vector3.right * 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
