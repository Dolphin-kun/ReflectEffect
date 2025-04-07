using System.ComponentModel.DataAnnotations;

namespace Reflection
{
    public enum ReflectionEnum_Range
    {
        [Display(Name = "画面サイズ", Description = "画面サイズ")]
        Screen = 1,

        [Display(Name = "カスタム", Description = "カスタム")]
        Custom = 2,
    }
}
