using UnityEditor; // Necesario para TextureImporter, AssetDatabase, EditorUtility, MenuItem
using UnityEngine; // Necesario para Debug.Log, Mathf

public class TextureImporterChanger : EditorWindow
{
    // --- Configuración del Script (puedes ajustar estos valores en el Editor) ---
    [SerializeField] // Permite serializar y mostrar en el Inspector de la ventana personalizada
    private string targetFolderPath = "Assets/Main Room"; // Ruta de la carpeta a escanear
    [SerializeField]
    private int newMaxSize = 1024; // El nuevo Max Size deseado
    [SerializeField]
    private TextureImporterFormat newFormat = TextureImporterFormat.RGBCompressedDXT1; // El formato de compresión deseado (DXT1 en tu caso)
    [SerializeField]
    private TextureImporterCompression newCompressionQuality = TextureImporterCompression.Compressed; // Calidad de compresión

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
        // Crea una instancia del script para poder acceder a sus campos (si se llama directamente desde el menú)
        TextureImporterChanger window = (TextureImporterChanger)GetWindow(typeof(TextureImporterChanger));
        window.SetMaxSizeForTexturesInFolder(window.targetFolderPath, window.newMaxSize, window.newFormat, window.newCompressionQuality);
    }

    // --- Lógica principal para cambiar la configuración ---
    private void SetMaxSizeForTexturesInFolder(string folderPath, int maxSize, TextureImporterFormat format, TextureImporterCompression compressionQuality)
    {
        // Valida la ruta de la carpeta
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogError($"TextureImporterChanger: La carpeta '{folderPath}' no es válida o no existe. Asegúrate de que la ruta sea correcta (ej. 'Assets/MiCarpeta').");
            return;
        }

        Debug.Log($"TextureImporterChanger: Iniciando ajuste de Max Size de texturas en '{folderPath}' a {maxSize} para WebGL (Formato: {format})...");

        // Inicia un bloque de edición de assets para un mejor rendimiento
        // Esto evita que Unity reimporte cada textura individualmente
        AssetDatabase.StartAssetEditing();

        // Busca todos los assets de tipo Texture dentro de la carpeta especificada
        string[] guids = AssetDatabase.FindAssets("t:Texture", new string[] { folderPath });
        int processedCount = 0;
        int updatedCount = 0;

        try
        {
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

                // Solo procesa si realmente es un TextureImporter
                if (textureImporter != null)
                {
                    EditorUtility.DisplayProgressBar("Ajustando Texturas", $"Procesando: {assetPath}", (float)processedCount / guids.Length);

                    // Obtiene la configuración de plataforma para WebGL
                    TextureImporterPlatformSettings platformSettings = textureImporter.GetPlatformTextureSettings("WebGL");

                    // Comprueba si la configuración actual ya es la deseada para evitar reimportaciones innecesarias
                    if (!platformSettings.overridden ||
                        platformSettings.maxTextureSize != maxSize ||
                        platformSettings.format != format ||
                        platformSettings.compressionQuality != compressionQuality)
                    {
                        // Aplica la anulación de la configuración para WebGL
                        platformSettings.overridden = true; // Asegura que la anulación está activa
                        platformSettings.maxTextureSize = maxSize;
                        platformSettings.format = format;
                        platformSettings.compressionQuality = compressionQuality; // Establece la calidad de compresión

                        // Aplica las nuevas configuraciones y fuerza la reimportación
                        textureImporter.SetPlatformTextureSettings(platformSettings);
                        textureImporter.SaveAndReimport();
                        Debug.Log($"TextureImporterChanger: Actualizada '{assetPath}' (WebGL Max Size: {maxSize}, Formato: {format}).");
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
            // Siempre asegúrate de detener el bloque de edición de assets
            AssetDatabase.StopAssetEditing();
            // Guarda los cambios en los assets
            AssetDatabase.SaveAssets();
            // Limpia la barra de progreso
            EditorUtility.ClearProgressBar();
            Debug.Log($"TextureImporterChanger: Finalizado. Total de texturas procesadas: {processedCount}, Actualizadas: {updatedCount}.");
        }
    }

    // --- GUI de la ventana del editor ---
    void OnGUI()
    {
        GUILayout.Label("Ajustes de Texturas para WebGL", EditorStyles.boldLabel);

        // Campo para la ruta de la carpeta
        targetFolderPath = EditorGUILayout.TextField("Ruta de la Carpeta:", targetFolderPath);

        // Campo para el nuevo Max Size (asegurándose de que sea una potencia de 2)
        newMaxSize = EditorGUILayout.IntField("Nuevo Max Size:", newMaxSize);
        newMaxSize = Mathf.ClosestPowerOfTwo(newMaxSize); // Asegura que el valor sea una potencia de 2
        EditorGUILayout.HelpBox($"El tamaño máximo se establecerá a la potencia de dos más cercana (ej. 512, 1024, 2048). El valor actual será {newMaxSize}.", MessageType.Info);

        // Desplegable para el formato de compresión
        newFormat = (TextureImporterFormat)EditorGUILayout.EnumPopup("Formato de Compresión:", newFormat);

        // Desplegable para la calidad de compresión
        newCompressionQuality = (TextureImporterCompression)EditorGUILayout.EnumPopup("Calidad de Compresión:", newCompressionQuality);

        EditorGUILayout.Space();

        if (GUILayout.Button("Aplicar Ajustes a Texturas en la Carpeta"))
        {
            SetMaxSizeForTexturesInFolder(targetFolderPath, newMaxSize, newFormat, newCompressionQuality);
        }
    }
}