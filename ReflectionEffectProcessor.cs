using Vortice.Direct2D1;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Player.Video;

namespace Reflection
{
    internal class ReflectionEffectProcessor : IVideoEffectProcessor
    {
        private readonly IGraphicsDevicesAndContext devices;
        private readonly ReflectionEffect item;
        private ID2D1Image? input;

        public ID2D1Image Output => input ?? throw new NullReferenceException(nameof(input) + " is null");

        public ReflectionEffectProcessor(IGraphicsDevicesAndContext devices, ReflectionEffect item)
        {
            this.devices = devices;
            this.item = item;
        }

        public DrawDescription Update(EffectDescription effectDescription)
        {
            var frame = effectDescription.ItemPosition.Frame;
            var length = effectDescription.ItemDuration.Frame;
            var fps = effectDescription.FPS;

            var X = effectDescription.DrawDescription.Draw.X;
            var Y = effectDescription.DrawDescription.Draw.Y;

            var screenWidth = effectDescription.ScreenSize.Width;
            var screenHeight = effectDescription.ScreenSize.Height;

            var speed = item.V.GetValue(frame, length, fps);
            var angle = item.Angle.GetValue(frame, length, fps) * (Math.PI / 180.0);

            var vx = speed * Math.Cos(angle);
            var vy = speed * Math.Sin(angle);

            double rawX = vx * frame / fps * 60.0;
            double rawY = vy * frame / fps * 60.0;

            double posX = X + rawX;
            double posY = Y + rawY;

            double collisionWidth = 0;
            double collisionHeight = 0;

            double rangeWidth = 0;
            double rangeHeight = 0;

            // 反射範囲
            if (item.RangeEnum == ReflectionEnum_Range.Screen)
            {
                rangeWidth = screenWidth / 2.0;
                rangeHeight = screenHeight / 2.0;
            }
            else if (item.RangeEnum == ReflectionEnum_Range.Custom)
            {
                rangeWidth = item.CustomRangeWidth.GetValue(frame, length, fps);
                rangeHeight = item.CustomRangeHeight.GetValue(frame, length, fps);
            }

            // 当たり判定
            if (item.CollisionEnum == ReflectionEnum_Collision.Bound)
            {
                collisionWidth = devices.DeviceContext.GetImageLocalBounds(input).Right;
                collisionHeight = devices.DeviceContext.GetImageLocalBounds(input).Bottom;
            }

            //X方向
            if (rangeWidth <= 0 || collisionWidth * 2 >= rangeWidth * 2)
            {
                posX = Math.Clamp(posX, -rangeWidth, rangeWidth);
            }
            else
            {
                double maxX = rangeWidth - collisionWidth;
                double minX = -rangeWidth + collisionWidth;

                while (posX > maxX || posX < minX)
                {
                    if (posX > maxX)
                    {
                        double over = posX - maxX;
                        posX = maxX - over;
                    }
                    else if (posX < minX)
                    {
                        double over = minX - posX;
                        posX = minX + over;
                    }
                }
            }

            // Y方向
            if (rangeHeight <= 0 || collisionHeight * 2 >= rangeHeight * 2)
            {
                posY = Math.Clamp(posY, -rangeHeight, rangeHeight);
            }
            else
            {
                double maxY = rangeHeight - collisionHeight;
                double minY = -rangeHeight + collisionHeight;

                while (posY > maxY || posY < minY)
                {
                    if (posY > maxY)
                    {
                        double over = posY - maxY;
                        posY = maxY - over;
                    }
                    else if (posY < minY)
                    {
                        double over = minY - posY;
                        posY = minY + over;
                    }
                }
            }


            var drawDesc = effectDescription.DrawDescription;
            return drawDesc with
            {
                Draw = new(
                    (float)posX,
                    (float)posY,
                    drawDesc.Draw.Z
                ),
            };
        }


        public void ClearInput()
        {
            input = null;
        }

        public void SetInput(ID2D1Image? input)
        {
            this.input = input;
        }

        public void Dispose()
        {
        }
    }
}
