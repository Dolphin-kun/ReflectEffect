using System.ComponentModel.DataAnnotations;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Controls;
using YukkuriMovieMaker.Exo;
using YukkuriMovieMaker.ItemEditor.CustomVisibilityAttributes;
using YukkuriMovieMaker.Player.Video;
using YukkuriMovieMaker.Plugin.Effects;

namespace Reflection
{
    [VideoEffect("反射", ["アニメーション"], ["反射","Reflection","hansha"], isAviUtlSupported: false)]
    internal class ReflectionEffect : VideoEffectBase
    {
        public override string Label => "反射";

        [Display(GroupName = "反射", Name = "移動速度", Description = "移動速度")]
        [AnimationSlider("F1", "px", 0, 100)]
        public Animation V { get; } = new Animation(10, 0, 99999);

        [Display(GroupName = "反射", Name = "角度", Description = "角度")]
        [AnimationSlider("F1", "°", -90, 90)]
        public Animation Angle { get; } = new Animation(0, -3600, 3600);

        //範囲
        [Display(GroupName = "反射", Name = "範囲", Description = "反射する範囲")]
        [EnumComboBox()]
        public ReflectionEnum_Range RangeEnum { get => rangeEnum; set => Set(ref rangeEnum, value); }
        ReflectionEnum_Range rangeEnum = ReflectionEnum_Range.Screen;

        [Display(GroupName = "反射", Name = "幅", Description = "反射範囲の幅")]
        [AnimationSlider("F1", "px", 1, 100)]
        [ShowPropertyEditorWhen(nameof(RangeEnum),ReflectionEnum_Range.Custom)]
        public Animation CustomRangeWidth { get; } = new Animation(200, 1, 5000);

        [Display(GroupName = "反射", Name = "高さ", Description = "反射範囲の高さ")]
        [AnimationSlider("F1", "px", 1, 100)]
        [ShowPropertyEditorWhen(nameof(RangeEnum), ReflectionEnum_Range.Custom)]
        public Animation CustomRangeHeight { get; } = new Animation(200, 1, 5000);

        //当たり判定
        [Display(GroupName = "反射", Name = "判定", Description = "当たり判定")]
        [EnumComboBox()]
        public ReflectionEnum_Collision CollisionEnum { get => collisionEnum; set => Set(ref collisionEnum, value); }
        ReflectionEnum_Collision collisionEnum = ReflectionEnum_Collision.Bound;

        public override IEnumerable<string> CreateExoVideoFilters(int keyFrameIndex, ExoOutputDescription exoOutputDescription)
        {
            return [];
        }

        public override IVideoEffectProcessor CreateVideoEffect(IGraphicsDevicesAndContext devices)
        {
            return new ReflectionEffectProcessor(devices, this);
        }

        protected override IEnumerable<IAnimatable> GetAnimatables() => [V, Angle, CustomRangeWidth, CustomRangeHeight];
    }
}
