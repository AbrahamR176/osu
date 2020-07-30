// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Osu.Objects.Drawables;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Osu.Skinning
{
    /// <summary>
    /// Legacy skinned spinner with one main spinning layer and a background layer.
    /// </summary>
    public class LegacyOldStyleSpinner : CompositeDrawable
    {
        private DrawableSpinner drawableSpinner;
        private Sprite disc;

        [BackgroundDependencyLoader]
        private void load(ISkinSource source, DrawableHitObject drawableObject)
        {
            drawableSpinner = (DrawableSpinner)drawableObject;

            Scale = new Vector2(0.625f);

            InternalChildren = new Drawable[]
            {
                new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Texture = source.GetTexture("spinner-background")
                },
                disc = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Texture = source.GetTexture("spinner-circle")
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            this.FadeOut();
            drawableSpinner.State.BindValueChanged(updateStateTransforms, true);
        }

        private void updateStateTransforms(ValueChangedEvent<ArmedState> state)
        {
            var spinner = drawableSpinner.HitObject;

            using (BeginAbsoluteSequence(spinner.StartTime - spinner.TimePreempt / 2, true))
                this.FadeInFromZero(spinner.TimePreempt / 2);
        }

        protected override void Update()
        {
            base.Update();
            disc.Rotation = drawableSpinner.RotationTracker.Rotation;
        }
    }
}
