using UnityEngine;

public class GameDotDisplay : MonoBehaviour
{
    public Game game;
    public GameObject blueDotPrefab;
    public GameObject pinkDotPrefab;
    public GameObject yellowDotPrefab;

    public float dotSpacing = 30f;
    public Vector2 programmingStart = new Vector2(0, 0);
    public Vector2 artStart = new Vector2(0, -40);
    public Vector2 writingStart = new Vector2(0, -80);

    public RectTransform dotParent; //UI parent

    void Start()
    {
        game = GetComponent<Game>();
        dotParent = GetComponent<RectTransform>();

        RegenerateDots();
    }

    public void RegenerateDots()
    {
        GameData data = PlayerProgression.Instance.savedGame;

        int prog = data != null ? data.programming : game.programming;
        int art  = data != null ? data.art        : game.art;
        int write= data != null ? data.writing    : game.writing;

        //delete previous UI dots
        for (int i = dotParent.childCount - 1; i >= 0; i--)
            Destroy(dotParent.GetChild(i).gameObject);

        SpawnRowBlue(prog, programmingStart);
        SpawnRowPink(art, artStart);
        SpawnRowYellow(write, writingStart);
    }

    void SpawnRowYellow(int count, Vector2 startOffset)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject dot = Instantiate(yellowDotPrefab, dotParent);
            RectTransform rt = dot.GetComponent<RectTransform>();

            //UI positioning (anchoredPosition)
            rt.anchoredPosition = new Vector2(
                startOffset.x + (i * dotSpacing),
                startOffset.y
            );
        }
    }
    void SpawnRowBlue(int count, Vector2 startOffset)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject dot = Instantiate(blueDotPrefab, dotParent);
            RectTransform rt = dot.GetComponent<RectTransform>();

            //UI positioning (anchoredPosition)
            rt.anchoredPosition = new Vector2(
                startOffset.x + (i * dotSpacing),
                startOffset.y
            );
        }
    }
    void SpawnRowPink(int count, Vector2 startOffset)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject dot = Instantiate(pinkDotPrefab, dotParent);
            RectTransform rt = dot.GetComponent<RectTransform>();

            //UI positioning (anchoredPosition)
            rt.anchoredPosition = new Vector2(
                startOffset.x + (i * dotSpacing),
                startOffset.y
            );
        }
    }
}
