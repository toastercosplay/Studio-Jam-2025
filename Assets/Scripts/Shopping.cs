using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Shopping : MonoBehaviour
{
    public List<RectTransform> imagesInScene;
    public RectTransform slot1;
    public RectTransform slot2;
    public RectTransform slot3;

    void Start()
    {

        List<RectTransform> imgPool = new List<RectTransform>(imagesInScene);
        for (int i = 0; i < imgPool.Count; i++)
        {
            int swap = Random.Range(i, imgPool.Count);
            (imgPool[i], imgPool[swap]) = (imgPool[swap], imgPool[i]);
        }


        foreach (var img in imgPool)
            img.gameObject.SetActive(false);

        ApplyToSlot(imgPool[0], slot1);
        ApplyToSlot(imgPool[1], slot2);
        ApplyToSlot(imgPool[2], slot3);
    }

    void ApplyToSlot(RectTransform img, RectTransform slot)
    {
        img.gameObject.SetActive(true);

        img.SetParent(slot);
        img.localScale = Vector3.one;

        //fill entire slot
        img.anchorMin = Vector2.zero;
        img.anchorMax = Vector2.one;
        img.offsetMin = Vector2.zero;
        img.offsetMax = Vector2.zero;

        img.pivot = slot.pivot;
    }

    public void progUpgrade()
    {
        if (PlayerProgression.Instance.coins < 10)
            return;
        PlayerProgression.Instance.coins -= 10;
        PlayerProgression.Instance.statChances[3] *= 5f;
    }

    public void artUpgrade()
    {
        if (PlayerProgression.Instance.coins < 10)
            return;
        PlayerProgression.Instance.coins -= 10;
        PlayerProgression.Instance.statChances[4] *= 5f;
    }

    public void writeUpgrade()
    {
        if (PlayerProgression.Instance.coins < 10)
            return;
        PlayerProgression.Instance.coins -= 10;
        PlayerProgression.Instance.statChances[5] *= 5f;
    }

    public void interUpgrade()
    {
        if (PlayerProgression.Instance.coins < 10)
            return;
        PlayerProgression.Instance.coins -= 10;
        PlayerProgression.Instance.statChances[6] *= 5f;
    }

    public void pullUpgrade()
    {
        if (PlayerProgression.Instance.coins < 10 || PlayerProgression.Instance.pullCost < 6)
            return;
        PlayerProgression.Instance.coins -= 10;
        PlayerProgression.Instance.pullCost -= 5;
    }
}
