using UnityEditor; // Necesario para TextureImporter, AssetDatabase, EditorUtility, MenuItem
using UnityEngine; // Necesario para Debug.Log, Mathf

public class TextureImporterChanger : EditorWindow
{
    // --- Configuración del Script (puedes ajustar estos valores en el Editor) ---
    [SerializeField]
    private string targetFolderPath = "Assets/Main Room"; // Ruta de la carpeta a escanear
    [SerializeField]
    private int newMaxSize = 1024; // El nuevo Max Size deseado
    [SerializeField]
    // FIX 1: Cambiado de RGBCompressedDXT1 a DXT1, que es el miembro correcto de la enumeración.
    private TextureImporterFormat newFormat = TextureImporterFormat.DXT1;
    [SerializeField]
    // Mantenemos la enumeración aquí para la UI del editor y el valor por defecto.
    private TextureImporterCompression newCompressionQuality = TextureImporterCompression.Compressed;

    // --- Opción de menú para abrir la ventana del editor ---
    [MenuItem("Tools/Texture Importer/Open WebGL Settings Window")]
    public static void ShowWindow()
    {
        GetWindow<TextureImporterChanger>("WebGL Texture Settings");
    }

    // --- Método para aplicar la configuración, accesible desde la ventana o directamente ---
    [MenuItem("Tools/Texture Importer/Apply WebGL Max Size 1024 to 'Main Room' (DXT1)")]
    public static void ApplySettingsToMainRoom()
    {
        TextureImporterChanger window = (TextureImporterChanger)GetWindow(typeof(TextureImporterChanger));
        window.SetMaxSizeForTexturesInFolder(window.targetFolderPath, window.newMaxSize, window.newFormat, window.newCompressionQuality);
    }

    // --- Lógica principal para cambiar la configuración ---
    private void SetMaxSizeForTexturesInFolder(string folderPath, int maxSize, TextureImporterFormat format, TextureImporterCompression compressionQuality)
    {
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogError($"TextureImporterChanger: La carpeta '{folderPath}' no es válida o no existe. Asegúrate de que la ruta sea correcta (ej. 'Assets/MiCarpeta').");
            return;
        }

        Debug.Log($"TextureImporterChanger: Iniciando ajuste de Max Size de texturas en '{folderPath}' a {maxSize} para WebGL (Formato: {format})...");

        AssetDatabase.StartAssetEditing();
        string[] guids = AssetDatabase.FindAssets("t:Texture", new string[] { folderPath });
        int processedCount = 0;
        int updatedCount = 0;

        try
        {
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

                if (textureImporter != null)
                {
                    EditorUtility.DisplayProgressBar("Ajustando Texturas", $"Procesando: {assetPath}", (float)processedCount / guids.Length);

                    TextureImporterPlatformSettings platformSettings = textureImporter.GetPlatformTextureSettings("WebGL");

                    // Valor actual de la calidad de compresión de la plataforma WebGL
                    // Intentamos obtenerlo como int para compatibilidad con versiones antiguas de Unity
                    int currentCompressionQualityInt;
                    try
                    {
                        // En versiones más nuevas de Unity, compressionQuality es de tipo TextureImporterCompression
                        currentCompressionQualityInt = (int)platformSettings.compressionQuality;
                    }
                    catch (System.InvalidCastException)
                    {
                        // En versiones más antiguas, compressionQuality puede ser directamente int
                        // Si falla el cast, asumimos que ya es int o manejamos un valor predeterminado
                        // Esto es un 'catch-all' de seguridad, idealmente platformSettings.compressionQuality debería ser TextureImporterCompression
                        currentCompressionQualityInt = -1; // Valor no válido para forzar la actualización
                        Debug.LogWarning($"TextureImporterChanger: La propiedad 'compressionQuality' para WebGL en '{assetPath}' no se pudo convertir a int, lo que puede indicar una versión antigua de Unity o una API diferente. Forzando actualización.");
                    }
                    
                    // Comprueba si la configuración actual ya es la deseada para evitar reimportaciones innecesarias
                    // FIX 2 y 3: Se añade un cast explícito (int) a newCompressionQuality para la comparación y asignación.
                    // Esto resuelve el error CS0019 y CS0266 si platformSettings.compressionQuality espera un int.
                    if (!platformSettings.overridden ||
                        platformSettings.maxTextureSize != maxSize ||
                        platformSettings.format != format ||
                        currentCompressionQualityInt != (int)compressionQuality) // Comparación con el valor int del enum
                    {
                        // Aplica la anulación de la configuración para WebGL
                        platformSettings.overridden = true; // Asegura que la anulación está activa
                        platformSettings.maxTextureSize = maxSize;
                        platformSettings.format = format;
                        platformSettings.compressionQuality = (int)compressionQuality; // Asignación con el valor int del enum

                        // Aplica las nuevas configuraciones y fuerza la reimportación
                        textureImporter.SetPlatformTextureSettings(platformSettings);
                        textureImporter.SaveAndReimport();
                        Debug.Log($"TextureImporterChanger: Actualizada '{assetPath}' (WebGL Max Size: {maxSize}, Formato: {format}, Compresión: {compressionQuality}).");
                        updatedCount++;
                    }
                    else
                    {
                        Debug.Log($"TextureImporterChanger: Omitida '{assetPath}' (ya cumple con la configuración WebGL deseada).");
                    }
                }
                processedCount++;
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();
            Debug.Log($"TextureImporterChanger: Finalizado. Total de texturas procesadas: {processedCount}, Actualizadas: {updatedCount}.");
        }
    }

    // --- GUI de la ventana del editor ---
    void OnGUI()
    {
        GUILayout.Label("Ajustes de Texturas para WebGL", EditorStyles.boldLabel);

        targetFolderPath = EditorGUILayout.TextField("Ruta de la Carpeta:", targetFolderPath);

        newMaxSize = EditorGUILayout.IntField("Nuevo Max Size:", newMaxSize);
        newMaxSize = Mathf.ClosestPowerOfTwo(newMaxSize); // Asegura que el valor sea una potencia de 2
        EditorGUILayout.HelpBox($"El tamaño máximo se establecerá a la potencia de dos más cercana (ej. 512, 1024, 2048). El valor actual será {newMaxSize}.", MessageType.Info);

        newFormat = (TextureImporterFormat)EditorGUILayout.EnumPopup("Formato de Compresión:", newFormat);
        newCompressionQuality = (TextureImporterCompression)EditorGUILayout.EnumPopup("Calidad de Compresión:", newCompressionQuality);

        EditorGUILayout.Space();

        if (GUILayout.Button("Aplicar Ajustes a Texturas en la Carpeta"))
        {
            SetMaxSizeForTexturesInFolder(targetFolderPath, newMaxSize, newFormat, newCompressionQuality);
        }
    }
}