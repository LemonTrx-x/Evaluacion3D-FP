using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MinijuegoTV : MonoBehaviour
{
    [Header("Referencias de la Interfaz")]
    public GameObject canvasMinijuego;
    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI textoOperacion;
    public TextMeshProUGUI textoBoton1;
    public TextMeshProUGUI textoBoton2;

    [Header("Mecánica de Salvación")]
    public GameObject botonSecreto;
    public Vector3 coordenadasSalvacion;

    [Header("Control del Jugador")]
    public PlayerInteract scriptInteraccion;
    public MonoBehaviour[] scriptsDeMovimientoYCamara;

    [Header("Configuración")]
    public float tiempoMaximo = 10f;
    private float tiempoRestante;
    private int rondaActual = 1;
    private int respuestaCorrecta;
    private int idBotonCorrecto;
    private bool juegoActivo = false;

    void Start()
    {
        if (canvasMinijuego != null) canvasMinijuego.SetActive(false);
        if (botonSecreto != null) botonSecreto.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!juegoActivo) return;

        tiempoRestante -= Time.deltaTime;
        textoTiempo.text = "Tiempo: " + Mathf.Ceil(tiempoRestante).ToString();

        // --- LÓGICA DE SALVACIÓN ---
        if (tiempoRestante <= 1f && tiempoRestante > 0)
        {
            // Si el botón está apagado, lo encendemos y le damos una posición aleatoria
            if (!botonSecreto.activeSelf)
            {
                MoverBotonAleatoriamente(); // ¡NUEVA FUNCIÓN!
                botonSecreto.SetActive(true);
            }
        }
        else
        {
            if (botonSecreto.activeSelf) botonSecreto.SetActive(false);
        }

        if (tiempoRestante <= 0)
        {
            Morir();
        }
    }

    // --- NUEVO CAMBIO: Calcula una posición al azar en la pantalla ---
    void MoverBotonAleatoriamente()
    {
        // Obtenemos el componente que controla la posición de la UI
        RectTransform rectBoton = botonSecreto.GetComponent<RectTransform>();

        // Calculamos los márgenes para que el botón no aparezca medio cortado fuera de la pantalla
        float margenX = rectBoton.rect.width / 2f;
        float margenY = rectBoton.rect.height / 2f;

        // Generamos coordenadas X e Y al azar dentro de los límites de la pantalla
        float posicionAleatoriaX = Random.Range(margenX, Screen.width - margenX);
        float posicionAleatoriaY = Random.Range(margenY, Screen.height - margenY);

        // Movemos el botón a esa nueva posición
        rectBoton.position = new Vector3(posicionAleatoriaX, posicionAleatoriaY, 0);
    }

    public void IniciarMinijuego()
    {
        canvasMinijuego.SetActive(true);
        juegoActivo = true;
        rondaActual = 1;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (scriptInteraccion != null) scriptInteraccion.enabled = false;
        foreach (MonoBehaviour script in scriptsDeMovimientoYCamara)
        {
            if (script != null) script.enabled = false;
        }

        GenerarNuevaPregunta();
    }

    public void ClickEnBotonSecreto()
    {
        juegoActivo = false;
        canvasMinijuego.SetActive(false);
        botonSecreto.SetActive(false);

        Transform pTransform = scriptInteraccion.jugador;
        CharacterController cc = pTransform.GetComponent<CharacterController>();

        if (cc != null)
        {
            cc.enabled = false;
            pTransform.position = coordenadasSalvacion;
            cc.enabled = true;
        }
        else
        {
            pTransform.position = coordenadasSalvacion;
        }

        scriptInteraccion.enabled = true;
        foreach (MonoBehaviour script in scriptsDeMovimientoYCamara)
        {
            if (script != null) script.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GenerarNuevaPregunta()
    {
        tiempoRestante = tiempoMaximo;
        if (rondaActual <= 3)
        {
            int num1 = Random.Range(1, 11);
            int num2 = Random.Range(1, 11);
            respuestaCorrecta = num1 + num2;
            textoOperacion.text = num1 + " + " + num2;
            int respuestaFalsa = respuestaCorrecta + Random.Range(1, 4);
            idBotonCorrecto = Random.Range(1, 3);
            if (idBotonCorrecto == 1)
            {
                textoBoton1.text = respuestaCorrecta.ToString();
                textoBoton2.text = respuestaFalsa.ToString();
            }
            else
            {
                textoBoton1.text = respuestaFalsa.ToString();
                textoBoton2.text = respuestaCorrecta.ToString();
            }
        }
        else if (rondaActual == 4)
        {
            textoOperacion.text = Random.Range(1000, 9999) + " x " + Random.Range(100, 999);
            textoBoton1.text = "No lo se";
            textoBoton2.text = ":)";
            idBotonCorrecto = 0;
        }
    }

    public void PresionarBoton1()
    {
        if (idBotonCorrecto == 1) { rondaActual++; GenerarNuevaPregunta(); }
        else Morir();
    }

    public void PresionarBoton2()
    {
        if (idBotonCorrecto == 2) { rondaActual++; GenerarNuevaPregunta(); }
        else Morir();
    }

    void Morir()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}