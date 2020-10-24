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
            GameEvent.OnGameLoad += OnGameLoad;
        }

        private static void OnGameLoad()
        {
            Game.OnNotify += OnNotify; 

            Game.Print("mango.SkinChanger loaded", Color.Coral);

            //main menu
            MainMenu = new Menu("mango.SkinChanger", "mango.SkinChanger", true);
            MainMenu.Attach();

            //menu bool
            var useSkinChanger = new MenuBool("useSkinChanger", "Use Skin Changer?", false);
            MainMenu.Add(useSkinChanger);

            //fetch champion skin names 
            var skinNames = ChampionSkinData.Skins[ObjectManager.Player.CharacterName].Keys.ToArray();

            //create MenuList
            var skinList = new MenuList("skins", "Skins", skinNames);

            MainMenu.Add(skinList);

            //skinList event handler
            skinList.ValueChanged += SkinListOnValueChanged;

            //retarded check if our last index is >= array length
            if (MainMenu["skins"].GetValue<MenuList>().Index >= skinList.Items.Length)
                MainMenu["skins"].GetValue<MenuList>().Index = 0;

            //setting skin OnLoad
            ObjectManager.Player.SetSkin(ChampionSkinData.Skins[ObjectManager.Player.CharacterName][skinList.SelectedValue]);
        }

        private static void OnNotify(GameNotifyEventArgs args)
        {
            switch (args.EventId)
            {
                case GameEventId.OnReincarnate:
                case GameEventId.OnResetChampion:
                    SetSkinId();
                    break;
            }
        }

        private static void SkinListOnValueChanged(object sender, EventArgs e) => SetSkinId();
        
        private static void SetSkinId()
        {
            if (!MainMenu["useSkinChanger"].GetValue<MenuBool>().Enabled) return;
            var skinList = MainMenu["skins"].GetValue<MenuList>();

            var player = ObjectManager.Player;
            var selectedSkin = skinList.SelectedValue;

            //picking skin id
            var skinIdToSet = ChampionSkinData.Skins[player.CharacterName][selectedSkin];

            player.SetSkin(skinIdToSet);
        }
    }
}
