using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StatImageMapping
{
    //using vector2s for ranges
    public Vector2Int progRange;
    public Vector2Int artRange;
    public Vector2Int writingRange;
    public Sprite sprite;
    public int stars;
}

public class ImageSelect : MonoBehaviour
{
    
    public Image targetImage;
    public Character myself;
    public StatImageMapping[] mappings;

    [SerializeField] public Sprite blueCapsule;
    [SerializeField] public Sprite pinkCapsule;
    [SerializeField] public Sprite yellowCapsule;
    [SerializeField] public Sprite greenCapsule;

    Scene m_Scene;
    string sceneName;

    void Start()
    {
        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;
        
        
        if (myself == null)
        {
            myself = GetComponent<Character>();
        }
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }

        if(sceneName == "GachaScene")
        {
            //setting based on stats in gacha scene (concealing actual)
            if(myself.programming > myself.art && myself.programming > myself.writing)
            {
                targetImage.sprite = blueCapsule;
            }
            else if(myself.art > myself.programming && myself.art > myself.writing)
            {
                targetImage.sprite = pinkCapsule;
            }
            else if(myself.writing > myself.programming && myself.writing > myself.art)
            {
                targetImage.sprite = yellowCapsule;
            }
            else
            {
                targetImage.sprite = greenCapsule;
            }
            return;
        }

        foreach (var mapping in mappings) //special mapping
        {
            if (myself.programming >= mapping.progRange.x && myself.programming <= mapping.progRange.y &&
                myself.art >= mapping.artRange.x && myself.art <= mapping.artRange.y &&
                myself.writing >= mapping.writingRange.x && myself.writing <= mapping.writingRange.y)
            {
                targetImage.sprite = mapping.sprite;
                //myself.setStars(mapping.stars);
                //Debug.Log($"Matched {mapping.name}");
                return;
            }
        }

        //Debug.LogWarning("No matching sprite for this character");
    }
}
