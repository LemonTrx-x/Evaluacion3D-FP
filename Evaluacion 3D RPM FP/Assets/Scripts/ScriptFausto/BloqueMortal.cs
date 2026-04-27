using UnityEngine;
using TMPro; 
using System.Collections; 

public class BloqueMortal : MonoBehaviour
{
    [Header("Configuración de Teletransporte")]
    public Vector3 coordenadasDestino = new Vector3(0f, 1f, 0f);

    [Header("Configuración de Detección")]
    public string tagJugador = "Player";

    [Header("Configuración de Interfaz")]
    public GameObject panelUI;
    public TextMeshProUGUI textoUI;
    
    [TextArea(3, 5)]
    public string mensajeAMostrar = "¡Fatal Error!";
    public float velocidadTexto = 0.05f;
    public float tiempoDeEspera = 2.0f;

    private void OnTriggerEnter(Collider otro)
    {
        if (otro.gameObject.CompareTag(tagJugador))
        {
            Teletransportar(otro.gameObject);
            MostrarTextoProgresivo();
        }
    }

    private void Teletransportar(GameObject jugador)
    {
        CharacterController cc = jugador.GetComponent<CharacterController>();

        if (cc != null)
        {
            cc.enabled = false;
            jugador.transform.position = coordenadasDestino;
            cc.enabled = true;
        }
        else
        {
            jugador.transform.position = coordenadasDestino;
        }
    }

    private void MostrarTextoProgresivo()
    {
        if (panelUI != null) panelUI.SetActive(true);
        
        if (textoUI != null)
        {
            textoUI.gameObject.SetActive(true);
            StopAllCoroutines(); 
            StartCoroutine(EfectoMecanografiaYDesactivar());
        }
    }

    private IEnumerator EfectoMecanografiaYDesactivar()
    {
        // 1. Asignamos TODO el texto de golpe
        textoUI.text = mensajeAMostrar;
        
        // 2. Le decimos a TextMeshPro que esconda todas las letras (visibles = 0)
        textoUI.maxVisibleCharacters = 0;

        // 3. Vamos revelando las letras una a una
        for (int i = 0; i <= mensajeAMostrar.Length; i++)
        {
            textoUI.maxVisibleCharacters = i;
            yield return new WaitForSeconds(velocidadTexto);
        }

        // 4. Esperamos el tiempo indicado
        yield return new WaitForSeconds(tiempoDeEspera);

        // 5. Apagamos todo
        if (textoUI != null)
        {
            textoUI.gameObject.SetActive(false); 
        }

        if (panelUI != null)
        {
            panelUI.SetActive(false); 
        }
    }
}