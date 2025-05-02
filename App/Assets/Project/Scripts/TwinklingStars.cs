using UnityEngine;

public class TwinklingStars : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    [Header("Configuración")]
    [SerializeField] private float twinkleSpeed = 3f;
    [SerializeField] private int maxStars = 200;
    [SerializeField] private float minAlpha = 0.3f;
    [SerializeField] private float maxAlpha = 1f;
    [SerializeField] private float starSize = 0.1f;

    void Start()
    {
        // Configurar cámara
        Camera.main.backgroundColor = new Color(0f, 0f, 0.1f, 1f);

        // Inicializar array de partículas
        particles = new ParticleSystem.Particle[maxStars];

        // Configurar sistema de partículas
        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startLifetime = 1000f;
        main.startSpeed = 0f;
        main.startSize = starSize;
        main.maxParticles = maxStars;

        // Configurar emisión
        var emission = ps.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, (short)maxStars) });

        // Configurar área de aparición
        UpdateShapeSize();
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Box;

        // Generar partículas iniciales
        ps.Emit(maxStars);
        ps.GetParticles(particles);
    }

    void Update()
    {
        UpdateParticles();
        UpdateShapeSize();
    }

    void UpdateParticles()
    {
        int numParticles = ps.GetParticles(particles);

        for (int i = 0; i < numParticles; i++)
        {
            float phase = ((float)particles[i].randomSeed / uint.MaxValue) * Mathf.PI * 2f;
            float alpha = Mathf.Sin(Time.time * twinkleSpeed + phase) * 0.5f + 0.5f;
            alpha = Mathf.Lerp(minAlpha, maxAlpha, alpha);

            Color32 color = particles[i].startColor;
            color.a = (byte)(alpha * 255);
            particles[i].startColor = color;
        }

        ps.SetParticles(particles, numParticles);
    }

    void UpdateShapeSize()
    {
        Camera cam = Camera.main;
        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;

        var shape = ps.shape;
        shape.scale = new Vector3(width, height, 0f);
    }
}