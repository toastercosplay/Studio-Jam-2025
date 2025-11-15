using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public List<Character> characterPool;   //prefabs to pull from

    public List<Character> activeCharacters = new List<Character>();  //currently characters 

    //static instance (super useful !!!!!!)
    public static GameManager Instance;

    public Character fusedPrefab; //fiox later

    public Transform[] slots = new Transform[10];
    int pullCount = 10;

    void Awake()
    {
        Instance = this;
    }
    
    //generating the characters
    public void GenerateCharacters()
    {
        ClearCharacters();

        for (int i = 0; i < pullCount; i++)
        {
            Character selected = PickRandomCharacter();

            // pick the slot for this character automatically 
            Transform slot = slots[i];

            Character instance = Instantiate(selected);
            activeCharacters.Add(instance);

            CharacterSlot slotScript = slots[i].GetComponent<CharacterSlot>();
            AssignCharacterToSlot(instance, slots[i], slotScript);
        }

        UpdateSlotLayout();
    }


    //random selection based on chance value from character
    Character PickRandomCharacter()
    {
        float totalChance = 0;

        foreach (var c in characterPool)
            totalChance += c.chance;

        float roll = Random.value * totalChance;
        float cumulative = 0;

        foreach (var c in characterPool)
        {
            cumulative += c.chance;
            if (roll <= cumulative)
                return c;
        }

        return characterPool[characterPool.Count - 1];
    }

    void ClearCharacters()
    {
        foreach (var c in activeCharacters)
        {
            if (c != null)
                Destroy(c.gameObject);
        }

        activeCharacters.Clear();
    }


    //dynamically update the character positions / slots when there are less than ten
    void UpdateSlotLayout()
    {
        int count = activeCharacters.Count;
        if (count == 0) return;

        // Keep track of which slots are already occupied (fused character stays in its slot)
        List<int> freeSlotIndices = new List<int>();
        for (int i = 0; i < slots.Length; i++)
        {
            CharacterSlot slotScript = slots[i].GetComponent<CharacterSlot>();
            if (slotScript.myCharacter == null)
                freeSlotIndices.Add(i);
        }

        // Determine center indices
        int centerLeft = 4;   // Slot 4
        int centerRight = 5;  // Slot 5

        // Remove any fused character from list so it doesn't get reassigned
        List<Character> charactersToPlace = new List<Character>(activeCharacters);
        charactersToPlace.RemoveAll(c => c.currentSlot != null && c.currentSlot.myCharacter == c);

        // Place remaining characters symmetrically around the center
        int leftOffset = 0;
        int rightOffset = 0;
        bool placeLeft = true;

        foreach (Character c in charactersToPlace)
        {
            int slotIndex;

            if (placeLeft)
            {
                slotIndex = centerLeft - leftOffset;
                leftOffset++;
            }
            else
            {
                slotIndex = centerRight + rightOffset;
                rightOffset++;
            }

            // Safety check
            if (slotIndex < 0) slotIndex = 0;
            if (slotIndex >= slots.Length) slotIndex = slots.Length - 1;

            AssignCharacterToSlot(c, slots[slotIndex], slots[slotIndex].GetComponent<CharacterSlot>());

            placeLeft = !placeLeft; // alternate left/right
        }
    }

    public void FuseCharacters(Character a, Character b)
    {
        //Debug.Log("Fusing");

        CharacterSlot targetSlot = a.currentSlot;

        activeCharacters.Remove(a);
        activeCharacters.Remove(b);
        
        //remove old
        Destroy(a.gameObject);
        Destroy(b.gameObject);

        //FIX THIS LATER
        Character fused = Instantiate(fusedPrefab, targetSlot.transform.position, Quaternion.identity, targetSlot.transform);

        // 5️⃣ Compute stats (your fusion formula)
        fused.programming = Mathf.RoundToInt((a.programming + b.programming) * 0.8f);
        fused.art = Mathf.RoundToInt((a.art * 0.5f + b.art * 0.5f) + 2);
        fused.writing = Mathf.Max(a.writing, b.writing);
        fused.chance = (a.chance + b.chance) * 0.5f;

        // 6️⃣ Assign fused character to the slot properly
        AssignCharacterToSlot(fused, targetSlot.transform, targetSlot);

        // 7️⃣ Add fused character back into the active list
        activeCharacters.Add(fused);

        // 8️⃣ Recalculate positions for all characters to converge
        UpdateSlotLayout();
    }

    public void AssignCharacterToSlot(Character character, Transform slotTransform, CharacterSlot slot)
    {

        character.currentSlot = slot;
        slot.myCharacter = character;

        //moving and prarenting
        character.transform.SetParent(slotTransform);
        character.transform.position = slotTransform.position;
    }
}

