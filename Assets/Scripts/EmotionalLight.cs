using UnityEngine;

public class EmotionalLight : MonoBehaviour
{
    private Light myLight;

    void Awake()
    {
        myLight = GetComponent<Light>();
    }

    public void SetEmotionColor(Color color)
    {
        if (myLight != null)
            myLight.color = color;
    }

    public void SetEmotionIntensity(float intensity)
    {
        if (myLight != null)
            myLight.intensity = intensity;
    }
}