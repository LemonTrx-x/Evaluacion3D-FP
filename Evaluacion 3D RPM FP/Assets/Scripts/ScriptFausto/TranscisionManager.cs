using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement; // <- Necesario para cambiar de escena

public class TransicionManager : MonoBehaviour
{
    [Header("Elementos de la Interfaz")]
    public Image panelNegro;
    public TextMeshProUGUI textoNivel;

    [Header("Ajustes de Tiempo")]
    public float velocidadTexto = 0.1f; 
    public float tiempoLectura = 2f;    

    void Start()
    {
        panelNegro.gameObject.SetActive(false);
        textoNivel.text = "";
    }

    // ------------------------------------------------------------------
    // FUNCIÓN 1: Para teletransportar en la MISMA escena (La que usa tu Botón)
    // ------------------------------------------------------------------
    public void IniciarTransicion(Transform jugador, Vector3 destino, string mensaje)
    {
        StartCoroutine(RutinaTransicion(jugador, destino, mensaje));
    }

    private IEnumerator RutinaTransicion(Transform jugador, Vector3 destino, string mensaje)
    {
        panelNegro.gameObject.SetActive(true);
        textoNivel.text = "";

        CharacterController controlador = jugador.GetComponent<CharacterController>();
        if (controlador != null) controlador.enabled = false;
        jugador.position = destino;
        if (controlador != null) controlador.enabled = true;

        foreach (char letra in mensaje)
        {
            textoNivel.text += letra;
            yield return new WaitForSeconds(velocidadTexto); 
        }

        yield return new WaitForSeconds(tiempoLectura);

        textoNivel.text = "";
        panelNegro.gameObject.SetActive(false);
    }

    // ------------------------------------------------------------------
    // FUNCIÓN 2: Para cambiar de ESCENA (La que usa tu Esqueleto)
    // ------------------------------------------------------------------
    public void IniciarTransicionEscena(string nombreEscena, string mensaje)
    {
        StartCoroutine(RutinaTransicionEscena(nombreEscena, mensaje));
    }

    private IEnumerator RutinaTransicionEscena(string nombreEscena, string mensaje)
    {
        // 1. Ponemos la pantalla en negro
        panelNegro.gameObject.SetActive(true);
        textoNivel.text = "";

        // 2. Efecto de escribir el texto
        foreach (char letra in mensaje)
        {
            textoNivel.text += letra;
            yield return new WaitForSeconds(velocidadTexto);
        }

        // 3. Dejamos que el jugador lea el texto unos segundos
        yield return new WaitForSeconds(tiempoLectura);

        // 4. Cargamos la nueva escena (El panel negro desaparecerá solo al cargar la otra escena)
        SceneManager.LoadScene(nombreEscena);
    }
}