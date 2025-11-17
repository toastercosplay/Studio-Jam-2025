using UnityEngine;

public class StatDotGenerator : MonoBehaviour
{
    public Character character;
    public GameObject blueDotPrefab;
    public GameObject pinkDotPrefab;
    public GameObject yellowDotPrefab;

    public float dotSpacing = 30f;
    public Vector2 programmingStart = new Vector2(0, 0);
    public Vector2 artStart = new Vector2(0, -40);
    public Vector2 writingStart = new Vector2(0, -80);

    public RectTransform dotParent; // UI parent

    void Start()
    {
        character = GetComponent<Character>();
        dotParent = GetComponent<RectTransform>();

        RegenerateDots();
    }

    public void RegenerateDots()
    {
        // delete previous UI dots
        for (int i = dotParent.childCount - 1; i >= 0; i--)
            Destroy(dotParent.GetChild(i).gameObject);

        // 3 rows
        SpawnRowBlue(character.programming, programmingStart);
        SpawnRowPink(character.art, artStart);
        SpawnRowYellow(character.writing, writingStart);
    }

    void SpawnRowYellow(int count, Vector2 startOffset)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject dot = Instantiate(yellowDotPrefab, dotParent);
            RectTransform rt = dot.GetComponent<RectTransform>();

            // UI positioning (anchoredPosition)
            rt.anchoredPosition = new Vector2(
                startOffset.x + (i * dotSpacing),
                startOffset.y
            );

            //dot.AddComponent<DotHoverGrow>(); // optional hover effect
        }
    }
    void SpawnRowBlue(int count, Vector2 startOffset)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject dot = Instantiate(blueDotPrefab, dotParent);
            RectTransform rt = dot.GetComponent<RectTransform>();

            // UI positioning (anchoredPosition)
            rt.anchoredPosition = new Vector2(
                startOffset.x + (i * dotSpacing),
                startOffset.y
            );

            //dot.AddComponent<DotHoverGrow>(); // optional hover effect
        }
    }
    void SpawnRowPink(int count, Vector2 startOffset)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject dot = Instantiate(pinkDotPrefab, dotParent);
            RectTransform rt = dot.GetComponent<RectTransform>();

            // UI positioning (anchoredPosition)
            rt.anchoredPosition = new Vector2(
                startOffset.x + (i * dotSpacing),
                startOffset.y
            );

            //dot.AddComponent<DotHoverGrow>(); // optional hover effect
        }
    }
}
