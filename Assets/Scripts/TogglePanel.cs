using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    public GameObject panelToToggle; // Arrastra aquí el Canvas o Panel desde el Inspector
    public KeyCode toggleKey = KeyCode.M; // Puedes cambiar la tecla aquí

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (panelToToggle != null)
            {
                panelToToggle.SetActive(!panelToToggle.activeSelf);
            }
        }
    }
}