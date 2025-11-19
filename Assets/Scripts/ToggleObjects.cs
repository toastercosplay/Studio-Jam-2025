using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject offObject;

    public void ActivateTargetAndDisableSelf()
    {
        if (targetObject != null)
            targetObject.SetActive(true);

        offObject.SetActive(false);
    }
}
