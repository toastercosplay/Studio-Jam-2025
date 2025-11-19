using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ConflictManager : MonoBehaviour
{
    public Character champion;
    public GameData currentGame;
    public Game game;
    public GameDotDisplay dotDisplay;

    public string successScene = "SuccessScene";
    public string failureScene = "FailureScene";

    public List<GameTemplate> gamePool = new List<GameTemplate>();

    void Start()
    {
        int diff = PlayerProgression.Instance.currentDifficulty;
        //Debug.Log($"ConflictManager starting with difficulty {diff}");

        //ALWAYS generate a fresh game if difficulty == 0
        if (diff == 0)
        {
            //Debug.Log("Difficulty is 0 → generating new game automatically.");
            GenerateNewGame();
        }
        else
        {
            //only generate if none exists
            if (PlayerProgression.Instance.savedGame == null)
            {
                //Debug.Log("No existing savedGame → generating new game.");
                GenerateNewGame();
            }
        }

        currentGame = PlayerProgression.Instance.savedGame;

        game.programming = currentGame.programming;
        game.art = currentGame.art;
        game.writing = currentGame.writing;

        //Debug.Log(game.programming + " " + game.art + " " + game.writing);
        //Debug.Log(currentGame.programming + " " + currentGame.art + " " + currentGame.writing);

        dotDisplay.RegenerateDots();

        //Debug.Log($"Loaded Game: P:{currentGame.programming} A:{currentGame.art} W:{currentGame.writing}");
    }

    void GenerateNewGame()
    {
        int diff = PlayerProgression.Instance.currentDifficulty;

        // filter templates to only those matching this tier
        var possibleGames = gamePool.FindAll(g => g.difficultyTier == diff);

        if (possibleGames.Count == 0)
        {
            Debug.LogError($"No game templates exist for difficulty tier {diff}!");
            return;
        }

        // pick random template
        GameTemplate template = possibleGames[Random.Range(0, possibleGames.Count)];

        GameData g = new GameData();
        g.programming = template.programming;
        g.art = template.art;
        g.writing = template.writing;
        g.diffTier = diff;

        //save the new persistent game data
        PlayerProgression.Instance.savedGame = g;

        //immediately sync the scene's Game component
        game.programming = g.programming;
        game.art = g.art;
        game.writing = g.writing;

        //update UI immediately so Start() calls don’t see zeros
        dotDisplay.RegenerateDots();
    }

    public void StartConflict(Character champ)
    {
        champion = champ;
        currentGame = PlayerProgression.Instance.savedGame;

        EvaluateConflict();
    }

    private void EvaluateConflict()
    {
        if (champion == null || currentGame == null)
        {
            Debug.LogError("Champion or Game not set!");
            return;
        }

        //Debug.Log($"Evaluating conflict: Champion P:{champion.programming} A:{champion.art} W:{champion.writing} VS Game P:{currentGame.programming} A:{currentGame.art} W:{currentGame.writing}");

        bool success =
            champion.programming >= currentGame.programming &&
            champion.art >= currentGame.art &&
            champion.writing >= currentGame.writing;

        if (success)
            HandleSuccess();
        else
            HandleFailure();
    }

    private void HandleSuccess()
    {
        //Debug.Log("Conflict SUCCESS!");

        //excess stat reward
        int reward = 0;
        reward += champion.programming - currentGame.programming;
        reward += champion.art - currentGame.art;
        reward += champion.writing - currentGame.writing;

        if (reward < 0) reward = 0;

        PlayerProgression.Instance.coins += reward;
        PlayerProgression.Instance.gamesCompleted++;

        //increase difficulty only AFTER tier 0
        PlayerProgression.Instance.currentDifficulty++;

        //lear the game so next scene generates a new one
        PlayerProgression.Instance.savedGame = null;

        //Debug.Log($"Reward coins: {reward}");
        //Debug.Log($"New difficulty: {PlayerProgression.Instance.currentDifficulty}");

        GameManager.Instance.ClearCharacters();

        SceneManager.LoadScene(successScene);
    }

    private void HandleFailure()
    {
        Debug.Log("Conflict FAILED!");

        PlayerProgression.Instance.savedGame = null;

        SceneManager.LoadScene(failureScene);
    }
}

[System.Serializable]
public class GameTemplate
{
    public int programming;
    public int art;
    public int writing;
    public int difficultyTier;
}
