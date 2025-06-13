using System.Collections.Generic;
using System.IO;  // Necesario para manejar archivos
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LightingController : MonoBehaviour
{
    public Light sceneLight;
    public Image plutchikWheel;
    public RectTransform emotionMarker;
    private string filePath;

    private Dictionary<string, Color> emotionColors = new Dictionary<string, Color>()
    {
        { "alegría", new Color(1f, 0.84f, 0.6f) },
        { "confianza", new Color(0.6f, 1f, 0.6f) },
        { "miedo", new Color(0.3f, 0.3f, 0.6f) },
        { "sorpresa", new Color(1f, 1f, 0.6f) },
        { "tristeza", new Color(0.2f, 0.4f, 0.8f) },
        { "asco", new Color(0.4f, 0.8f, 0.4f) },
        { "ira", new Color(1f, 0.2f, 0.2f) },
        { "anticipación", new Color(1f, 0.6f, 0.2f) }
    };
    private Dictionary<string, string[]> secondaryEmotions = new Dictionary<string, string[]>
{
    { "amor", new string[] { "alegría", "confianza" } },
    { "sumisión", new string[] { "confianza", "miedo" } },
    { "susto", new string[] { "miedo", "sorpresa" } },
    { "decepción", new string[] { "sorpresa", "tristeza" } },
    { "remordimiento", new string[] { "tristeza", "asco" } },
    { "desprecio", new string[] { "asco", "ira" } },
    { "agresión", new string[] { "ira", "anticipación" } },
    { "optimismo", new string[] { "anticipación", "alegría" } }
};

    public Slider alegriaSlider, confianzaSlider, miedoSlider, sorpresaSlider;
    public Slider tristezaSlider, ascoSlider, iraSlider, anticipacionSlider;
    public Button applyButton;

    private Dictionary<string, Vector2[]> emotionRanges = new Dictionary<string, Vector2[]>()
    {
        { "alegría", new Vector2[] { new Vector2(0, 117), new Vector2(24, 117), new Vector2(-27, 120), new Vector2(-1, 136), new Vector2(-1, 105) } },
        { "confianza", new Vector2[] { new Vector2(93, 84), new Vector2(69, 110), new Vector2(107, 99), new Vector2(107, 61), new Vector2(78, 75) } },
        { "miedo", new Vector2[] { new Vector2(130, 4), new Vector2(146, 4), new Vector2(114, 4), new Vector2(130, 32), new Vector2(130, -24) } },
        { "sorpresa", new Vector2[] { new Vector2(90, -78), new Vector2(104, -93), new Vector2(76, -64), new Vector2(113, -59), new Vector2(70, -100) } },
        { "tristeza", new Vector2[] { new Vector2(0, -113), new Vector2(0, -96), new Vector2(0, -127), new Vector2(30, -109), new Vector2(-32, -109) } },
        { "asco", new Vector2[] { new Vector2(-96, -78), new Vector2(-118, -55), new Vector2(-109, -88), new Vector2(-73, -96), new Vector2(-85, -70) } },
        { "ira", new Vector2[] { new Vector2(-133, 0), new Vector2(-133, 32), new Vector2(-133, -22), new Vector2(-114, 0), new Vector2(-152, 0) } },
        { "anticipación", new Vector2[] { new Vector2(-93, 84), new Vector2(-71, 101), new Vector2(-78, 75), new Vector2(-112, 63), new Vector2(-112, 100) } }, 
        // Emociones secundarias
        { "amor", new Vector2[] { new Vector2(69, 150), new Vector2(91, 169), new Vector2(55, 122), new Vector2(48, 156), new Vector2(87, 139) } },
        { "sumisión", new Vector2[] {  new Vector2(167, 64), new Vector2(192, 71), new Vector2(135, 54), new Vector2(177, 42), new Vector2(157, 82) } },
        { "susto", new Vector2[] {  new Vector2(166, -54), new Vector2(192, -67), new Vector2(127, -42), new Vector2(171, -41), new Vector2(153, -74) } },
        { "decepción", new Vector2[] {  new Vector2(73, -149), new Vector2(81, -164), new Vector2(53, -113), new Vector2(45, -154), new Vector2(94, -137) } },
        { "remordimiento", new Vector2[] {  new Vector2(-78, -144), new Vector2(-49, -144), new Vector2(-108, -144), new Vector2(-83, -162), new Vector2(-56, -112) } },
        { "desprecio", new Vector2[] {  new Vector2(-171, -54), new Vector2(-171, -90), new Vector2(-171, -36), new Vector2(-193, -64), new Vector2(-135, -44) } },
        { "agresión", new Vector2[] {  new Vector2(-165, 67), new Vector2(-190, 67), new Vector2(-134, 54), new Vector2(-172, 50), new Vector2(-159, 82) } },
        { "optimismo", new Vector2[] {  new Vector2(-74, 151), new Vector2(-74, 172), new Vector2(-48, 121), new Vector2(-46, 155), new Vector2(-82, 142) } }


    };
    public void OnSliderChanged()
    {
        ApplyEmotionalLighting();
    }
    void Start()
    {

        filePath = Application.persistentDataPath + "/settings.json"; // Ruta del archivo JSON
        profilesPath = Application.persistentDataPath + "/emotion_profiles.json";
        saveProfileButton.onClick.AddListener(SaveProfile);
        LoadFromJson();
        LoadFromJson(); // Cargar valores al iniciar
        applyButton.onClick.AddListener(() => { ApplyEmotionalLighting(); SaveToJson(); });
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        applyButton.onClick.AddListener(ApplyEmotionalLighting);

        alegriaSlider.onValueChanged.AddListener(delegate { OnSliderChanged(); });
        confianzaSlider.onValueChanged.AddListener(delegate { OnSliderChanged(); });
        miedoSlider.onValueChanged.AddListener(delegate { OnSliderChanged(); });
        sorpresaSlider.onValueChanged.AddListener(delegate { OnSliderChanged(); });
        tristezaSlider.onValueChanged.AddListener(delegate { OnSliderChanged(); });
        ascoSlider.onValueChanged.AddListener(delegate { OnSliderChanged(); });
        iraSlider.onValueChanged.AddListener(delegate { OnSliderChanged(); });
        anticipacionSlider.onValueChanged.AddListener(delegate { OnSliderChanged(); });

    }

    void Update()
    {
       
        if (EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(plutchikWheel.rectTransform, Input.mousePosition, null, out localPoint);
            CheckEmotionSelection(localPoint);
        }
    }
   

    private void SaveToJson()
    {
        EmotionalLight[] emotionalLights = FindObjectsOfType<EmotionalLight>();

        EmotionSettings settings = new EmotionSettings
        {

            alegria = alegriaSlider.value,
            confianza = confianzaSlider.value,
            miedo = miedoSlider.value,
            sorpresa = sorpresaSlider.value,
            tristeza = tristezaSlider.value,
            asco = ascoSlider.value,
            ira = iraSlider.value,
            anticipacion = anticipacionSlider.value,
            lightR = sceneLight.color.r,
            lightG = sceneLight.color.g,
            lightB = sceneLight.color.b,
            markerX = emotionMarker.anchoredPosition.x, // Guardar posición X
            markerY = emotionMarker.anchoredPosition.y  // Guardar posición Y

        };

        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(filePath, json); // Guardar el archivo en el disco
        Debug.Log("Datos guardados en: " + filePath);
    }

    private void LoadFromJson()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            EmotionSettings settings = JsonUtility.FromJson<EmotionSettings>(json);

            alegriaSlider.value = settings.alegria;
            confianzaSlider.value = settings.confianza;
            miedoSlider.value = settings.miedo;
            sorpresaSlider.value = settings.sorpresa;
            tristezaSlider.value = settings.tristeza;
            ascoSlider.value = settings.asco;
            iraSlider.value = settings.ira;
            anticipacionSlider.value = settings.anticipacion;
            EmotionalLight[] emotionalLights = FindObjectsOfType<EmotionalLight>();

            foreach (EmotionalLight eLight in emotionalLights)
            {
                eLight.SetEmotionColor(new Color(settings.lightR, settings.lightG, settings.lightB));
            }


            // Restaurar la posición del emotionMarker
            emotionMarker.anchoredPosition = new Vector2(settings.markerX, settings.markerY);


            Debug.Log("Datos cargados desde JSON.");
        }
        else
        {
            Debug.LogWarning("No se encontró el archivo JSON, se usarán valores predeterminados.");
        }
    }
    private float GetEmotionWeight(string emotion)
    {
        switch (emotion)
        {
            case "alegría": return alegriaSlider.value;
            case "confianza": return confianzaSlider.value;
            case "miedo": return miedoSlider.value;
            case "sorpresa": return sorpresaSlider.value;
            case "tristeza": return tristezaSlider.value;
            case "asco": return ascoSlider.value;
            case "ira": return iraSlider.value;
            case "anticipación": return anticipacionSlider.value;
            default: return 0f;
        }
    }
    public void ApplyEmotionalLighting()
    {
        Color finalColor = Color.black;
        float totalWeight = 0f;
        // Diccionario con colores de emociones (ejemplo)
        Dictionary<string, Color> emotionColors = new Dictionary<string, Color>
        {
            { "alegría", new Color(1f, 0.84f, 0.6f) },
            { "confianza", new Color(0.6f, 1f, 0.6f) },
            { "miedo", new Color(0.3f, 0.3f, 0.6f) },
            { "sorpresa", new Color(1f, 1f, 0.6f) },
            { "tristeza", new Color(0.2f, 0.4f, 0.8f) },
            { "asco", new Color(0.4f, 0.8f, 0.4f) },
            { "ira", new Color(1f, 0.2f, 0.2f) },
            { "anticipación", new Color(1f, 0.6f, 0.2f) }
        };

        EmotionalLight[] emotionalLights = FindObjectsOfType<EmotionalLight>();



        foreach (var emotion in emotionColors.Keys)
        {

            float weight = GetEmotionWeight(emotion);
            if (weight > 0)
            {
                finalColor += emotionColors[emotion] * weight;
                totalWeight += weight;
            }
        }

        if (totalWeight > 0)
        {
            finalColor /= totalWeight;
        }

        foreach (EmotionalLight eLight in emotionalLights)
        {
            eLight.SetEmotionColor(finalColor);
        }
        UpdateEmotionMarker(finalColor);
        SaveToJson(); // Guardar el estado después de aplicar cambios
    }

    private void CheckEmotionSelection(Vector2 clickPosition)
    {
        foreach (var emotion in emotionRanges)
        {
            if (IsPointInRange(clickPosition, emotion.Value))
            {
                SetEmotion(emotion.Key);
                return;
            }
        }
    }

    private bool IsPointInRange(Vector2 point, Vector2[] range)
    {
        for (int i = 0; i < range.Length; i++)
        {
            if (Vector2.Distance(point, range[i]) < 5f)
                return true;
        }
        return false;
    }

    // Modificar SetEmotion para ajustar los sliders de las emociones secundarias
    private void SetEmotion(string emotion)
    {
        // Restablecer todos los sliders
        ResetSliders();

        if (emotionColors.ContainsKey(emotion))
        {
            sceneLight.color = emotionColors[emotion];
            PositionMarker(emotion);
            SetSliderValue(emotion, 1.0f);
        }
        else if (secondaryEmotions.ContainsKey(emotion))
        {
            // Ajustar sliders de las emociones primarias que forman la secundaria
            foreach (var primaryEmotion in secondaryEmotions[emotion])
            {
                SetSliderValue(primaryEmotion, 0.5f); // Ajustar a 50% para cada primaria
            }

            // Mezclar los colores de las emociones primarias
            ApplyEmotionalLighting();
            PositionMarker(emotion);
        }
    }
    private void SetSliderValue(string emotion, float value)
    {
        switch (emotion)
        {
            case "alegría": alegriaSlider.value = value; break;
            case "confianza": confianzaSlider.value = value; break;
            case "miedo": miedoSlider.value = value; break;
            case "sorpresa": sorpresaSlider.value = value; break;
            case "tristeza": tristezaSlider.value = value; break;
            case "asco": ascoSlider.value = value; break;
            case "ira": iraSlider.value = value; break;
            case "anticipación": anticipacionSlider.value = value; break;
        }
    }

    private void ResetSliders()
    {
        alegriaSlider.value = 0f;
        confianzaSlider.value = 0f;
        miedoSlider.value = 0f;
        sorpresaSlider.value = 0f;
        tristezaSlider.value = 0f;
        ascoSlider.value = 0f;
        iraSlider.value = 0f;
        anticipacionSlider.value = 0f;
    }

    private void UpdateEmotionMarker(Color generatedColor)
    {
        string closestEmotion = "";
        float closestDistance = float.MaxValue;

        foreach (var emotion in emotionColors)
        {
            float distance = Vector3.Distance(
                new Vector3(emotion.Value.r, emotion.Value.g, emotion.Value.b),
                new Vector3(generatedColor.r, generatedColor.g, generatedColor.b)
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEmotion = emotion.Key;
            }
        }

        PositionMarker(closestEmotion);
    }

    private void PositionMarker(string emotion)
    {
        if (emotionRanges.ContainsKey(emotion))
        {
            // Si la emoción secundaria tiene coordenadas predefinidas, usa la primera posición
            emotionMarker.anchoredPosition = emotionRanges[emotion][0];
        }
        else if (secondaryEmotions.ContainsKey(emotion))
        {
            // Si es una emoción secundaria, calcular la posición promedio de sus emociones primarias
            Vector2 averagePosition = Vector2.zero;
            int count = 0;

            foreach (var primaryEmotion in secondaryEmotions[emotion])
            {
                if (emotionRanges.ContainsKey(primaryEmotion))
                {
                    averagePosition += emotionRanges[primaryEmotion][0]; // Toma la primera coordenada de la emoción primaria
                    count++;
                }
            }

            if (count > 0)
            {
                averagePosition /= count; // Calcula el promedio
                emotionMarker.anchoredPosition = averagePosition;
            }
        }
    }

    [System.Serializable]
    public class EmotionSettings
    {
        public float alegria, confianza, miedo, sorpresa;
        public float tristeza, asco, ira, anticipacion;
        public float lightR, lightG, lightB;
        public float markerX, markerY; // Posición del marcador

    }
    [System.Serializable]
    public class EmotionProfile
    {
        public string name;
        public float alegria, confianza, miedo, sorpresa, tristeza, asco, ira, anticipacion;
        public float lightR, lightG, lightB;
        public float markerX, markerY;
    }

    [System.Serializable]
    public class EmotionProfileList
    {
        public List<EmotionProfile> profiles = new List<EmotionProfile>();
    }
    // UI para guardar estado
    public InputField profileNameInput;
    public Button saveProfileButton;
    // Rutas de archivos

    private string profilesPath;
    private void SaveProfile()
    {
        string name = profileNameInput.text.Trim();
        if (string.IsNullOrEmpty(name)) return;

        EmotionProfile profile = new EmotionProfile
        {
            name = name,
            alegria = alegriaSlider.value,
            confianza = confianzaSlider.value,
            miedo = miedoSlider.value,
            sorpresa = sorpresaSlider.value,
            tristeza = tristezaSlider.value,
            asco = ascoSlider.value,
            ira = iraSlider.value,
            anticipacion = anticipacionSlider.value,
            lightR = sceneLight.color.r,
            lightG = sceneLight.color.g,
            lightB = sceneLight.color.b,
            markerX = emotionMarker.anchoredPosition.x,
            markerY = emotionMarker.anchoredPosition.y
        };

        EmotionProfileList profileList = LoadAllProfiles();
        profileList.profiles.Add(profile);
        File.WriteAllText(profilesPath, JsonUtility.ToJson(profileList, true));

        Debug.Log($"Perfil '{name}' guardado.");
    }
    private EmotionProfileList LoadAllProfiles()
    {
        if (!File.Exists(profilesPath))
            return new EmotionProfileList();

        string json = File.ReadAllText(profilesPath);
        return JsonUtility.FromJson<EmotionProfileList>(json);
    }
    public void ActivateProfile(string profileName)
    {
        EmotionProfileList profileList = LoadAllProfiles();
        EmotionProfile profile = profileList.profiles.Find(p => p.name == profileName);

        if (profile == null)
        {
            Debug.LogWarning($"Perfil '{profileName}' no encontrado.");
            return;
        }

        // Aplicar valores
        alegriaSlider.value = profile.alegria;
        confianzaSlider.value = profile.confianza;
        miedoSlider.value = profile.miedo;
        sorpresaSlider.value = profile.sorpresa;
        tristezaSlider.value = profile.tristeza;
        ascoSlider.value = profile.asco;
        iraSlider.value = profile.ira;
        anticipacionSlider.value = profile.anticipacion;

        emotionMarker.anchoredPosition = new Vector2(profile.markerX, profile.markerY);
        sceneLight.color = new Color(profile.lightR, profile.lightG, profile.lightB);

        ApplyEmotionalLighting(); // Aplica visualmente (si tienes esta función ya creada)
        Debug.Log($"Perfil '{profileName}' activado.");
    }
}
