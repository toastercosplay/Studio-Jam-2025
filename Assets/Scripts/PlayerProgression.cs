using UnityEngine;
using System.Collections.Generic;

public class PlayerProgression : MonoBehaviour
{
    public static PlayerProgression Instance;//there can only be one

    public int currentDifficulty = 1;
    public int coins = 0;
    public int gamesCompleted = 0;

    public List<CharacterData> savedCharacters = new List<CharacterData>();
    public GameData savedGame;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

[System.Serializable]
public class CharacterData
{
    public int programming;
    public int art;
    public int writing;
}

[System.Serializable]
public class GameData
{
    public int programming;
    public int art;
    public int writing;
    public int diffTier;
}


