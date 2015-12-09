using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Tarot.Forms.MaterialSkin.Animations
{
    #region Using Directives

    

    #endregion

    internal class AnimationManager
    {
        private const double MaxValue = 1.00;

        private const double MinValue = 0.00;

        private readonly List<object[]> animationDatas;

        private readonly List<AnimationDirection> animationDirections;

        private readonly List<double> animationProgresses;

        private readonly List<Point> animationSources;

        private readonly Timer animationTimer = new Timer {Interval = 5, Enabled = false};

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="singular">
        ///     If true, only one animation is supported. The current animation will
        ///     be replaced with the new one. If false, a new anmation is added to
        ///     the list.
        /// </param>
        public AnimationManager(bool singular = true)
        {
            animationProgresses = new List<double>();
            animationSources = new List<Point>();
            animationDirections = new List<AnimationDirection>();
            animationDatas = new List<object[]>();

            Increment = 0.03;
            SecondaryIncrement = 0.03;
            AnimationType = AnimationType.Linear;
            InterruptAnimation = true;
            Singular = singular;

            if (Singular)
            {
                animationProgresses.Add(0);
                animationSources.Add(new Point(0, 0));
                animationDirections.Add(AnimationDirection.In);
            }

            animationTimer.Tick += AnimationTimerOnTick;
        }

        public delegate void AnimationFinished(object sender);

        public delegate void AnimationProgress(object sender);

        public event AnimationFinished OnAnimationFinished;

        public event AnimationProgress OnAnimationProgress;

        public AnimationType AnimationType { get; set; }

        public double Increment { get; set; }

        public bool InterruptAnimation { get; set; }

        public double SecondaryIncrement { get; set; }

        public bool Singular { get; set; }

        public int GetAnimationCount()
        {
            return animationProgresses.Count;
        }

        public object[] GetData()
        {
            if (!Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }

            if (animationDatas.Count == 0)
            {
                throw new Exception("Invalid animation");
            }

            return animationDatas[0];
        }

        public object[] GetData(int index)
        {
            if (!(index < animationDatas.Count))
            {
                throw new IndexOutOfRangeException("Invalid animation index");
            }

            return animationDatas[index];
        }

        public AnimationDirection GetDirection()
        {
            if (!Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }

            if (animationDirections.Count == 0)
            {
                throw new Exception("Invalid animation");
            }

            return animationDirections[0];
        }

        public AnimationDirection GetDirection(int index)
        {
            if (!(index < animationDirections.Count))
            {
                throw new IndexOutOfRangeException("Invalid animation index");
            }

            return animationDirections[index];
        }

        public double GetProgress()
        {
            if (!Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }

            if (animationProgresses.Count == 0)
            {
                throw new Exception("Invalid animation");
            }

            return GetProgress(0);
        }

        public double GetProgress(int index)
        {
            if (!(index < GetAnimationCount()))
            {
                throw new IndexOutOfRangeException("Invalid animation index");
            }

            switch (AnimationType)
            {
                case AnimationType.Linear:
                    return AnimationLinear.CalculateProgress(animationProgresses[index]);
                case AnimationType.EaseInOut:
                    return AnimationEaseInOut.CalculateProgress(animationProgresses[index]);
                case AnimationType.EaseOut:
                    return AnimationEaseOut.CalculateProgress(animationProgresses[index]);
                case AnimationType.CustomQuadratic:
                    return AnimationCustomQuadratic.CalculateProgress(animationProgresses[index]);
                default:
                    throw new NotImplementedException("The given AnimationType is not implemented");
            }
        }

        public Point GetSource(int index)
        {
            if (!(index < GetAnimationCount()))
            {
                throw new IndexOutOfRangeException("Invalid animation index");
            }

            return animationSources[index];
        }

        public Point GetSource()
        {
            if (!Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }

            if (animationSources.Count == 0)
            {
                throw new Exception("Invalid animation");
            }

            return animationSources[0];
        }

        public bool IsAnimating()
        {
            return animationTimer.Enabled;
        }

        public void SetData(object[] data)
        {
            if (!Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }

            if (animationDatas.Count == 0)
            {
                throw new Exception("Invalid animation");
            }

            animationDatas[0] = data;
        }

        public void SetDirection(AnimationDirection direction)
        {
            if (!Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }

            if (animationProgresses.Count == 0)
            {
                throw new Exception("Invalid animation");
            }

            animationDirections[0] = direction;
        }

        public void SetProgress(double progress)
        {
            if (!Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }

            if (animationProgresses.Count == 0)
            {
                throw new Exception("Invalid animation");
            }

            animationProgresses[0] = progress;
        }

        public void StartNewAnimation(AnimationDirection animationDirection, object[] data = null)
        {
            StartNewAnimation(animationDirection, new Point(0, 0), data);
        }

        public void StartNewAnimation(
            AnimationDirection animationDirection,
            Point animationSource,
            object[] data = null)
        {
            if (!IsAnimating() || InterruptAnimation)
            {
                if (Singular && animationDirections.Count > 0)
                {
                    animationDirections[0] = animationDirection;
                }
                else
                {
                    animationDirections.Add(animationDirection);
                }

                if (Singular && animationSources.Count > 0)
                {
                    animationSources[0] = animationSource;
                }
                else
                {
                    animationSources.Add(animationSource);
                }

                if (!(Singular && animationProgresses.Count > 0))
                {
                    switch (animationDirections[animationDirections.Count - 1])
                    {
                        case AnimationDirection.InOutRepeatingIn:
                        case AnimationDirection.InOutIn:
                        case AnimationDirection.In:
                            animationProgresses.Add(MinValue);
                            break;
                        case AnimationDirection.InOutRepeatingOut:
                        case AnimationDirection.InOutOut:
                        case AnimationDirection.Out:
                            animationProgresses.Add(MaxValue);
                            break;
                        default:
                            throw new Exception("Invalid AnimationDirection");
                    }
                }

                if (Singular && animationDatas.Count > 0)
                {
                    animationDatas[0] = data ?? new object[] {};
                }
                else
                {
                    animationDatas.Add(data ?? new object[] {});
                }
            }

            animationTimer.Start();
        }

        public void UpdateProgress(int index)
        {
            switch (animationDirections[index])
            {
                case AnimationDirection.InOutRepeatingIn:
                case AnimationDirection.InOutIn:
                case AnimationDirection.In:
                    IncrementProgress(index);
                    break;
                case AnimationDirection.InOutRepeatingOut:
                case AnimationDirection.InOutOut:
                case AnimationDirection.Out:
                    DecrementProgress(index);
                    break;
                default:
                    throw new Exception("No AnimationDirection has been set");
            }
        }

        private void AnimationTimerOnTick(object sender, EventArgs eventArgs)
        {
            for (var i = 0; i < animationProgresses.Count; i++)
            {
                UpdateProgress(i);

                if (!Singular)
                {
                    if (animationDirections[i] == AnimationDirection.InOutIn
                        && animationProgresses[i] == MaxValue)
                    {
                        animationDirections[i] = AnimationDirection.InOutOut;
                    }
                    else if (animationDirections[i] == AnimationDirection.InOutRepeatingIn
                             && animationProgresses[i] == MinValue)
                    {
                        animationDirections[i] = AnimationDirection.InOutRepeatingOut;
                    }
                    else if (animationDirections[i] == AnimationDirection.InOutRepeatingOut
                             && animationProgresses[i] == MinValue)
                    {
                        animationDirections[i] = AnimationDirection.InOutRepeatingIn;
                    }
                    else if ((animationDirections[i] == AnimationDirection.In
                              && animationProgresses[i] == MaxValue)
                             || (animationDirections[i] == AnimationDirection.Out
                                 && animationProgresses[i] == MinValue)
                             || (animationDirections[i] == AnimationDirection.InOutOut
                                 && animationProgresses[i] == MinValue))
                    {
                        animationProgresses.RemoveAt(i);
                        animationSources.RemoveAt(i);
                        animationDirections.RemoveAt(i);
                        animationDatas.RemoveAt(i);
                    }
                }
                else
                {
                    if (animationDirections[i] == AnimationDirection.InOutIn
                        && animationProgresses[i] == MaxValue)
                    {
                        animationDirections[i] = AnimationDirection.InOutOut;
                    }
                    else if (animationDirections[i] == AnimationDirection.InOutRepeatingIn
                             && animationProgresses[i] == MaxValue)
                    {
                        animationDirections[i] = AnimationDirection.InOutRepeatingOut;
                    }
                    else if (animationDirections[i] == AnimationDirection.InOutRepeatingOut
                             && animationProgresses[i] == MinValue)
                    {
                        animationDirections[i] = AnimationDirection.InOutRepeatingIn;
                    }
                }
            }

            if (OnAnimationProgress != null)
            {
                OnAnimationProgress(this);
            }
        }

        private void DecrementProgress(int index)
        {
            animationProgresses[index] -= animationDirections[index] == AnimationDirection.InOutOut
                                          || animationDirections[index]
                                          == AnimationDirection.InOutRepeatingOut
                ? SecondaryIncrement
                : Increment;
            if (animationProgresses[index] < MinValue)
            {
                animationProgresses[index] = MinValue;

                for (var i = 0; i < GetAnimationCount(); i++)
                {
                    if (animationDirections[i] == AnimationDirection.InOutIn)
                    {
                        return;
                    }
                    if (animationDirections[i] == AnimationDirection.InOutRepeatingIn)
                    {
                        return;
                    }
                    if (animationDirections[i] == AnimationDirection.InOutRepeatingOut)
                    {
                        return;
                    }
                    if (animationDirections[i] == AnimationDirection.InOutOut
                        && animationProgresses[i] != MinValue)
                    {
                        return;
                    }
                    if (animationDirections[i] == AnimationDirection.Out && animationProgresses[i] != MinValue)
                    {
                        return;
                    }
                }

                animationTimer.Stop();
                if (OnAnimationFinished != null)
                {
                    OnAnimationFinished(this);
                }
            }
        }

        private void IncrementProgress(int index)
        {
            animationProgresses[index] += Increment;
            if (animationProgresses[index] > MaxValue)
            {
                animationProgresses[index] = MaxValue;

                for (var i = 0; i < GetAnimationCount(); i++)
                {
                    if (animationDirections[i] == AnimationDirection.InOutIn)
                    {
                        return;
                    }
                    if (animationDirections[i] == AnimationDirection.InOutRepeatingIn)
                    {
                        return;
                    }
                    if (animationDirections[i] == AnimationDirection.InOutRepeatingOut)
                    {
                        return;
                    }
                    if (animationDirections[i] == AnimationDirection.InOutOut
                        && animationProgresses[i] != MaxValue)
                    {
                        return;
                    }
                    if (animationDirections[i] == AnimationDirection.In && animationProgresses[i] != MaxValue)
                    {
                        return;
                    }
                }

                animationTimer.Stop();
                if (OnAnimationFinished != null)
                {
                    OnAnimationFinished(this);
                }
            }
        }
    }
}