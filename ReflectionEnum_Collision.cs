using System.ComponentModel.DataAnnotations;

namespace Reflection
{
    public enum ReflectionEnum_Collision
    {
        [Display(Name = "アイテムサイズ", Description = "アイテムのサイズ")]
        Bound = 1,

        [Display(Name = "中心", Description = "中心")]
        Center = 2,
    }
}
