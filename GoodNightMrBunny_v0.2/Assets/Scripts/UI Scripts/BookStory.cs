using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookStory : MonoBehaviour
{
    #region Attributes
    private bool _bookHasFinished = false;
    private static BookStory _instance;
    public static BookStory Instance => _instance;
    
    public float duracionTransparencia = 4.0f;
    private float tiempoTranscurrido = 0f;
    [SerializeField]private Image imagenCanvas;

    [SerializeField] private GameObject _loadingCanvas;
    [SerializeField] private GameObject _loadingText;
    #endregion

    #region Methods
    public bool BookHasFinished => _bookHasFinished;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (FindObjectOfType<UserData>().currentLevelPlayed == 1)
        {
            _loadingCanvas.SetActive(false);
            _loadingText.SetActive(false);
        }
        else
        {
            _bookHasFinished = true;
            imagenCanvas.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    public void IsOnLastPage()
    {
        if (GetComponent<Book>().currentPage == 7)
        {
            IniciarTransparenciaGradual();
            this.gameObject.SetActive(false);
        }
    }

    void IniciarTransparenciaGradual()
    {
        // Reiniciar el tiempo transcurrido
        tiempoTranscurrido = 0f;

        // Llamar a la función de actualización cada frame
        InvokeRepeating("ActualizarTransparenciaGradual", 0f, Time.deltaTime);
    }

    void ActualizarTransparenciaGradual()
    {
        // Incrementar el tiempo transcurrido
        tiempoTranscurrido += Time.deltaTime;

        // Calcular el valor de alpha gradualmente
        float alpha = Mathf.Lerp(1f, 0f, tiempoTranscurrido / duracionTransparencia);

        // Actualizar el canal alpha del color del componente Image
        Color nuevoColor = imagenCanvas.color;
        nuevoColor.a = alpha;
        imagenCanvas.color = nuevoColor;

        // Verificar si se alcanzó la transparencia total
        if (tiempoTranscurrido >= duracionTransparencia)
        {
            // Desactivar el componente Image
            imagenCanvas.enabled = false;

            // Detener la actualización
            CancelInvoke("ActualizarTransparenciaGradual");

            _bookHasFinished = true;
            GameManager.Instance.StartGame();
            
        }
    }

    public void SkipStory()
    {
        GetComponent<Book>().currentPage = 7;
        Destroy(GetComponent<Book>());
        _bookHasFinished = true;
        IsOnLastPage();
    }
    #endregion
}
