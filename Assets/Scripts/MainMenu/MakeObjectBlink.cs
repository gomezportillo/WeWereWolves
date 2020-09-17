using UnityEngine;
using UnityEngine.UI;

public class MakeObjectBlink : MonoBehaviour
{
    [SerializeField]
    private float timeToBlink = 1f;
    private bool isEnabled = true;

    void Start()
    {
        Blink();
    }

    void Blink()
    {
        GetComponent<TMPro.TextMeshProUGUI>().enabled = isEnabled;

        isEnabled = !isEnabled;
        Invoke("Blink", timeToBlink);
    }
}
