using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogoEsqueleto : MonoBehaviour
{
    [Header("Referencias de la Interfaz")]
    public TextMeshProUGUI textoDialogoTMP;
    public GameObject panelBurbuja;

    [Header("Contenido de la Misión")]
    [TextArea(3, 10)]
    public string[] frasesDeMision;

    [Header("Configuración del Regaño")]
    public string fraseRecordatorio = "¿Qué estás mirando? ¡Ve a por mi cangrejo!";
    public float tiempoSilencio = 5f; // Los 5 segundos que querías

    [Header("Configuración de Escena")]
    public string nombreEscenaDestino = "SceneTesting";

    [Header("Configuración de Velocidad")]
    public float velocidadEscritura = 0.05f;

    [Header("Audio")]
    public AudioSource altavoz;
    public AudioClip sonidoLetra;

    private int indiceActual = 0;
    private bool estaEscribiendo = false;
    private bool esperandoParaRegaño = false;
    private bool recordatorioActivo = false;
    private Coroutine corrutinaEscritura;
    private string fraseCompletaActual;

    void Start()
    {
        if (panelBurbuja != null) panelBurbuja.SetActive(false);
        if (altavoz == null) altavoz = GetComponent<AudioSource>();

        IniciarDialogo();
    }

    void Update()
    {
        // Solo detectamos input si NO estamos en los 5 segundos de silencio
        if (!esperandoParaRegaño)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                ManejarInputJugador();
            }
        }
    }

    public void IniciarDialogo()
    {
        if (frasesDeMision.Length == 0) return;
        indiceActual = 0;
        recordatorioActivo = false;
        esperandoParaRegaño = false;
        panelBurbuja.SetActive(true);
        EmpezarNuevaFrase();
    }

    void ManejarInputJugador()
    {
        if (estaEscribiendo)
        {
            // Autocompletar texto si estamos escribiendo
            StopCoroutine(corrutinaEscritura);
            textoDialogoTMP.text = fraseCompletaActual;
            estaEscribiendo = false;
        }
        else
        {
            // Si ya estamos en el recordatorio y el jugador pulsa, TELETRANSPORTAR
            if (recordatorioActivo)
            {
                SceneManager.LoadScene(nombreEscenaDestino);
                return;
            }

            // Pasar a la siguiente frase
            indiceActual++;
            if (indiceActual < frasesDeMision.Length)
            {
                EmpezarNuevaFrase();
            }
            else
            {
                // ¡AQUÍ ESTÁ EL CAMBIO! 
                // Terminaron las frases normales: cerramos y esperamos
                StartCoroutine(SecuenciaSilencioYRegaño());
            }
        }
    }

    void EmpezarNuevaFrase()
    {
        fraseCompletaActual = frasesDeMision[indiceActual];
        corrutinaEscritura = StartCoroutine(EscribirLetras());
    }

    IEnumerator EscribirLetras()
    {
        estaEscribiendo = true;
        textoDialogoTMP.text = "";

        foreach (char letra in fraseCompletaActual.ToCharArray())
        {
            textoDialogoTMP.text += letra;
            if (letra != ' ' && altavoz != null && sonidoLetra != null)
            {
                altavoz.pitch = Random.Range(0.8f, 1.2f);
                altavoz.PlayOneShot(sonidoLetra);
            }
            yield return new WaitForSeconds(velocidadEscritura);
        }
        estaEscribiendo = false;
    }

    // --- NUEVA LÓGICA DE ESPERA ---
    IEnumerator SecuenciaSilencioYRegaño()
    {
        esperandoParaRegaño = true; // Bloqueamos el input del jugador
        panelBurbuja.SetActive(false); // Desaparece el panel

        yield return new WaitForSeconds(tiempoSilencio); // Esperamos los 5 segundos

        // Reaparece el panel con la frase final
        esperandoParaRegaño = false;
        recordatorioActivo = true;
        panelBurbuja.SetActive(true);
        fraseCompletaActual = fraseRecordatorio;
        corrutinaEscritura = StartCoroutine(EscribirLetras());
    }
}