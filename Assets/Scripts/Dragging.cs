using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Dragging : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Character character;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector3 startPosition;
    private Transform startParent;

    public float scaleMultiplier = 1.75f; //how much bigger when hovered
    public float lerpSpeed = 10f; //smoothing speed
    private Vector3 originalScale;
    private Vector3 targetScale;

    Scene m_Scene;
    string sceneName;
    
    void Awake()
    {
        character = GetComponent<Character>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        originalScale = transform.localScale;
        targetScale = originalScale;

        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;

        //find the canvas
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
           canvas = FindFirstObjectByType<Canvas>(); 
        }
            
    }

    void Update()
    {
        if(sceneName != "GachaScene")
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * lerpSpeed);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(sceneName != "GachaScene")
        {
            startPosition = rectTransform.position;
            startParent = transform.parent;

            canvasGroup.blocksRaycasts = false; //important for some reason
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(sceneName != "GachaScene")
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out pos);
            rectTransform.position = canvas.transform.TransformPoint(pos);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(sceneName != "GachaScene")
        {
            canvasGroup.blocksRaycasts = true;

            //if droped nowehere just snap snap back
            rectTransform.position = startPosition;
            transform.SetParent(startParent);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(sceneName != "GachaScene")
        {
            targetScale = originalScale * scaleMultiplier;
        }

        if (transform.parent != null)
        {
            transform.parent.SetAsLastSibling();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(sceneName != "GachaScene")
        {
            targetScale = originalScale;
        }
    }
}
