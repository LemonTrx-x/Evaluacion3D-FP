using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public Transform jugador;
    public float rangoInteraccion = 3f;

    // --- NUEVO CAMBIO: Variable para definir qué capas escuchar ---
    [Header("Filtros de Física")]
    public LayerMask capaInteractuable;

    private bool mirandoInteractuable = false;
    private Button botonActual;

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
        estiloPersonalizado.fontSize = 20;
        estiloPersonalizado.fontStyle = FontStyle.Bold;

        // --- OPTIONAL: Auto-asignar la layer por código si está vacía ---
        // Esto busca la layer llamada "Interactuable" por seguridad.
        if (capaInteractuable == 0)
        {
            capaInteractuable = LayerMask.GetMask("Interactuable");
        }
    }

    void Update()
    {
        mirandoInteractuable = false;
        botonActual = null;

        if (jugador == null) return;

        Ray rayo = new Ray(transform.position, transform.forward);
        RaycastHit impacto;

        // --- NUEVO CAMBIO: Raycast Optimizado ---
        // Hemos añadido 'capaInteractuable' al final. 
        // El rayo ignora todo lo que no esté en esa capa.
        if (Physics.Raycast(rayo, out impacto, rangoInteraccion, capaInteractuable))
        {
            // Como ya sabemos que chocó con algo "Interactuable", 
            // buscamos el script Button.
            botonActual = impacto.collider.GetComponent<Button>();

            if (botonActual != null)
            {
                // ¡Lo estamos mirando, está en rango Y es de la layer correcta!
                mirandoInteractuable = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    botonActual.EjecutarAccion(jugador);
                }
            }
        }
    }

    void OnGUI()
    {
        if (mirandoInteractuable)
        {
            float tamano = 35f;
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