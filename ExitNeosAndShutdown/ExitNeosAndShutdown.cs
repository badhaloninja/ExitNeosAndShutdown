using FrooxEngine;
using HarmonyLib;
using NeosModLoader;
using System;
using FrooxEngine.UIX;
using BaseX;
using System.Diagnostics;

namespace ExitNeosAndShutdown
{
    public class ExitNeosAndShutdown : NeosMod
    {
        public override string Name => "ExitNeosAndShutdown";
        public override string Author => "badhaloninja";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/badhaloninja/ExitNeosAndShutdown";
        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("me.badhaloninja.ExitNeosAndShutdown");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(ExitScreen), "OnStart")]
        private class ExitScreen_OnStart_Patch
        {
            public static void Postfix(ExitScreen __instance)
            {
                var buttonr = __instance.ScreenCanvas.Slot.GetComponentInChildren<Button>();
                UIBuilder ui = new UIBuilder(buttonr.Slot.Parent);
                
                RadiantUI_Constants.SetupDefaultStyle(ui);
                color default_BACKGROUND = UserspaceRadiantDash.DEFAULT_BACKGROUND;

                var icon = new Uri("neosdb:///04d7fb7edabf9de12b34f90cffed659f0059ec319ef89b01cc3f6f3270f6f52b.png");
                var bg = new Uri("neosdb:///f0bcf2560e4bab1527f98459a06b2f28bbc80e7186d80ecfb37f651245b9491b.webp");
                var font = new Uri("neosdb:///2bef27c595904db1e3d89d9bdce9ab0531fa85d875a1cea1cb15a34dee7e0b35");

                var btn = AddButton(__instance, ui, icon, "Shutdown", null);
                btn.Slot.PersistentSelf = false;
                var image = btn.Slot.GetComponent<Image>();
                
                image.Sprite.Target = image.Slot.AttachSprite(bg);

                btn.Label.Font.Target = btn.Label.Slot.AttachFont(font);

                btn.LocalPressed += Shutdown;
            }

            [HarmonyReversePatch]
            [HarmonyPatch(typeof(ExitScreen), "AddButton")]
            public static Button AddButton(object instance, UIBuilder ui, Uri icon, LocaleString label, ButtonEventHandler action)
            {
                // its a stub so it has no initial content
                throw new NotImplementedException("It's a stub");
            }

            private static void Shutdown(IButton button, ButtonEventData eventData)
            {
                var psi = new ProcessStartInfo("shutdown", "/s /t 0") ///t 0
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                Process.Start(psi);
            }
        }
    }
}