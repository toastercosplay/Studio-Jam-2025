using UnityEngine;
using System.Collections.Generic;

public class PlayerProgression : MonoBehaviour
{
    public static PlayerProgression Instance;

    public int currentDifficulty = 0;
    public int coins = 20;
    public int pullCost = 30;
    public int gamesCompleted = 0;

    public List<CharacterData> savedCharacters = new List<CharacterData>();

    public GameData savedGame;

    public List<float> statChances = new List<float>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        coins = 20;

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void EnsureChanceListSize(int requiredCount, List<CharacterStatOption> defaultOptions)
    {
        //first-time setup: list is empty
        if (statChances.Count == 0)
        {
            statChances.Clear();
            foreach (var option in defaultOptions)
                statChances.Add(option.chance); // copy initial chance values
            return;
        }

        //if size mismatches (e.g., you add a new tier), fix it cleanly
        while (statChances.Count < requiredCount)
            statChances.Add(defaultOptions[statChances.Count].chance);

        while (statChances.Count > requiredCount)
            statChances.RemoveAt(statChances.Count - 1);
    }
}

[System.Serializable]
public class CharacterData
{
    public int programming;
    public int art;
    public int writing;
    public int stars;
}

[System.Serializable]
public class GameData
{
    public int programming;
    public int art;
    public int writing;
    public int diffTier;
}
