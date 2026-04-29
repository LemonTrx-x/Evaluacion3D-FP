using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public Transform jugador;
    public float rangoInteraccion = 3f;

    [Header("Filtros de Física")]
    public LayerMask capaInteractuable;

    [Header("Configuración de Interfaz (UI)")]
    [Tooltip("Tamaño del recuadro blanco en pantalla")]
    public float tamanoCajaUI = 100f;
    [Tooltip("Tamaño de la letra 'E'")]
    public int tamanoFuenteUI = 80;

    // --- AÑADIDO: Variables para el sonido ---
    [Header("Configuración de Audio")]
    [Tooltip("El sonido que se reproducirá al apretar la E")]
    public AudioClip sonidoInteraccion;
    private AudioSource fuenteDeAudio;
    // ----------------------------------------

    private bool mirandoInteractuable = false;
    private GUIStyle estiloPersonalizado;

    void Start()
    {
        // --- AÑADIDO: Inicializar el AudioSource ---
        // Buscamos un AudioSource. Si el objeto no tiene uno, se lo añadimos automáticamente.
        fuenteDeAudio = GetComponent<AudioSource>();
        if (fuenteDeAudio == null)
        {
            fuenteDeAudio = gameObject.AddComponent<AudioSource>();
        }
        // -------------------------------------------

        estiloPersonalizado = new GUIStyle();

        Texture2D fondoBlanco = new Texture2D(1, 1);
        fondoBlanco.SetPixel(0, 0, Color.white);
        fondoBlanco.Apply();

        estiloPersonalizado.normal.background = fondoBlanco;
        estiloPersonalizado.normal.textColor = Color.black;
        estiloPersonalizado.alignment = TextAnchor.MiddleCenter;
        estiloPersonalizado.fontSize = tamanoFuenteUI;
        estiloPersonalizado.fontStyle = FontStyle.Bold;

        if (capaInteractuable == 0)
        {
            capaInteractuable = LayerMask.GetMask("Interactuable");
        }
    }

    void Update()
    {
        // --- NUEVO: Si el juego está pausado (pantalla de victoria), no hacemos nada ---
        if (Time.timeScale == 0f)
        {
            mirandoInteractuable = false;
            return; 
        }

        mirandoInteractuable = false;

        if (jugador == null) return;

        Ray rayo = new Ray(transform.position, transform.forward);
        RaycastHit impacto;

        if (Physics.Raycast(rayo, out impacto, rangoInteraccion, capaInteractuable))
        {
            InteractuableTV tele = impacto.collider.GetComponent<InteractuableTV>();
            Button boton = impacto.collider.GetComponent<Button>();
            Final scriptFinal = impacto.collider.GetComponent<Final>();

            if (tele != null || boton != null || scriptFinal != null)
            {
                mirandoInteractuable = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // --- AÑADIDO: Reproducimos el sonido de interacción ---
                    // Comprobamos que hayas asignado un sonido en el Inspector
                    if (sonidoInteraccion != null)
                    {
                        fuenteDeAudio.PlayOneShot(sonidoInteraccion);
                    }
                    // ------------------------------------------------------

                    if (tele != null)
                    {
                        tele.Encender();
                    }
                    else if (boton != null)
                    {
                        boton.EjecutarAccion(jugador);
                    }
                    else if (scriptFinal != null)
                    {
                        scriptFinal.TerminarJuego();
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        // --- NUEVO: Evitamos dibujar la "E" si el juego está pausado ---
        if (Time.timeScale == 0f) return;

        if (mirandoInteractuable)
        {
            float tamano = tamanoCajaUI;
            float posX = (Screen.width - tamano) / 2;
            float posY = (Screen.height - tamano) / 2;

            GUI.Box(new Rect(posX, posY, tamano, tamano), "E", estiloPersonalizado);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rangoInteraccion);
    }
}