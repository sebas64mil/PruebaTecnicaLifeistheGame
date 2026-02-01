using System;

public static class AnimationState
{
    private static int selectedAnimationIndex = 0;

    // ----------------- Event triggered when the selected animation changes ------------------
    public static event Action<int> OnAnimationChanged;

    public static int SelectedAnimationIndex
    {
        // ----------------- Get the currently selected animation index ------------------
        get => selectedAnimationIndex;

        // ----------------- Set a new animation index and notify listeners if it changes ------------------
        set
        {
            if (selectedAnimationIndex == value) return;

            selectedAnimationIndex = value;
            OnAnimationChanged?.Invoke(selectedAnimationIndex);
        }
    }
}
