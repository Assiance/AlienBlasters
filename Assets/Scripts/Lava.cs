using UnityEngine;
using UnityEngine.SceneManagement;

public class Lava : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            SceneManager.LoadScene(0);
    }
}
