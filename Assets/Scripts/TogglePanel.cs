using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    public GameObject panelToToggle; // Arrastra aqu� el Canvas o Panel desde el Inspector
    public KeyCode toggleKey = KeyCode.M; // Puedes cambiar la tecla aqu�

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