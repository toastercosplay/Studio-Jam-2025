using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class StatGameMapping
{
    public Vector3Int stats; //exact stats: x=prog, y=art, z=writing
    public List<Sprite> sprites; //possible images for this exact stat
}

public class GameImages : MonoBehaviour
{
    public Image targetImage;
    public Game myself;
    public List<StatGameMapping> mappings;

    void Start()
    {
        if (myself == null) myself = GetComponent<Game>();
        if (targetImage == null) targetImage = GetComponent<Image>();

        UpdateImage();
    }

    public void UpdateImage()
    {
        foreach (var mapping in mappings)
        {
            if (myself.programming == mapping.stats.x &&
                myself.art == mapping.stats.y &&
                myself.writing == mapping.stats.z)
            {
                //pick a random sprite from the list
                if (mapping.sprites.Count > 0)
                {
                    targetImage.sprite = mapping.sprites[Random.Range(0, mapping.sprites.Count)];
                    return;
                }
            }
        }

        Debug.LogWarning("No matching sprite for this game");
    }
}
