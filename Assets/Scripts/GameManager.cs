using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CharacterStatOption
{
    public Vector3Int stats; // x = programming, y = art, z = writing
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

    void Awake() => Instance = this;

    void Start()
    {
        if (PlayerProgression.Instance.savedCharacters == null || 
            PlayerProgression.Instance.savedCharacters.Count == 0)
        {
            GenerateCharacters();
            SaveCharactersToProgression();
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
        Vector3Int selectedStats = PickRandomStats();

        // create character object
        Character instance = Instantiate(generalCharacterPrefab);

        instance.programming = selectedStats.x;
        instance.art = selectedStats.y;
        instance.writing = selectedStats.z;

        // save stats to permanent storage
        CharacterData data = new CharacterData()
        {
            programming = selectedStats.x,
            art = selectedStats.y,
            writing = selectedStats.z
        };
        PlayerProgression.Instance.savedCharacters.Add(data);

        // put character in slot
        Transform slot = slots[i];
        AssignCharacterToSlot(instance, slot, slot.GetComponent<CharacterSlot>());
        activeCharacters.Add(instance);
    }
    }

    Vector3Int PickRandomStats()
    {
        // sum all chances
        float totalChance = 0f;
        foreach (var option in statOptions)
            totalChance += option.chance;

        float roll = Random.value * totalChance;
        float cumulative = 0f;

        foreach (var option in statOptions)
        {
            cumulative += option.chance;
            if (roll <= cumulative)
                return option.stats;
        }

        // fallback: last option
        return statOptions[statOptions.Count - 1].stats;
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
        fused.programming = Mathf.RoundToInt((a.programming + b.programming) * 0.8f);
        fused.art = Mathf.RoundToInt((a.art * 0.5f + b.art * 0.5f) + 2);
        fused.writing = Mathf.Max(a.writing, b.writing);
        fused.chance = (a.chance + b.chance) * 0.5f;

        AssignCharacterToSlot(fused, targetSlot.transform, targetSlot);
        activeCharacters.Add(fused);
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

    void SaveCharactersToProgression()
    {
        PlayerProgression.Instance.savedCharacters.Clear();

        foreach (Character c in activeCharacters)
        {
            CharacterData data = new CharacterData();
            data.programming = c.programming;
            data.art = c.art;
            data.writing = c.writing;

            PlayerProgression.Instance.savedCharacters.Add(data);
        }
    }

    void LoadCharactersFromProgression()
    {
        ClearCharacters();

        int index = 0;

        foreach (CharacterData data in PlayerProgression.Instance.savedCharacters)
        {
            Character instance = Instantiate(generalCharacterPrefab);

            instance.programming = data.programming;
            instance.art = data.art;
            instance.writing = data.writing;

            Transform slot = slots[index];
            CharacterSlot slotScript = slot.GetComponent<CharacterSlot>();
            AssignCharacterToSlot(instance, slot, slotScript);

            activeCharacters.Add(instance);

            index++;
        }
    }
}
