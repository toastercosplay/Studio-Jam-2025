using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

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
        myself = GetComponent<Game>();
        targetImage = GetComponent<Image>();

        StartCoroutine(DelayedUpdate());
    }

    IEnumerator DelayedUpdate()
    {
        yield return null; //wait 1 frame
        UpdateImage();
    }

    public void UpdateImage()
    {
        GameData data = PlayerProgression.Instance.savedGame;

        //fallback to Game component if savedGame is null
        int prog = data != null ? data.programming : myself.programming;
        int art  = data != null ? data.art        : myself.art;
        int write= data != null ? data.writing    : myself.writing;

        foreach (var mapping in mappings)
        {
            if (prog == mapping.stats.x &&
                art == mapping.stats.y &&
                write == mapping.stats.z)
            {
                if (mapping.sprites.Count > 0)
                {
                    targetImage.sprite = mapping.sprites[Random.Range(0, mapping.sprites.Count)];
                    return;
                }
            }
        }

        Debug.LogWarning("No matching sprite for game stats: " +
            $"P:{prog} A:{art} W:{write}");
    }
}
