using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionSlot : MonoBehaviour, IDropHandler
{
    ConflictManager conflictManager;
    public GameData targetGame;// assign the game to test against

    void Start()
    {
        conflictManager = GetComponent<ConflictManager>();
    }

    //called when something is dropped on this slot
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("Something dropped on selection slot.");
        // Check if what was dropped has a Character component
        Character droppedCharacter = eventData.pointerDrag?.GetComponent<Character>();

        if (droppedCharacter != null)
        {
            //Debug.Log($"Character {droppedCharacter.name} dropped on selection slot!");

            //sdtart the conflict using the ConflictManager
            conflictManager.StartConflict(droppedCharacter, targetGame);
        }
    }
}
