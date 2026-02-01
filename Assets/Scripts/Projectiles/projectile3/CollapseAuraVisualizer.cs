using UnityEngine;

[RequireComponent(typeof(CollapseProjectileStats))]
public class CollapseAuraVisualizer : MonoBehaviour
{
    [Header("Aura Visual")]
    [SerializeField] private Transform auraSphere;
    [SerializeField] private float growSpeed = 6f;

    [Header("Aura Particles")]
    [SerializeField] private ParticleSystem auraParticles;

    private CollapseProjectileStats stats;
    private ParticleSystem.MainModule particlesMain;

    // ----------------- Initialize references and cache particle main module ------------------
    private void Awake()
    {
        stats = GetComponent<CollapseProjectileStats>();

        if (auraParticles != null)
            particlesMain = auraParticles.main;

        Debug.Log(stats.collapseRadius + " collapse sphere radius");
    }

    // ----------------- Set initial aura and particle states when enabled ------------------
    private void OnEnable()
    {
        float diameter = stats.collapseRadius * 2f;

        // Set aura scale to zero initially
        if (auraSphere != null)
            auraSphere.localScale = Vector3.zero;

        // Set particle size and play particles
        if (auraParticles != null)
        {
            particlesMain.startSize = diameter;
            auraParticles.Play();
        }
    }

    // ----------------- Update the aura scale smoothly each frame ------------------
    private void Update()
    {
        if (auraSphere == null || stats == null) return;

        float targetScale = stats.collapseRadius * 2f;
        Vector3 target = Vector3.one * targetScale;

        auraSphere.localScale = Vector3.Lerp(
            auraSphere.localScale,
            target,
            Time.deltaTime * growSpeed
        );
    }
}
