using UnityEngine;
using UnityEngine.SceneManagement;

public class ConflictManager : MonoBehaviour
{
    public Character champion;
    public GameData currentGame;

    public string successScene = "SuccessScene";
    public string failureScene = "FailureScene";

    public int currentGameDifficulty = 1;

    //logic to either: 
    //for each of the three stats, if the champion's stat is >= game's stat
    //if all three stats pass, return true
    //else, failure and the game is lost (play animation and change scene)

    //call this from the whatever else
    void Start()
    {
        if (PlayerProgression.Instance.savedGame == null)
        GenerateNewGame();

        currentGame = PlayerProgression.Instance.savedGame;
    }

    void GenerateNewGame()
    {
        GameData g = new GameData();

        int diff = PlayerProgression.Instance.currentDifficulty;

        g.programming = Random.Range(0, diff + 1);
        g.art = Random.Range(0, diff + 1);
        g.writing = Random.Range(0, diff + 1);
        g.diffTier = diff;

        // save globally for next scene
        PlayerProgression.Instance.savedGame = g;

        //Debug.Log($"Generated game stats: P:{currentGame.programming} A:{currentGame.art} W:{currentGame.writing}");
    }

    public void StartConflict(Character champ, GameData g)
    {
        champion = champ;
        currentGame = g;

        EvaluateConflict();
    }

    private void EvaluateConflict()
    {
        if (champion == null || currentGame == null)
        {
            Debug.LogError("Champion or Game not set!");
            return;
        }

        bool success = true;

        if (champion.programming < currentGame.programming) success = false;
        if (champion.art < currentGame.art) success = false;
        if (champion.writing < currentGame.writing) success = false;

        if (success)
        {
            Debug.Log("Conflict success!");
            //SceneManager.LoadScene(successScene);
        }
        else
        {
            Debug.Log("Conflict failed!");
            //SceneManager.LoadScene(failureScene);
        }
    }
    
}
