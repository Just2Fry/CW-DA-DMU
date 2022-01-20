using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PornHub.HyperMEM;
using static PornHub.HyperMEM.Vectors.Vec3;

namespace PornHub.Game
{
    class Threads
    {
        public static Vector3 ZmLocation = new Vector3();
        public static Vector3 BotLocation = new Vector3();
        public static Vector3 DogLocation = new Vector3();

        internal void UpdatePointers()
        {
            //Offsets.PlayerCompPtr = MEM.ReadInt64(MEM.GameBase + Security.Crypto.GetNumber(Offsets.PlayerBase));
            //Offsets.PlayerPedPtr = MEM.ReadInt64(MEM.GameBase + Security.Crypto.GetNumber(Offsets.PlayerBase) + 0x8);
            //Offsets.ZMGlobalBase = MEM.ReadInt64(MEM.GameBase + Security.Crypto.GetNumber(Offsets.PlayerBase) + 0x60);
            //Offsets.ZMBotBase = MEM.ReadInt64(MEM.GameBase + Security.Crypto.GetNumber(Offsets.PlayerBase) + 0x68);
            //Offsets.ZMBotListBase = MEM.ReadInt64(Offsets.ZMBotBase + Offsets.ZM_Bot_List_Offset);
            Offsets.PlayerCompPtr = MEM.ReadInt64(MEM.GameBase + Offsets.ZPlayerBase);
            Offsets.PlayerPedPtr = MEM.ReadInt64(MEM.GameBase + Offsets.ZPlayerBase + 0x8);
            Offsets.ZMGlobalBase = MEM.ReadInt64(MEM.GameBase + Offsets.ZPlayerBase) + 0x60;
            Offsets.ZMBotBase = MEM.ReadInt64(MEM.GameBase + Offsets.ZPlayerBase) + 0x68;
            Offsets.ZMBotListBase = MEM.ReadInt64(Offsets.ZMBotBase + Offsets.ZM_Bot_List_Offset);
        }

        public void Rapid_Thread()
        {
            while (true)
            {
                if (Form1.ThisForm.CB_Bots.Switched)
                {
                    for (int i = 4; i < 18; i++)
                    {
                        PlayerFunctions.Teleport(i,BotLocation);
                    }
                }

                if (Form1.ThisForm.CB_Rapid_P1.Switched)
                {
                    PlayerFunctions.SetRapidFire(0);
                }
                if (Form1.ThisForm.CB_Rapid_P2.Switched)
                {
                    PlayerFunctions.SetRapidFire(1);
                }
                if (Form1.ThisForm.CB_Rapid_P3.Switched)
                {
                    PlayerFunctions.SetRapidFire(2);
                }
                if (Form1.ThisForm.CB_Rapid_P4.Switched)
                {
                    PlayerFunctions.SetRapidFire(3);
                }
                Thread.Sleep(5);
            }
        }

        //public void zqinsXpShit_Thread()
        //{
        //    while (true)
        //    {
        //        //Weapon XP
        //        MEM.WriteFloat(MEM.GameBase + Offsets.zqinRankShit + 0x10, Form1.ThisForm.Bar_Weapon.Value);
        //
        //        //Rank XP
        //        MEM.WriteFloat(MEM.GameBase + Offsets.zqinRankShit + 0x0, Form1.ThisForm.Bar_Rank.Value);//visual
        //        MEM.WriteFloat(MEM.GameBase + Offsets.zqinRankShit + 0x8, Form1.ThisForm.Bar_Rank.Value);//real
        //    }
        //}
        public void Cycle_Thread() //Checks for gametype and assigns Weapon Cycle
        {
            while (true)
            {
                var zqiNMadeThis = MEM.ReadInt32(MEM.GameBase + Offsets.ZSeshState).ToString("X");
                if (zqiNMadeThis.EndsWith("21"))
                {
                    if (PlayerFunctions.GunList[0] != PlayerFunctions.GunListMP[0])
                    {
                        Form1.ThisForm.SetupMPGuns();
                        Form1.ThisForm.Btn_Dark.Enabled = false;
                        Form1.ThisForm.Dtn_Diamond.Enabled = true;
                        //MessageBox.Show("MP Detected");
                    }
                }
                else if (zqiNMadeThis.EndsWith("20"))
                {
                    if (PlayerFunctions.GunList[0] != PlayerFunctions.GunListZM[0])
                    {
                        Form1.ThisForm.SetupZMGuns();
                        Form1.ThisForm.Btn_Dark.Enabled = true;
                        Form1.ThisForm.Dtn_Diamond.Enabled = false;
                        //MessageBox.Show("ZM Detected");
                    }
                }
                Thread.Sleep(1000);
            }
        }

