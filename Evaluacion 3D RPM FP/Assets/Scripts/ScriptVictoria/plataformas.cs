using UnityEngine;

public class plataformas : MonoBehaviour
{

    public float delay = 0.2f;
    private bool breaking = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!breaking && other.CompareTag("Player"))
        {
            breaking = true;
            StartCoroutine(Break());
        }
    }

    private System.Collections.IEnumerator Break()
    {
        Vector3 originalPos = transform.position;

        for (int i = 0; i < 10; i++)
        {
            transform.position = originalPos + Random.insideUnitSphere * 0.05f;
            yield return new WaitForSeconds(0.02f);
        }

        Destroy(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
