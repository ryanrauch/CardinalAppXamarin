using System;
using UIKit;

namespace FlyoverKit.iOS
{
    public static class UIViewPropertyAnimatorExtensions
    {
        public static void ForceStopAnimation(this UIViewPropertyAnimator animator)
        {
            // Stop animation without finishing.
            animator.StopAnimation(true);
        }

        /// <summary>
        /// Convenience function to set the completion with a parameter closure
        /// </summary>
        /// <param name="animator"></param>
        public static void SetCompletion(this UIViewPropertyAnimator animator, Action<UIViewAnimatingPosition> completion)
        {
            animator.AddCompletion(completion);
        }

        public static void SetCompletion(this UIViewPropertyAnimator animator)
        {
            animator.AddCompletion((a) => { });
        }
    }
}
