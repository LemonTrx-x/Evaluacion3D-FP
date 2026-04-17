using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogoEsqueleto : MonoBehaviour
{
    [Header("Referencias de la Interfaz")]
    public TextMeshProUGUI textoDialogoTMP;
    public GameObject panelBurbuja;

    [Header("Animación del Esqueleto")]
    [Tooltip("Arrastra aquí el objeto del esqueleto que tiene el Animator")]
    public Animator animadorEsqueleto; // <--- NUEVO

    [Header("Contenido de la Misión")]
    [TextArea(3, 10)]
    public string[] frasesDeMision;

    [Header("Configuración del Regaño")]
    public string fraseRecordatorio = "¿Qué estás mirando? ¡Ve a por mi cangrejo!";
    public float tiempoSilencio = 5f;

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

        // Si no asignaste el animador, intentamos buscarlo
        if (animadorEsqueleto == null) animadorEsqueleto = GetComponent<Animator>();

        IniciarDialogo();
    }

    void Update()
    {
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

        // Resetear el enojo al empezar por si acaso
        if (animadorEsqueleto != null) animadorEsqueleto.SetBool("estaEnojado", false);

        EmpezarNuevaFrase();
    }

    void ManejarInputJugador()
    {
        if (estaEscribiendo)
        {
            StopCoroutine(corrutinaEscritura);
            textoDialogoTMP.text = fraseCompletaActual;
            estaEscribiendo = false;
        }
        else
        {
            if (recordatorioActivo)
            {
                SceneManager.LoadScene(nombreEscenaDestino);
                return;
            }

            indiceActual++;
            if (indiceActual < frasesDeMision.Length)
            {
                EmpezarNuevaFrase();
            }
            else
            {
                StartCoroutine(SecuenciaSilencioYRegaño());
            }
        }
    }

    void EmpezarNuevaFrase()
    {
        // --- PUNTO 1: COMPROBACIÓN DEL TEXTO 5 ---
        if (animadorEsqueleto != null)
        {
            if (indiceActual == 5)
            {
                // Si es la frase 5, ¡se enoja!
                animadorEsqueleto.SetBool("estaEnojado", true);
            }
            else
            {
                // --- NUEVO: APAGAR EL ENOJO ---
                // Si es cualquier OTRA frase, le decimos que se relaje
                animadorEsqueleto.SetBool("estaEnojado", false);
            }
        }

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

    IEnumerator SecuenciaSilencioYRegaño()
    {
        esperandoParaRegaño = true;
        panelBurbuja.SetActive(false);

        yield return new WaitForSeconds(tiempoSilencio);

        // --- PUNTO 2: ACTIVACIÓN POR ESPERA ---
        if (animadorEsqueleto != null)
        {
            animadorEsqueleto.SetBool("estaEnojado", true);
        }

        esperandoParaRegaño = false;
        recordatorioActivo = true;
        panelBurbuja.SetActive(true);
        fraseCompletaActual = fraseRecordatorio;
        corrutinaEscritura = StartCoroutine(EscribirLetras());
    }
}