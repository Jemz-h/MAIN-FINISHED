using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;   

namespace Project_Sauyo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("SuperCartoon.ttf", "SuperCartoon");
                });

#if ANDROID
            // Remove ripple/white cast effect on ImageButton (Android only)
            ImageButtonHandler.Mapper.AppendToMapping("NoRipple", (handler, view) =>
            {
                var v = handler.PlatformView;
                v.StateListAnimator = null;   // disables press animation
                v.SetBackground(null);        // removes default ripple background
            });
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
