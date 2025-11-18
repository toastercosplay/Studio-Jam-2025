using UnityEngine;

public class Shopping : MonoBehaviour
{
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void progUpgrade()
    {
        PlayerProgression.Instance.coins -= 10;
        PlayerProgression.Instance.statChances[3] *= 5f;

    }
    public void artUpgrade()
    {
        PlayerProgression.Instance.coins -= 10;
        PlayerProgression.Instance.statChances[4] *= 5f;

    }
    public void writeUpgrade()
    {
        PlayerProgression.Instance.coins -= 10;
        PlayerProgression.Instance.statChances[5] *= 5f;

    }

    public void interUpgrade()
    {
        PlayerProgression.Instance.coins -= 10;
        PlayerProgression.Instance.statChances[6] *= 5f;

    }
}
