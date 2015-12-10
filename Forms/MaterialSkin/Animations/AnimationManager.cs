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
            this.animationProgresses = new List<double>();
            this.animationSources = new List<Point>();
            this.animationDirections = new List<AnimationDirection>();
            this.animationDatas = new List<object[]>();

            this.Increment = 0.03;
            this.SecondaryIncrement = 0.03;
            this.AnimationType = AnimationType.Linear;
            this.InterruptAnimation = true;
            this.Singular = singular;

            if (this.Singular)
            {
                this.animationProgresses.Add(0);
                this.animationSources.Add(new Point(0, 0));
                this.animationDirections.Add(AnimationDirection.In);
            }

            this.animationTimer.Tick += this.AnimationTimerOnTick;
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
            return this.animationProgresses.Count;
        }

        public object[] GetData()
        {
            if (!this.Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }

            if (this.animationDatas.Count == 0)
            {
                throw new Exception("Invalid animation");
            }

            return this.animationDatas[0];
        }

        public object[] GetData(int index)
        {
            if (!(index < this.animationDatas.Count))
            {
                throw new IndexOutOfRangeException("Invalid animation index");
            }

            return this.animationDatas[index];
        }

        public AnimationDirection GetDirection()
        {
            if (!this.Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }

            if (this.animationDirections.Count == 0)
            {
                throw new Exception("Invalid animation");
            }

            return this.animationDirections[0];
        }

        public AnimationDirection GetDirection(int index)
        {
            if (!(index < this.animationDirections.Count))
            {
                throw new IndexOutOfRangeException("Invalid animation index");
            }

            return this.animationDirections[index];
        }

        public double GetProgress()
        {
            if (!this.Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }

            if (this.animationProgresses.Count == 0)
            {
                throw new Exception("Invalid animation");
            }

            return this.GetProgress(0);
        }

        public double GetProgress(int index)
        {
            if (!(index < this.GetAnimationCount()))
            {
                throw new IndexOutOfRangeException("Invalid animation index");
            }

            switch (this.AnimationType)
            {
                case AnimationType.Linear:
                    return AnimationLinear.CalculateProgress(this.animationProgresses[index]);
                case AnimationType.EaseInOut:
                    return AnimationEaseInOut.CalculateProgress(this.animationProgresses[index]);
                case AnimationType.EaseOut:
                    return AnimationEaseOut.CalculateProgress(this.animationProgresses[index]);
                case AnimationType.CustomQuadratic:
                    return AnimationCustomQuadratic.CalculateProgress(this.animationProgresses[index]);
                default:
                    throw new NotImplementedException("The given AnimationType is not implemented");
            }
        }

        public Point GetSource(int index)
        {
            if (!(index < this.GetAnimationCount()))
            {
                throw new IndexOutOfRangeException("Invalid animation index");
            }

            return this.animationSources[index];
        }

        public Point GetSource()
        {
            if (!this.Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }

            if (this.animationSources.Count == 0)
            {
                throw new Exception("Invalid animation");
            }

            return this.animationSources[0];
        }

        public bool IsAnimating()
        {
            return this.animationTimer.Enabled;
        }

        public void SetData(object[] data)
        {
            if (!this.Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }

            if (this.animationDatas.Count == 0)
            {
                throw new Exception("Invalid animation");
            }

            this.animationDatas[0] = data;
        }

        public void SetDirection(AnimationDirection direction)
        {
            if (!this.Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }

            if (this.animationProgresses.Count == 0)
            {
                throw new Exception("Invalid animation");
            }

            this.animationDirections[0] = direction;
        }

        public void SetProgress(double progress)
        {
            if (!this.Singular)
            {
                throw new Exception("Animation is not set to Singular.");
            }

            if (this.animationProgresses.Count == 0)
            {
                throw new Exception("Invalid animation");
            }

            this.animationProgresses[0] = progress;
        }

        public void StartNewAnimation(AnimationDirection animationDirection, object[] data = null)
        {
            this.StartNewAnimation(animationDirection, new Point(0, 0), data);
        }

        public void StartNewAnimation(
            AnimationDirection animationDirection,
            Point animationSource,
            object[] data = null)
        {
            if (!this.IsAnimating() || this.InterruptAnimation)
            {
                if (this.Singular && this.animationDirections.Count > 0)
                {
                    this.animationDirections[0] = animationDirection;
                }
                else
                {
                    this.animationDirections.Add(animationDirection);
                }

                if (this.Singular && this.animationSources.Count > 0)
                {
                    this.animationSources[0] = animationSource;
                }
                else
                {
                    this.animationSources.Add(animationSource);
                }

                if (!(this.Singular && this.animationProgresses.Count > 0))
                {
                    switch (this.animationDirections[this.animationDirections.Count - 1])
                    {
                        case AnimationDirection.InOutRepeatingIn:
                        case AnimationDirection.InOutIn:
                        case AnimationDirection.In:
                            this.animationProgresses.Add(MinValue);
                            break;
                        case AnimationDirection.InOutRepeatingOut:
                        case AnimationDirection.InOutOut:
                        case AnimationDirection.Out:
                            this.animationProgresses.Add(MaxValue);
                            break;
                        default:
                            throw new Exception("Invalid AnimationDirection");
                    }
                }

                if (this.Singular && this.animationDatas.Count > 0)
                {
                    this.animationDatas[0] = data ?? new object[] {};
                }
                else
                {
                    this.animationDatas.Add(data ?? new object[] {});
                }
            }

            this.animationTimer.Start();
        }

        public void UpdateProgress(int index)
        {
            switch (this.animationDirections[index])
            {
                case AnimationDirection.InOutRepeatingIn:
                case AnimationDirection.InOutIn:
                case AnimationDirection.In:
                    this.IncrementProgress(index);
                    break;
                case AnimationDirection.InOutRepeatingOut:
                case AnimationDirection.InOutOut:
                case AnimationDirection.Out:
                    this.DecrementProgress(index);
                    break;
                default:
                    throw new Exception("No AnimationDirection has been set");
            }
        }

        private void AnimationTimerOnTick(object sender, EventArgs eventArgs)
        {
            for (var i = 0; i < this.animationProgresses.Count; i++)
            {
                this.UpdateProgress(i);

                if (!this.Singular)
                {
                    if (this.animationDirections[i] == AnimationDirection.InOutIn
                        && this.animationProgresses[i] == MaxValue)
                    {
                        this.animationDirections[i] = AnimationDirection.InOutOut;
                    }
                    else if (this.animationDirections[i] == AnimationDirection.InOutRepeatingIn
                             && this.animationProgresses[i] == MinValue)
                    {
                        this.animationDirections[i] = AnimationDirection.InOutRepeatingOut;
                    }
                    else if (this.animationDirections[i] == AnimationDirection.InOutRepeatingOut
                             && this.animationProgresses[i] == MinValue)
                    {
                        this.animationDirections[i] = AnimationDirection.InOutRepeatingIn;
                    }
                    else if ((this.animationDirections[i] == AnimationDirection.In
                              && this.animationProgresses[i] == MaxValue)
                             || (this.animationDirections[i] == AnimationDirection.Out
                                 && this.animationProgresses[i] == MinValue)
                             || (this.animationDirections[i] == AnimationDirection.InOutOut
                                 && this.animationProgresses[i] == MinValue))
                    {
                        this.animationProgresses.RemoveAt(i);
                        this.animationSources.RemoveAt(i);
                        this.animationDirections.RemoveAt(i);
                        this.animationDatas.RemoveAt(i);
                    }
                }
                else
                {
                    if (this.animationDirections[i] == AnimationDirection.InOutIn
                        && this.animationProgresses[i] == MaxValue)
                    {
                        this.animationDirections[i] = AnimationDirection.InOutOut;
                    }
                    else if (this.animationDirections[i] == AnimationDirection.InOutRepeatingIn
                             && this.animationProgresses[i] == MaxValue)
                    {
                        this.animationDirections[i] = AnimationDirection.InOutRepeatingOut;
                    }
                    else if (this.animationDirections[i] == AnimationDirection.InOutRepeatingOut
                             && this.animationProgresses[i] == MinValue)
                    {
                        this.animationDirections[i] = AnimationDirection.InOutRepeatingIn;
                    }
                }
            }

            if (this.OnAnimationProgress != null)
            {
                this.OnAnimationProgress(this);
            }
        }

        private void DecrementProgress(int index)
        {
            this.animationProgresses[index] -= this.animationDirections[index] == AnimationDirection.InOutOut
                                               || this.animationDirections[index]
                                               == AnimationDirection.InOutRepeatingOut
                ? this.SecondaryIncrement
                : this.Increment;
            if (this.animationProgresses[index] < MinValue)
            {
                this.animationProgresses[index] = MinValue;

                for (var i = 0; i < this.GetAnimationCount(); i++)
                {
                    if (this.animationDirections[i] == AnimationDirection.InOutIn)
                    {
                        return;
                    }
                    if (this.animationDirections[i] == AnimationDirection.InOutRepeatingIn)
                    {
                        return;
                    }
                    if (this.animationDirections[i] == AnimationDirection.InOutRepeatingOut)
                    {
                        return;
                    }
                    if (this.animationDirections[i] == AnimationDirection.InOutOut
                        && this.animationProgresses[i] != MinValue)
                    {
                        return;
                    }
                    if (this.animationDirections[i] == AnimationDirection.Out && this.animationProgresses[i] != MinValue)
                    {
                        return;
                    }
                }

                this.animationTimer.Stop();
                if (this.OnAnimationFinished != null)
                {
                    this.OnAnimationFinished(this);
                }
            }
        }

        private void IncrementProgress(int index)
        {
            this.animationProgresses[index] += this.Increment;
            if (this.animationProgresses[index] > MaxValue)
            {
                this.animationProgresses[index] = MaxValue;

                for (var i = 0; i < this.GetAnimationCount(); i++)
                {
                    if (this.animationDirections[i] == AnimationDirection.InOutIn)
                    {
                        return;
                    }
                    if (this.animationDirections[i] == AnimationDirection.InOutRepeatingIn)
                    {
                        return;
                    }
                    if (this.animationDirections[i] == AnimationDirection.InOutRepeatingOut)
                    {
                        return;
                    }
                    if (this.animationDirections[i] == AnimationDirection.InOutOut
                        && this.animationProgresses[i] != MaxValue)
                    {
                        return;
                    }
                    if (this.animationDirections[i] == AnimationDirection.In && this.animationProgresses[i] != MaxValue)
                    {
                        return;
                    }
                }

                this.animationTimer.Stop();
                if (this.OnAnimationFinished != null)
                {
                    this.OnAnimationFinished(this);
                }
            }
        }
    }
}