using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CharacterStatOption
{
    public Vector3Int stats; // x = programming, y = art, z = writing
    public int stars;
    public float chance;
}

public class GameManager : MonoBehaviour
{
    public Character generalCharacterPrefab;  // single prefab for all pulls

    public List<CharacterStatOption> statOptions; // assign in Inspector

    public Transform[] slots = new Transform[10];

    int pullCount = 10;

    public static GameManager Instance;
    public List<Character> activeCharacters = new List<Character>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (PlayerProgression.Instance.savedCharacters == null || 
            PlayerProgression.Instance.savedCharacters.Count == 0)
        {
            GenerateCharacters();
            //SaveCharactersToProgression();
        }
        else
        {
            // Restore previously saved characters
            LoadCharactersFromProgression();
        }
    }

    public void GenerateCharacters()
    {
        ClearCharacters();

        PlayerProgression.Instance.savedCharacters.Clear();

        for (int i = 0; i < pullCount; i++)
        {
            var chosen = PickRandomStats();
            Vector3Int stats = chosen.stats;
            int stars = chosen.stars;

            // Instantiate character
            Character instance = Instantiate(generalCharacterPrefab);
            instance.programming = stats.x;
            instance.art = stats.y;
            instance.writing = stats.z;
            instance.setStars(stars);

            //Debug.Log($"Generated character with stats: P:{instance.programming} A:{instance.art} W:{instance.writing} Stars:{instance.stars}");

            // Assign to slot
            Transform slot = slots[i];
            AssignCharacterToSlot(instance, slot, slot.GetComponent<CharacterSlot>());
            activeCharacters.Add(instance);
        }
        SaveCharactersToProgression();
    }

    private (CharacterStatOption option, Vector3Int stats, int stars) PickRandomStats()
    {
        float totalChance = 0f;
        foreach (var option in statOptions)
            totalChance += option.chance;

        float roll = Random.value * totalChance;
        float cumulative = 0f;

        foreach (var option in statOptions)
        {
            cumulative += option.chance;
            if (roll <= cumulative)
            {
                //Debug.Log($"Chosen stats: {option.stats} stars: {option.stars}");
                return (option, option.stats, option.stars);
            }
        }

        var last = statOptions[statOptions.Count - 1];
        return (last, last.stats, last.stars);
    }

    public void FuseCharacters(Character a, Character b)
    {
        CharacterSlot targetSlot = a.currentSlot;

        activeCharacters.Remove(a);
        activeCharacters.Remove(b);

        Destroy(a.gameObject);
        Destroy(b.gameObject);

        Character fused = Instantiate(generalCharacterPrefab);

        // fusion formula

        if (a.programming == b.programming && a.art == b.art && a.writing == b.writing && a.stars == b.stars)
        {
            fused.setStars(a.stars + 1);
            fused.programming = a.programming * (a.stars+ 1);
            fused.art = a.art * (a.stars+ 1);
            fused.writing = a.writing * (a.stars+ 1);
        }
        else
        {
            fused.programming = a.programming + b.programming;
            fused.art = a.art + b.art;
            fused.writing = a.writing + b.writing;
            fused.setStars(Mathf.Max(a.stars, b.stars));
            Debug.Log(fused.programming + " " + fused.art + " " + fused.writing + " " + fused.stars);
        }

        AssignCharacterToSlot(fused, targetSlot.transform, targetSlot);
        activeCharacters.Add(fused);

        SaveCharactersToProgression();
    }

    public void AssignCharacterToSlot(Character character, Transform slotTransform, CharacterSlot slot)
    {
        character.currentSlot = slot;
        slot.myCharacter = character;

        character.transform.SetParent(slotTransform);
        character.transform.position = slotTransform.position;
    }

    void ClearCharacters()
    {
        foreach (var c in activeCharacters)
            if (c != null) Destroy(c.gameObject);

        activeCharacters.Clear();
    }

    private void SaveCharactersToProgression()
    {
        PlayerProgression.Instance.savedCharacters.Clear();

        foreach (Character c in activeCharacters)
        {
            CharacterData data = new CharacterData
            {
                programming = c.programming,
                art = c.art,
                writing = c.writing,
                stars = c.stars
            };
            PlayerProgression.Instance.savedCharacters.Add(data);
        }
    }

    private void LoadCharactersFromProgression()
    {
        ClearCharacters();

        for (int i = 0; i < PlayerProgression.Instance.savedCharacters.Count; i++)
        {
            CharacterData data = PlayerProgression.Instance.savedCharacters[i];
            Character instance = Instantiate(generalCharacterPrefab);

            instance.programming = data.programming;
            instance.art = data.art;
            instance.writing = data.writing;
            instance.setStars(data.stars);

            Transform slot = slots[i];
            AssignCharacterToSlot(instance, slot, slot.GetComponent<CharacterSlot>());
            activeCharacters.Add(instance);
        }
    }
}

