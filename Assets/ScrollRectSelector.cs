using HelpingMethods;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSelector : MonoBehaviour
{
    
    

    [Header("Settings")]
    [SerializeField] private uint offsetFromBottom;
    [Tooltip("delay before moving to selected child"), Range(1, 255)]
    [SerializeField] private byte framsToWait = 1;


    private ScrollRect ScrollRect => GetComponent<ScrollRect>();


    


    

    public void Select(int childIndex)
    {
        ActionCaller.CallAfterframes(() => MoveTo(childIndex), framsToWait);
    }

    private void MoveTo(int childIndex)
    {
        var rectSize = ScrollRect.GetComponent<RectTransform>().sizeDelta;
        var contentSize = ScrollRect.content.sizeDelta;

        var selectedObject = ScrollRect.content.GetChild(childIndex);



        float value = -selectedObject.localPosition.y - rectSize.y + offsetFromBottom;
        Vector3 contentLocalPosition = ScrollRect.content.localPosition;
        contentLocalPosition.y = Mathf.Clamp(value, 0, contentSize.y);
        ScrollRect.content.localPosition = contentLocalPosition;


    }

}
