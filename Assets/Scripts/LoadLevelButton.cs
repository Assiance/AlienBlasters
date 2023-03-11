using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelButton : MonoBehaviour
{
    [SerializeField] string _level;

    public void Load()
    {
        SceneManager.LoadScene(_level);
    }
}