        //public void MPSettings_Thread() //Auto Sets Unlimited Score & Time
        //{
        //    while (true)
        //    {
        //        var zqiNSmart = MEM.ReadInt32(MEM.GameBase + Offsets.ZSeshState).ToString("X");
        //        var TimeLimit = MEM.ReadInt16(MEM.GameBase + Offsets.TimeLimit);
        //        var ScoreLimit = MEM.ReadInt16(MEM.GameBase + Offsets.ScoreLimit);
        //        if (zqiNSmart.EndsWith("21"))
        //        {
        //            if (TimeLimit != 0000 || ScoreLimit != 0000)
        //            {
        //                MEM.WriteInt16(MEM.GameBase + Offsets.TimeLimit, 0x0000);
        //                MEM.WriteInt16(MEM.GameBase + Offsets.ScoreLimit, 0x0000);
        //            }
        //        }
        //        Thread.Sleep(50);
        //    }
        //}

        public void Noclip_Thread()
        {
            while (true)
            {
                if (Form1.ThisForm.CB_Noclip.Switched)
                {
                    var Player1Movement = MEM.ReadByte(MEM.GameBase + Offsets.ZNoClipDir + (0x1A38 * 0));
                    if (Player1Movement == 1)
                    {
                        PlayerFunctions.Move2Angle(0, PlayerFunctions.GetPlayerAngles(0), Form1.ThisForm.bar_noclip.Value);
                    }
                    if (Player1Movement == 2) //Backwards
                    {
                        var ModdedAngle = PlayerFunctions.GetPlayerAngles(0);
                        ModdedAngle.X = -ModdedAngle.X;
                        PlayerFunctions.Move2Angle(0, ModdedAngle + new Vectors.Vec2.Vector2(0, 180), Form1.ThisForm.bar_noclip.Value);
                    }
                    if (Player1Movement == 3) //Left
                    {
                        var ModdedAngle = PlayerFunctions.GetPlayerAngles(0);
                        ModdedAngle.X = 0;
                        PlayerFunctions.Move2Angle(0, ModdedAngle + new Vectors.Vec2.Vector2(0, 90), Form1.ThisForm.bar_noclip.Value);
                    }
                    if (Player1Movement == 4) //Right
                    {
                        var ModdedAngle = PlayerFunctions.GetPlayerAngles(0);
                        ModdedAngle.X = 0;
                        PlayerFunctions.Move2Angle(0, ModdedAngle + new Vectors.Vec2.Vector2(0, -90), Form1.ThisForm.bar_noclip.Value);
                    }
                }
                Thread.Sleep(1);
            }
        }
        public static bool InGame = false;

        //public static void Runme()//Only runs when in a game and runs once - zqin
        //{
        //    for (int z = 1; z == 1; z++)
        //    {
        //        if (z == 1)
        //        Form1.ThisForm.AutoTextUpdate();
        //        z = 2;
        //    }
        //}
        public static void RestoreTool()
        {
            var Checks = Branding.GetControls.FindAllChildrenByType<ReaLTaiizor.Controls.MetroSwitch>(Form1.ThisForm);
            var Sliders = Branding.GetControls.FindAllChildrenByType<ReaLTaiizor.Controls.PoisonTrackBar>(Form1.ThisForm);

            foreach (ReaLTaiizor.Controls.MetroSwitch CB in Checks)
            {
                CB.Switched = false;
            }

            foreach (ReaLTaiizor.Controls.PoisonTrackBar Slider in Sliders)
            {
                Slider.Value = Slider.MouseWheelBarPartitions;
            }

            //restore TP
            MEM.WriteBytes(MEM.GameBase + Offsets.ZTeleport, new byte[] { 0x8B, 0x83, 0x80, 0x06, 0x00, 0x00, 0x89, 0x81, 0xD4, 0x02, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x83, 0x84, 0x06, 0x00, 0x00, 0xF3, 0x0F, 0x11, 0x81, 0xD8, 0x02, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x8B, 0x88, 0x06, 0x00, 0x00, 0xF3, 0x0F, 0x11, 0x89, 0xDC, 0x02, 0x00, 0x00, 0xC7, 0x83, 0xBC, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x33, 0xC9 });

            //Restore magic
            MEM.WriteBytes(MEM.GameBase + Offsets.ZShoot, new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08 });
            if (Form1.CodeCave != 0)
            {
                MEM.WriteBytes(Form1.CodeCave, new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
                Form1.CodeCave = 0;
            }

            //Restore magic 2
            MEM.WriteBytes(MEM.GameBase + Offsets.ZKill, new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08 });
            if (Form1.CodeCave2 != 0)
            {
                MEM.WriteBytes(Form1.CodeCave2, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                Form1.CodeCave2 = 0;
            }

            //Restore noclip
            MEM.WriteBytes(MEM.GameBase + Offsets.ZNoClipFunc, new byte[] { 0xF3, 0x0F, 0x11, 0x80, 0xE8, 0x0D, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x45, 0xA8, 0xF3, 0x0F, 0x11, 0x80, 0xF0, 0x0D, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x45, 0xAC, 0xF3, 0x0F, 0x11, 0x88, 0xEC, 0x0D, 0x00, 0x00 });

            //Restore Round Skip
            MEM.WriteBytes(MEM.GameBase + Offsets.ZRound, new byte[] { 0x8B, 0x91, 0x20, 0x02, 0x00, 0x00 });

            //restore XP
            var Restore_PAT = new byte[] { 0x40, 0x55, 0x56, 0x57, 0x41, 0x54 };
            MEM.WriteBytes(MEM.GameBase + Offsets.ZXP,  Restore_PAT);
            if (Form1.Cave_XP != 0)
            {
                var Restore_CV = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                MEM.WriteBytes(Form1.Cave_XP, Restore_CV);
            }

            //MEM.WriteAsciiString(MEM.GameBase + Offsets.EndMatchText, "Quit Match");
        }

