using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public List<Character> characterPool;   //prefabs to pull from

    public List<Character> activeCharacters = new List<Character>();  //currently characters 

    public Transform[] slots = new Transform[10];
    int pullCount = 10;


    //generating the characters
    public void GenerateCharacters()
    {
        ClearCharacters();

        for (int i = 0; i < pullCount; i++)
        {
            Character selected = PickRandomCharacter();

            // pick the slot for this character automatically 
            Transform slot = slots[i];

            Character instance = Instantiate(selected, slot.position, Quaternion.identity, slot);

            activeCharacters.Add(instance);
        }

        UpdateSlotLayout();
    }


    //random selection based on chance
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

        // fallback (rare)
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


    //dynamically update the characters when there are less than ten
    void UpdateSlotLayout()
    {
        int count = activeCharacters.Count;

        if (count == 0)
            return;

        int startIndex = (10 - count) / 2;

        for (int i = 0; i < count; i++)
        {
            Character c = activeCharacters[i];

            Transform targetSlot = slots[startIndex + i];

            c.transform.SetParent(targetSlot);
            c.transform.position = targetSlot.position;
        }
    }
}

