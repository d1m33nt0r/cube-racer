using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FpsCounter))]
public class FpsDisplay : MonoBehaviour
{
    [SerializeField]
    private Text label;

    private FpsCounter fpsCounter;

    private void Awake()
    {
        fpsCounter = GetComponent<FpsCounter>();
    }

    private void Update()
    {
        label.text = fpsCounter.FPS.ToString();
    }
}
