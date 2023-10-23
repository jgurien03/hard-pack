using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public float lerpDuration = 1.0f;
    private bool isLerping = false;
    private float lerpStartTime;
    private Color startColor;
    private Color targetColor;
    private Color startColor1;
    private Color targetColor1;

    public SettingsMenu settings;

    public Image testImage;

    public ButtonHover check;

    void Start()
    {
        // Hide Button2 initially
        button2.gameObject.SetActive(false);
        // Add a click event listener to Button1
        button1.onClick.AddListener(ShowButton2);
    }

    void Update()
    {
        // Check if lerping is in progress
        if (isLerping)
        {
            float t = (Time.unscaledTime - lerpStartTime) / lerpDuration;
            if (t < 1.0f)
            {
                // Lerp the color of the image component
                button2.GetComponent<Image>().color = Color.Lerp(startColor, targetColor, t);
                testImage.GetComponent<Image>().color = Color.Lerp(startColor1, targetColor1, t);
            }
            else
            {
                // Lerp completed
                isLerping = false;
            }
        }
    }

    void ShowButton2()
    {
        // Check if the mouse click occurred within the boundaries of Button 1
        RectTransform rectTransform = button1.GetComponent<RectTransform>();
        Vector2 mousePos = Input.mousePosition;
        bool isMouseWithinButton1Bounds = RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos);

        if (isMouseWithinButton1Bounds)
        {
            // Show Button2 when Button1 is clicked and the mouse is within its bounds
            button2.gameObject.SetActive(true);
            button1.enabled = false;
            check.SetHoverEnabled(false);

            // Set the initial and target colors for lerping
            startColor = button2.GetComponent<Image>().color;
            startColor.a = 0f;
            targetColor = button2.GetComponent<Image>().color;
            targetColor.a = 1f;
            startColor1 = testImage.GetComponent<Image>().color;
            startColor1.a = 1f;
            targetColor1 = testImage.GetComponent<Image>().color;
            targetColor1.a = 0f;

            // Start the lerp
            isLerping = true;
            lerpStartTime = Time.unscaledTime;
        }
    }

    public bool GetButton
    {
        get { return button1.enabled; }
    }
}