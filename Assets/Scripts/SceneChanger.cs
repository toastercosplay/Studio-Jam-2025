using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    
    [SerializeField] private string sceneName;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }
}
