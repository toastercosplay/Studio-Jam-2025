using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSlot : MonoBehaviour, IDropHandler
{
    public Character myCharacter;

    public void OnDrop(PointerEventData eventData)
    {
        Dragging dropped = eventData.pointerDrag.GetComponent<Dragging>();

        if (dropped == null)
        {
            return;
        }

        Character droppedChar = dropped.character;

        // if slot empty, just place the character
        if (myCharacter == null)
        {
            myCharacter = droppedChar;
            dropped.transform.SetParent(transform);
            dropped.transform.position = transform.position;
            return;
        }

        // otherwise  fuse existing and dropped
        Character existing = myCharacter;
        GameManager.Instance.FuseCharacters(existing, droppedChar);
    }
}
