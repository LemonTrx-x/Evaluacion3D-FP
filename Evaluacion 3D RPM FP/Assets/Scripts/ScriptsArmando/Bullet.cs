using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int points = 1;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("CrabShoot"))
        {
            ScoreManager.instance.AddPoints(points);
            Destroy(other.gameObject);
        }
    }
}