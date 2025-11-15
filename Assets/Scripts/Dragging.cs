using UnityEngine;
using UnityEngine.EventSystems;

public class Dragging : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Character character;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector3 startPosition;
    private Transform startParent;
    
    void Awake()
    {
        character = GetComponent<Character>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        //find the canvas
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
           canvas = FindObjectOfType<Canvas>(); 
        }
            
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
        startParent = transform.parent;

        canvasGroup.blocksRaycasts = false; //important for some reason
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out pos);
        rectTransform.position = canvas.transform.TransformPoint(pos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        //if droped nowehere just snap snap back
        rectTransform.position = startPosition;
        transform.SetParent(startParent);
    }
}
