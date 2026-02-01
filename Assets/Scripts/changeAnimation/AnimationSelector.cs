using UnityEngine;

public class AnimationSelector : MonoBehaviour
{
    public int CurrentIndex => currentIndex;
    private int currentIndex = 0;

    private Animator animator;

    private void Awake()
    {
        // ----------------- Initialize animator reference and sync current animation index ------------------
        currentIndex = AnimationState.SelectedAnimationIndex;
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimation(int index)
    {
        // ----------------- Change the preview animation based on the given index ------------------
        currentIndex = index;
        animator.SetFloat("AnimDancingIndex", currentIndex);
        Debug.Log($"[Selector] Preview index: {currentIndex}");
    }

    public void SelectAnimation()
    {
        // ----------------- Confirm and store the selected animation index globally ------------------
        AnimationState.SelectedAnimationIndex = currentIndex;
        Debug.Log($"[Selector] Selected index: {currentIndex}");
    }
}
