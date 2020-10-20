using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnsoulSharp.Loader.Service;
using EnsoulSharp.SDK.Core;
using Newtonsoft.Json;

namespace mango.SkinChanger
{
    using EnsoulSharp;
    using EnsoulSharp.SDK;
    using EnsoulSharp.SDK.MenuUI;
    using EnsoulSharp.SDK.MenuUI.Values;
    using EnsoulSharp.SDK.Prediction;
    using EnsoulSharp.SDK.Utility;

    using Color = System.Drawing.Color;

    class Program
    {
        private static Menu MainMenu;

        static void Main(string[] args)
        {
            // start init script when game already load
            GameEvent.OnGameLoad += OnGameLoad;
        }

        private static void OnGameLoad()
        {
            Game.Print("mango.SkinChanger loaded", Color.Coral);

            MainMenu = new Menu("mango.SkinChanger", "mango.SkinChanger", true);
            MainMenu.Attach();

            var useSkinChanger = new MenuBool("useSkinChanger", "Use Skin Changer?", false);
            MainMenu.Add(useSkinChanger);

            var skinNames = Test.Skins[ObjectManager.Player.CharacterName].Keys.ToArray();

            var skinList = new MenuList("skins", "Skins", skinNames);

            MainMenu.Add(skinList);

            Game.OnUpdate += OnUpdate;
            skinList.ValueChanged += SkinListOnValueChanged;

            Console.WriteLine(skinList.Items.Length);
            Console.WriteLine(MainMenu["skins"].GetValue<MenuList>().Index);

            if (MainMenu["skins"].GetValue<MenuList>().Index >= skinList.Items.Length)
                MainMenu["skins"].GetValue<MenuList>().Index = 0;

            Console.WriteLine(MainMenu["skins"].GetValue<MenuList>().Index);
        }

        private static void OnUpdate(EventArgs args)
        {

        }

        private static void SkinListOnValueChanged(object sender, EventArgs e)
        {
            var skinList = (MenuList)sender;
            Console.WriteLine(skinList.SelectedValue);

            if (MainMenu["useSkinChanger"].GetValue<MenuBool>().Enabled)
            {
                var player = ObjectManager.Player;
                var selectedSkin = skinList.SelectedValue;
                var skinIdToSet = Test.Skins[player.CharacterName][selectedSkin];

                player.SetSkin(skinIdToSet);
            }
        }
    }
}