        public static ulong GameTime = 0;
        public void Update_Thread()
        {
            UpdatePointers();

            //Main Threads
            var Threads = new Threads();

            //Rapid Thread
            var Rapid_Thread = new Thread(Threads.Rapid_Thread);
            Rapid_Thread.IsBackground = true;
            Rapid_Thread.Start();

            //Noclip Thread
            var Noclip_Thread = new Thread(Threads.Noclip_Thread);
            Noclip_Thread.IsBackground = true;
            Noclip_Thread.Start();

            //Weapon Cycle Check Thread
            var Cycle_Thread = new Thread(Threads.Cycle_Thread);
            Cycle_Thread.IsBackground = true;
            Cycle_Thread.Start();

            //MP Game Settings Thread
            //var MPSettings_Thread = new Thread(Threads.MPSettings_Thread);
            //MPSettings_Thread.IsBackground = true;
            //MPSettings_Thread.Start();

            //XP Thread
            //var zqiNsXP = new Thread(Threads.zqinsXpShit_Thread);
            //zqiNsXP.IsBackground = true;
            //zqiNsXP.Start();

            while (true)
            {
               //try
               // {
               //     var Currewnt_GameTime = MEM.ReadInt64(MEM.GameBase + Security.Crypto.GetNumber(Offsets.GameClock));
               //     
               //     if(Currewnt_GameTime == 0)
               //     {
               //         continue;
               //     }
               //     
               //     if (Currewnt_GameTime == GameTime)
               //     {
               //         Debug.WriteLine("Game was paused");
               //         Security.WebAuth.ReportMyself(true, "User May Have Froze Game for debugging");
               //     }
               //     else
               //     {
               //         GameTime = Currewnt_GameTime;
               //     }

                    UpdatePointers();

                    InGame = PlayerFunctions.GetPlayerName(0) != "UnnamedPlayer" && PlayerFunctions.GetPlayerName(0) != "";
                if (!InGame)
                {
                    RestoreTool();
                    Thread.Sleep(1000);
                }
                //if(InGame)
                //{
                //    Runme();
                //}

                //var GunID = MEM.ReadInt32(Offsets.PlayerCompPtr + ((Offsets.PC_ArraySize_Offset * (ulong)0) + Offsets.PC_SetWeaponID0));
                //var GunName = MEM.ReadAsciiString(MEM.GameBase + 0x17EB3F10, 32);
                //if(GunName != "")
                //{
                //    Debug.WriteLine(GunID + ":" + GunName);
                //    Thread.Sleep(500);
                //}
                //Form1.ThisForm.LBL_InGame.Text = "In Match: " + InGame.ToString();

                if (Form1.ThisForm.CB_TP_ZM.Switched)
                    {
                        MEM.WriteFloat(MEM.GameBase + Offsets.ZTeleport + 0x1, Threads.ZmLocation.X);
                        MEM.WriteFloat(MEM.GameBase + Offsets.ZTeleport + 0xC, Threads.ZmLocation.Y);
                        MEM.WriteFloat(MEM.GameBase + Offsets.ZTeleport + 0x17, Threads.ZmLocation.Z);
                    }

                    //player ein
                    if (Form1.ThisForm.CB_God_P1.Switched)
                    {
                        PlayerFunctions.SetGodMode(0, true);
                    }
                    else
                    {
                        PlayerFunctions.SetGodMode(0, false);
                    }

                    if (Form1.ThisForm.CB_Points_P1.Switched)
                    {
                        PlayerFunctions.SetPoints(0, 133700);
                    }
                    if (Form1.ThisForm.CB_Ammo_P1.Switched)
                    {
                        PlayerFunctions.UnlimitedAmmo(0);
                    }

                    if (Form1.ThisForm.CB_Speed_P1.Switched)
                    {
                        PlayerFunctions.SetPlayerSpeed(0, 2.5f);
                    }

                    if (Form1.ThisForm.CB_Crit_P1.Switched)
                    {
                        PlayerFunctions.SetCriticalKills(0);
                    }

                    if (Form1.ThisForm.CB_Jail_P1.Switched)
                    {
                        PlayerFunctions.SendToJail(0);
                    }

                    if (Form1.ThisForm.CB_Croshair_P1.Switched)
                    {
                        PlayerFunctions.SetZMPos(0);
                    }

                    //player dos
                    if (Form1.ThisForm.CB_God_P2.Switched)
                    {
                        PlayerFunctions.SetGodMode(1, true);
                    }
                    else
                    {
                        PlayerFunctions.SetGodMode(1, false);
                    }


                    if (Form1.ThisForm.CB_Points_P2.Switched)
                    {
                        PlayerFunctions.SetPoints(1, 133700);
                    }
                    if (Form1.ThisForm.CB_Ammo_P2.Switched)
                    {
                        PlayerFunctions.UnlimitedAmmo(1);
                    }

                    if (Form1.ThisForm.CB_Speed_P2.Switched)
                    {
                        PlayerFunctions.SetPlayerSpeed(1, 2.5f);
                    }

                    if (Form1.ThisForm.CB_Crit_P2.Switched)
                    {
                        PlayerFunctions.SetCriticalKills(1);
                    }

                    if (Form1.ThisForm.CB_Jail_P2.Switched)
                    {
                        PlayerFunctions.SendToJail(1);
                    }

                    if (Form1.ThisForm.CB_Croshair_P2.Switched)
                    {
                        PlayerFunctions.SetZMPos(1);
                    }

                    //player tres
                    if (Form1.ThisForm.CB_God_P3.Switched)
                    {
                        PlayerFunctions.SetGodMode(2, true);
                    }
                    else
                    {
                        PlayerFunctions.SetGodMode(2, false);
                    }


                    if (Form1.ThisForm.CB_Points_P3.Switched)
                    {
                        PlayerFunctions.SetPoints(2, 133700);
                    }
                    if (Form1.ThisForm.CB_Ammo_P3.Switched)
                    {
                        PlayerFunctions.UnlimitedAmmo(2);
                    }

                    if (Form1.ThisForm.CB_Speed_P3.Switched)
                    {
                        PlayerFunctions.SetPlayerSpeed(2, 2.5f);
                    }

                    if (Form1.ThisForm.CB_Crit_P3.Switched)
                    {
                        PlayerFunctions.SetCriticalKills(2);
                    }

                    if (Form1.ThisForm.CB_Jail_P3.Switched)
                    {
                        PlayerFunctions.SendToJail(2);
                    }

                    if (Form1.ThisForm.CB_Croshair_P3.Switched)
                    {
                        PlayerFunctions.SetZMPos(2);
                    }

                    //player cuatro
                    if (Form1.ThisForm.CB_God_P4.Switched)
                    {
                        PlayerFunctions.SetGodMode(3, true);
                    }
                    else
                    {
                        PlayerFunctions.SetGodMode(3, false);
                    }


                    if (Form1.ThisForm.CB_Points_P4.Switched)
                    {
                        PlayerFunctions.SetPoints(3, 133700);
                    }
                    if (Form1.ThisForm.CB_Ammo_P4.Switched)
                    {
                        PlayerFunctions.UnlimitedAmmo(3);
                    }

                    if (Form1.ThisForm.CB_Speed_P4.Switched)
                    {
                        PlayerFunctions.SetPlayerSpeed(3, 2.5f);
                    }

                    if (Form1.ThisForm.CB_Crit_P4.Switched)
                    {
                        PlayerFunctions.SetCriticalKills(3);
                    }

                    if (Form1.ThisForm.CB_Jail_P4.Switched)
                    {
                        PlayerFunctions.SendToJail(3);
                    }

                    if (Form1.ThisForm.CB_Croshair_P4.Switched)
                    {
                        PlayerFunctions.SetZMPos(3);
                    }
                //}
                //catch (Exception e)
                //{
                //    Debug.WriteLine("Error Caught at Update_Thread: " + e.ToString());
                //}

                int z = 1;
                    if (z == 1)
                    {

                    }
                    else
                    {
                        z = 1;
                    }
                Thread.Sleep(50);
            }
        }
    }
}