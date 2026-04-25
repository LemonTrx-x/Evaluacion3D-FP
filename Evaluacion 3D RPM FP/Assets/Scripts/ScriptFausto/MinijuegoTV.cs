using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events; 
using TMPro;

public class MinijuegoTV : MonoBehaviour
{
    // =======================================================
    // 📍 COORDENADAS Y TELETRANSPORTE 📍
    // =======================================================
    [Header("===== DESTINO DEL CANGREJO (BOTÓN SECRETO) =====")]
    public Vector3 coordenadasCangrejo;

    [Header("===== DESTINO DE CASTIGO (SI PIERDES) =====")]
    public Vector3 coordenadasMuerte;

    // =======================================================
    // 🎬 TRANSICIONES (¡NUEVO!) 🎬
    // =======================================================
    [Header("--- TRANSICIÓN ---")]
    [Tooltip("Arrastra aquí el objeto que tiene tu script TransicionManager")]
    public TransicionManager gestorTransiciones;
    
    [Tooltip("El texto que saldrá en la pantalla negra al ganar")]
    public string textoVictoria = "Nivel 2: La Oficina...";
    
    [Tooltip("El texto que saldrá en la pantalla negra al perder")]
    public string textoDerrota = "Has fallado. Castigo inminente...";

    // =======================================================
    // 🔗 EVENTOS EXTERNOS 🔗
    // =======================================================
    [Header("--- EVENTOS EXTERNOS ---")]
    public UnityEvent eventoAlAtraparCangrejo; 

    // =======================================================
    // 🖥️ REFERENCIAS DE LA INTERFAZ Y JUEGO 🖥️
    // =======================================================
    [Header("Referencias de la Interfaz")]
    public GameObject canvasMinijuego;
    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI textoOperacion;
    public TextMeshProUGUI textoBoton1;
    public TextMeshProUGUI textoBoton2;

    [Header("El Botón Secreto (UI)")]
    public GameObject botonSecreto;

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

        if (tiempoRestante <= 1f && tiempoRestante > 0)
        {
            if (!botonSecreto.activeSelf)
            {
                MoverBotonAleatoriamente();
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

    void MoverBotonAleatoriamente()
    {
        RectTransform rectBoton = botonSecreto.GetComponent<RectTransform>();
        float margenX = rectBoton.rect.width / 2f;
        float margenY = rectBoton.rect.height / 2f;
        float posicionAleatoriaX = Random.Range(margenX, Screen.width - margenX);
        float posicionAleatoriaY = Random.Range(margenY, Screen.height - margenY);
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
        if (eventoAlAtraparCangrejo != null) eventoAlAtraparCangrejo.Invoke();
        
        // ¡Usamos el texto de victoria!
        TerminarMinijuegoYTeletransportar(coordenadasCangrejo, textoVictoria); 
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
            if (idBotonCorrecto == 1) { textoBoton1.text = respuestaCorrecta.ToString(); textoBoton2.text = respuestaFalsa.ToString(); }
            else { textoBoton1.text = respuestaFalsa.ToString(); textoBoton2.text = respuestaCorrecta.ToString(); }
        }
        else if (rondaActual == 4)
        {
            textoOperacion.text = Random.Range(1000, 9999) + " x " + Random.Range(100, 999);
            textoBoton1.text = "No lo se";
            textoBoton2.text = ":)";
            idBotonCorrecto = 0;
        }
    }

    public void PresionarBoton1() { if (idBotonCorrecto == 1) { rondaActual++; GenerarNuevaPregunta(); } else Morir(); }
    public void PresionarBoton2() { if (idBotonCorrecto == 2) { rondaActual++; GenerarNuevaPregunta(); } else Morir(); }

    void Morir()
    {
        // ¡Usamos el texto de derrota!
        TerminarMinijuegoYTeletransportar(coordenadasMuerte, textoDerrota);
    }

    // --- FUNCIÓN MAESTRA CON EL NUEVO TRANSICION MANAGER ---
    void TerminarMinijuegoYTeletransportar(Vector3 destino, string mensajePantalla)
    {
        juegoActivo = false;
        canvasMinijuego.SetActive(false);
        botonSecreto.SetActive(false);

        // Si conectaste el Manager, él se encarga de todo el teletransporte y efecto
        if (gestorTransiciones != null)
        {
            gestorTransiciones.IniciarTransicion(scriptInteraccion.jugador, destino, mensajePantalla);
        }
        else
        {
            // Fallback: Si se te olvida poner el Manager, hace el teletransporte brusco antiguo
            Transform pTransform = scriptInteraccion.jugador;
            CharacterController cc = pTransform.GetComponent<CharacterController>();
            if (cc != null) { cc.enabled = false; pTransform.position = destino; cc.enabled = true; }
            else { pTransform.position = destino; }
        }

        // Devolvemos el control al jugador
        scriptInteraccion.enabled = true;
        foreach (MonoBehaviour script in scriptsDeMovimientoYCamara)
        {
            if (script != null) script.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}