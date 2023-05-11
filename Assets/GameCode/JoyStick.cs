using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image JoyStickBackground;                   // 좌측 조이스틱 백그라운드
    private Image JoyStickImage;                        // 좌축 조이스틱 이미지

    private Vector2 posInput;


    void Start()
    {
        JoyStickBackground = GetComponent<Image>();
        JoyStickImage = transform.GetChild(0).GetComponent<Image>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(JoyStickBackground.rectTransform,
            eventData.position, eventData.pressEventCamera, out posInput))
        {
            posInput.x = posInput.x / (JoyStickBackground.rectTransform.sizeDelta.x);
            posInput.y = posInput.y / (JoyStickBackground.rectTransform.sizeDelta.y);

            // normalize
            if(posInput.magnitude > 1.0f)
            {
                posInput = posInput.normalized;
            }

            // joystick move
            JoyStickImage.rectTransform.anchoredPosition = new Vector2(posInput.x * (JoyStickBackground.rectTransform.sizeDelta.x / 2),
                (posInput.y * (JoyStickBackground.rectTransform.sizeDelta.y) / 2));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 터치 떼면 이미지 가운데 정렬 되게
        posInput = Vector2.zero;
        JoyStickImage.rectTransform.anchoredPosition = Vector2.zero;
    }

    // x 축
    public float inputHorizontal()
    {
        if (posInput.x != 0)
            return posInput.x;
        else
            return Input.GetAxis("Horizontal");
    }

    // y 축
    public float inputVertical()
    {
        if (posInput.y != 0)
            return posInput.y;
        else
            return Input.GetAxis("Vertical");
    }
}
