using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;

public class EventUI : MonoBehaviour
{
    public List<GameObject> listaInstrucciones;
    public int currentIndex = 0;
    public List<string> mensajesInstrucciones;
    public TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Actulizar visibilidad de los paneles
        UpdateVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Metodo para actualizar visibilidad de paneles

    private void UpdateVisibility()
    {
        for (int i = 0; i < listaInstrucciones.Count; i++)
        {
            //Solo el panel en el indice actual está activo
            listaInstrucciones[i].SetActive(i == currentIndex);
        }
    }

    //Metodo para cambiar entre paneles

    public void CycleObjects()
    {
        //Incrementa el indice y vuelve al inicio
        currentIndex = (currentIndex + 1) % listaInstrucciones.Count;

        //Actualizar visibilidad
        UpdateVisibility();
    }

    //Metodo para actualizar el metodo mostrado

    private void UpdateText()
    {
        if(mensajesInstrucciones.Count > 0)
        {
            
        }
    }

    //Metodo para cambiar de escena

    public void ChangeSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

      //Metodo para cambiar de escena

    public void ChangeSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    //Metodo para salir de la aplicacion

    public void ExitGame()
    {
        Debug.Log("Va a salir");
        Application.Quit();
        Debug.Log("Ya salio");
    }
}
