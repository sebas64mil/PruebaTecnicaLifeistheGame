using UnityEngine;

[RequireComponent(typeof(OrbitProjectileStats))]
public class OrbitAuraVisualizer : MonoBehaviour
{
    [Header("Aura Visual")]
    [SerializeField] private Transform auraSphere;
    [SerializeField] private float growSpeed = 6f;

    [Header("Aura Particles")]
    [SerializeField] private ParticleSystem auraParticles;

    private OrbitProjectileStats stats;
    private ParticleSystem.MainModule particlesMain;

    // ------------------ Initializes the stats reference and particle module ------------------
    private void Awake()
    {
        stats = GetComponent<OrbitProjectileStats>();

        if (auraParticles != null)
            particlesMain = auraParticles.main;

        Debug.Log(stats.orbitRadius + " radio de la esfera");
    }

    // ------------------ Sets initial scale and particle size when enabled ------------------
    private void OnEnable()
    {
        float diameter = stats.orbitRadius * 2f;

        if (auraSphere != null)
            auraSphere.localScale = Vector3.zero;

        if (auraParticles != null)
        {
            particlesMain.startSize = diameter;
            auraParticles.Play();
        }
    }

    // ------------------ Smoothly scales the aura sphere to match the orbit radius ------------------
    private void Update()
    {
        if (auraSphere == null || stats == null) return;

        float targetScale = stats.orbitRadius * 2f;
        Vector3 target = Vector3.one * targetScale;

        auraSphere.localScale = Vector3.Lerp(
            auraSphere.localScale,
            target,
            Time.deltaTime * growSpeed
        );
    }
}
