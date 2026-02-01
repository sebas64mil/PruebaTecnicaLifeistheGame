using UnityEngine;

public class AnimationBlendController : MonoBehaviour
{
    private Animator animator;
    private string parameterName = "AnimDancingIndex";

    private void Awake()
    {
        // ----------------- Initialize animator reference and apply the selected animation index ------------------
        animator = GetComponent<Animator>();
        animator.SetFloat(parameterName, AnimationState.SelectedAnimationIndex);
    }

    private void OnEnable()
    {
        // ----------------- Subscribe to animation change events ------------------
        AnimationState.OnAnimationChanged += OnAnimationChanged;
    }

    private void OnDisable()
    {
        // ----------------- Unsubscribe from animation change events ------------------
        AnimationState.OnAnimationChanged -= OnAnimationChanged;
    }

    private void OnAnimationChanged(int index)
    {
        // ----------------- Update animator parameter when the animation index changes ------------------
        animator.SetFloat(parameterName, index);
        Debug.Log($"[BlendController] Animator updated to: {index}");
    }
}
