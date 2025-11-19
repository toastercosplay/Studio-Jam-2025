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

    public List<CharacterStatOption> statOptions;

    public Transform[] slots = new Transform[10];

    int pullCount = 10;
    public int pullCost;

    public static GameManager Instance;
    public List<Character> activeCharacters = new List<Character>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        bool needGeneration = false;
        pullCost = PlayerProgression.Instance.pullCost;

        if (PlayerProgression.Instance.savedCharacters == null || 
            PlayerProgression.Instance.savedCharacters.Count < pullCount)
        {
            needGeneration = true;
        }
        else
        {
            //expired failsafe for a bug, but doesnt hurt to keep
            foreach (var c in PlayerProgression.Instance.savedCharacters)
            {
                if (c.stars <= 0)
                {
                    needGeneration = true;
                    break;
                }
            }
        }

        if (needGeneration)
        {
            GenerateCharacters();
        }
        else
        {
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

            //instantiate character
            Character instance = Instantiate(generalCharacterPrefab);
            instance.programming = stats.x;
            instance.art = stats.y;
            instance.writing = stats.z;
            instance.setStars(stars);

            //Debug.Log($"Generated character with stats: P:{instance.programming} A:{instance.art} W:{instance.writing} Stars:{instance.stars}");

            //assign to slot
            Transform slot = slots[i];
            AssignCharacterToSlot(instance, slot, slot.GetComponent<CharacterSlot>());
            activeCharacters.Add(instance);
        }
        SaveCharactersToProgression();
    }

    private (CharacterStatOption option, Vector3Int stats, int stars) PickRandomStats()
    {
        //in case lists are different (they shouldnt be)
        if (PlayerProgression.Instance.statChances.Count != statOptions.Count)
        {
            Debug.LogError("Chance list and stat options list size mismatch!");
            return (statOptions[0], statOptions[0].stats, statOptions[0].stars);
        }

        //sum all chances from PlayerProgression
        float totalChance = 0f;
        foreach (float c in PlayerProgression.Instance.statChances)
            totalChance += c;

        // roll
        float roll = Random.value * totalChance;
        float cumulative = 0f;

        //step through statOptions using PlayerProgression chances
        for (int i = 0; i < statOptions.Count; i++)
        {
            cumulative += PlayerProgression.Instance.statChances[i];

            if (roll <= cumulative)
            {
                CharacterStatOption opt = statOptions[i];
                return (opt, opt.stats, opt.stars);
            }
        }

        //fallback: return last
        var last = statOptions[statOptions.Count - 1];
        return (last, last.stats, last.stars);
    }

    public void FuseCharacters(Character a, Character b)
    {
        //Debug.Log($"Fusing characters: A(P:{a.programming} A:{a.art} W:{a.writing} S:{a.stars}) + B(P:{b.programming} A:{b.art} W:{b.writing} S:{b.stars})");
        
        
        CharacterSlot targetSlot = a.currentSlot;

        activeCharacters.Remove(a);
        activeCharacters.Remove(b);

        Destroy(a.gameObject);
        Destroy(b.gameObject);

        Character fused = Instantiate(generalCharacterPrefab);

        //fusion formulas 

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
        }
        //Debug.Log(fused.programming + " " + fused.art + " " + fused.writing + " " + fused.stars);
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

    public void ClearCharacters()
    {
        //Debug.Log("Clearing existing characters...");
        
        foreach (var c in activeCharacters)
            if (c != null) Destroy(c.gameObject);

        activeCharacters.Clear();

        foreach (var s in slots)
        {
            var cs = s.GetComponent<CharacterSlot>();
            cs.myCharacter = null;
        }
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
            CharacterSlot slotComponent = slot.GetComponent<CharacterSlot>();

            // FORCE CLEAN BINDINGS
            instance.currentSlot = slotComponent;
            slotComponent.myCharacter = instance;

            AssignCharacterToSlot(instance, slot, slotComponent);

            activeCharacters.Add(instance);
        }
  
    }

    public void PayForCharacters()
    {
        if(PlayerProgression.Instance.coins < pullCost)
        {
            //Debug.Log("Not enough coins to pull characters!");
            return;
        }

        PlayerProgression.Instance.coins -= pullCost;
        
        ClearCharacters();

        PlayerProgression.Instance.savedCharacters.Clear();

        for (int i = 0; i < pullCount; i++)
        {
            var chosen = PickRandomStats();
            Vector3Int stats = chosen.stats;
            int stars = chosen.stars;

            //instantiate character
            Character instance = Instantiate(generalCharacterPrefab);
            instance.programming = stats.x;
            instance.art = stats.y;
            instance.writing = stats.z;
            instance.setStars(stars);

            //Debug.Log($"Generated character with stats: P:{instance.programming} A:{instance.art} W:{instance.writing} Stars:{instance.stars}");

            //assign to slot
            Transform slot = slots[i];
            AssignCharacterToSlot(instance, slot, slot.GetComponent<CharacterSlot>());
            activeCharacters.Add(instance);
        }
        SaveCharactersToProgression();
    }
}

