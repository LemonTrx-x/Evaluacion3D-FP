using UnityEngine;

public class Button : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public Transform jugador;
    public float rangoInteraccion = 3f;

    [Header("Destino del Teletransporte")]
    public Vector3 coordenadasDestino = new Vector3(10f, 1f, 10f);

    private bool enRango = false;

    private GUIStyle estiloPersonalizado;

    void Start()
    {
        // 1. Inicializamos el estilo
        estiloPersonalizado = new GUIStyle();

        // 2. Creamos la textura de fondo blanco
        Texture2D fondoBlanco = new Texture2D(1, 1);
        fondoBlanco.SetPixel(0, 0, Color.white);
        fondoBlanco.Apply();

        // 3. Configuramos fondo blanco y letra negra
        estiloPersonalizado.normal.background = fondoBlanco;
        estiloPersonalizado.normal.textColor = Color.black;

        // 4. Centramos el texto y ajustamos la fuente
        estiloPersonalizado.alignment = TextAnchor.MiddleCenter;
        // He subido un poco el tamaño de la fuente (de 16 a 20)
        // para que la 'E' sola se vea bien grande y clara en el cuadradito.
        estiloPersonalizado.fontSize = 20;
        estiloPersonalizado.fontStyle = FontStyle.Bold;
    }

    void Update()
    {
        if (jugador == null) return;

        // Calculamos la distancia entre el botón y el jugador
        float distancia = Vector3.Distance(transform.position, jugador.position);

        // Verificamos si estamos dentro del rango
        enRango = distancia <= rangoInteraccion;

        // Interacción
        if (enRango && Input.GetKeyDown(KeyCode.E))
        {
            jugador.position = coordenadasDestino;
        }
    }

    void OnGUI()
    {
        if (enRango)
        {
            // --- CAMBIOS AQUÍ: GUI más pequeño ---
            // Definimos un cuadrado pequeño (ej: 35x35 píxeles)
            float tamano = 35f;

            float posX = (Screen.width - tamano) / 2;
            float posY = (Screen.height - tamano) / 2 + 50f;

            // --- CAMBIOS AQUÍ: Texto solo "E" ---
            GUI.Box(new Rect(posX, posY, tamano, tamano), "E", estiloPersonalizado);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoInteraccion);
    }
}