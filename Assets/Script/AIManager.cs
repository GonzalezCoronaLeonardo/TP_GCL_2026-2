using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public Transform entrance;
    [SerializeField]
    public Transform exit;

    public float detectionRange = 2f;
    public float exitRange = 2f;
    public float entranceRange = 15f;

    List<NavMeshAgent> agents = new List<NavMeshAgent>();

    NavMeshTriangulation triangulation;
    Vector3 entrancePos;

    float detectionRangeSqr;
    float exitRangeSqr;
    float entranceRangeSqr;

    bool gameWon = false;
    System.Random random = new();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entrancePos = entrance.position;
        triangulation = NavMesh.CalculateTriangulation();
        detectionRangeSqr = detectionRange * detectionRange;
        exitRangeSqr = exitRange * exitRange;
        entranceRangeSqr = entranceRange * entranceRange;
        FindAllEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;

        //Detección de enemigos cerca del jugador

        bool playerCaught = false;
        foreach(var agent in agents)
        {
            if(!agent.enabled) continue;

            if((agent.transform.position - playerPos).sqrMagnitude < detectionRange)
            {
                playerCaught = true;
                break;
            }
        }

        //Si el jugador es atrapado, se mueve a la entrada
        if(playerCaught)        {
            MovePlayerToEntrance();
            RelocateAllNPC();

            return;
        }
        
        //Jugador en la salida
        if((playerPos - exit.position).sqrMagnitude < exitRangeSqr)
        {
            gameWon = true;
            Debug.Log("Player has reached the exit. Game won!");
        }

        //Perseguir al jugador
        foreach(var agent in agents)
        {
            if(agent.enabled && !agent.isStopped)
            {
                agent.SetDestination(playerPos);
            }
        }
    }

    //Metodo para llevar a player a la entrada
    public void MovePlayerToEntrance()
    {
        var cc = player.GetComponent<NavMeshAgent>();
        if (cc != null)
        {
            cc.enabled = false;
        }
        player.transform.position = entrancePos;
          if (cc != null)
        {
            cc.enabled = true;
        }
        Debug.Log("Player moved to entrance." + entrancePos);
    }

    //Metodo para posicionar a los enemigos
    void RelocateAllNPC()
    {
        if(triangulation.vertices.Length == 0)
        {
            Debug.LogError("No triangulation vertices found. Ensure the NavMesh is properly baked.");
            return;
        }
        foreach (var agent in agents)
        {
            agent.enabled = false;
            //agent.transform.position = triangulation.vertices[Random.Range(0, triangulation.vertices.Length)];
            agent.enabled = true;
        }
    }

    //Metodo para calcular posiciones validas

    Vector3 GetValidRandomPosition()
    {

            Vector3 pos;
            do
            {
                int i = random.Next(0, triangulation.vertices.Length / 3) * 3;
                Vector3 v1 = triangulation.vertices[triangulation.indices[i]];
                Vector3 v2 = triangulation.vertices[triangulation.indices[i + 1]];
                Vector3 v3 = triangulation.vertices[triangulation.indices[i + 2]];

                float r1 =(float)random.NextDouble();
                float r2 =(float)random.NextDouble();
                if (r1 + r2 > 1f)
                {
                    r1 = 1f - r1;
                    r2 = 1f - r2;
                }
                pos = v1 + r1 * (v2 - v1) + r2 * (v3 - v1);
                    
            }
            while((pos - entrancePos).sqrMagnitude < entranceRangeSqr);

        return pos;
    }

    //Metodo para agregar enemigos a la lista
    
    void FindAllEnemies()
    {
        agents.Clear();
        foreach(var agent in FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None))
        {
            if(agent.gameObject.CompareTag("Enemy"))
            {
                agents.Add(agent);
            }
        }
    }
}
