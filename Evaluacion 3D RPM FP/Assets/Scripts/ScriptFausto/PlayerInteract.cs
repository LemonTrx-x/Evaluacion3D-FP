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

    // Ya no guardamos el boton actual aquí, lo resolvemos en el momento
    private bool mirandoInteractuable = false;
    private GUIStyle estiloPersonalizado;

    void Start()
    {
        // 1. Inicializamos tu estilo exacto
        estiloPersonalizado = new GUIStyle();

        Texture2D fondoBlanco = new Texture2D(1, 1);
        fondoBlanco.SetPixel(0, 0, Color.white);
        fondoBlanco.Apply();

        estiloPersonalizado.normal.background = fondoBlanco;
        estiloPersonalizado.normal.textColor = Color.black;
        estiloPersonalizado.alignment = TextAnchor.MiddleCenter;
        estiloPersonalizado.fontSize = tamanoFuenteUI;
        estiloPersonalizado.fontStyle = FontStyle.Bold;

        // Auto-asignar la layer por código si está vacía
        if (capaInteractuable == 0)
        {
            capaInteractuable = LayerMask.GetMask("Interactuable");
        }
    }

    void Update()
    {
        mirandoInteractuable = false;

        if (jugador == null) return;

        Ray rayo = new Ray(transform.position, transform.forward);
        RaycastHit impacto;

        // El rayo ignora todo lo que no esté en la capa Interactuable
        if (Physics.Raycast(rayo, out impacto, rangoInteraccion, capaInteractuable))
        {
            // --- NUEVO CAMBIO: Buscamos ambos scripts ---
            InteractuableTV tele = impacto.collider.GetComponent<InteractuableTV>();
            Button boton = impacto.collider.GetComponent<Button>();

            // Si el objeto tiene un script de TV O un script de Botón...
            if (tele != null || boton != null)
            {
                // Mostramos la letra E en pantalla
                mirandoInteractuable = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Comprobamos qué es lo que estamos mirando exactamente para actuar
                    if (tele != null)
                    {
                        tele.Encender();
                    }
                    else if (boton != null)
                    {
                        boton.EjecutarAccion(jugador);
                    }
                }
            }
        }
    }

    void OnGUI()
    {
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