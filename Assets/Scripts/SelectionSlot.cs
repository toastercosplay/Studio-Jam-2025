using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionSlot : MonoBehaviour, IDropHandler
{
    private ConflictManager conflictManager;

    void Start()
    {
        //find ConflictManager anywhere in the scene
        conflictManager = FindFirstObjectByType<ConflictManager>();

        if (conflictManager == null)
            Debug.LogError("SelectionSlot: No ConflictManager found in the scene!");
    }

    public void OnDrop(PointerEventData eventData)
    {
        //get dropped object and check if it's a Character
        Character droppedCharacter = eventData.pointerDrag?.GetComponent<Character>();

        if (droppedCharacter == null)
            return;

        //Debug.Log($"Character {droppedCharacter.name} dropped on selection slot!");

        //start the conflict with the dropped champion
        conflictManager.StartConflict(droppedCharacter);
    }
}

