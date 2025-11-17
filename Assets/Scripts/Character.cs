using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public float chance = 0f;
    [SerializeField] public int stars = 0;

    [SerializeField] public int programming = 0;
    [SerializeField] public int art = 0;
    [SerializeField] public int writing = 0;

    public CharacterSlot currentSlot;

    public void setStars(int count)
    {
        stars = count;
    }
}
