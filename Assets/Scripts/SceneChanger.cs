using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    
    [SerializeField] private string sceneName;

    public void ChangeScene(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }
}
