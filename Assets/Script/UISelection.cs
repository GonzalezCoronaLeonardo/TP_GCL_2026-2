using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System;

public class UISelection : MonoBehaviour
{
    public static bool gazeAt;
    [SerializeField]
    public float fillTime = 5f;
    public Image radialImage;
    public UnityEvent onFillComplete; //Evento generico que se asigna al termiar la carga

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Proceso asincrono
    private Coroutine FillCoroutine;
    void Start()
    {
        gazeAt = false;
        radialImage.fillAmount = 0;
    }

    public void OnPointerEnter()
    {
        gazeAt = true;
        
        if (FillCoroutine != null)
        {
            StopCoroutine(FillCoroutine);
        }
        FillCoroutine = StartCoroutine(FillRadial());
    }

    public void OnPointerExit()
    {
        gazeAt = false;
        if (FillCoroutine != null)
        {
            StopCoroutine(FillCoroutine);
            FillCoroutine = null;
        }
        radialImage.fillAmount = 0;
    }

    private IEnumerator FillRadial()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fillTime)
        {
            if (!gazeAt) //Si el usuario deja de mirar el objeto, se detiene la corrutina y se reinicia la imagen
            {
                radialImage.fillAmount = 0;
                yield break;
            }
            elapsedTime += Time.deltaTime;
            radialImage.fillAmount = Mathf.Clamp01(elapsedTime / fillTime);

            yield return null;
        }
    

    //El evento a ejecutar
    onFillComplete?.Invoke();

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
