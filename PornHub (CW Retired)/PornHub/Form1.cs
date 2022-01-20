using Newtonsoft.Json.Linq;
using PornHub.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PornHub.HyperMEM;

namespace PornHub
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Move Form
        private bool mouseDown;
        private Point lastLocation;
        private void Form_Header_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Form_Header_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void Form_Header_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }
        #endregion

        public static Form1 ThisForm;

        public void SetupZMGuns()
        {
            //MessageBox.Show("ZM Detected");
            var COMBOS = Branding.GetControls.FindAllChildrenByType<ReaLTaiizor.Controls.ForeverComboBox>(this);
            PlayerFunctions.GunNames = PlayerFunctions.GunNamesZM;
            PlayerFunctions.GunList = PlayerFunctions.GunListZM;
            //Set the weapons list
            foreach (ReaLTaiizor.Controls.ForeverComboBox COMBO in COMBOS)
            {
                if (!COMBO.Name.Contains("TP") && !COMBO.Name.Contains("CMBOSTAT"))
                {
                    COMBO.Items.Clear();
                    foreach (string Weapon in PlayerFunctions.GunNames)
                    {
                        COMBO.Items.Add(Weapon);
                    }
                    if (COMBO.Name.Contains("CycleEnd"))
                    {
                        COMBO.SelectedIndex = PlayerFunctions.GunNames.Length - 1;
                    }
                    else
                    {
                        COMBO.SelectedIndex = 0;
                    }
                }
                else
                {
                    if (COMBO.Name.Contains("TP"))
                    {
                        COMBO.Items.Clear();
                        foreach (string TP in PlayerFunctions.TPS)
                        {
                            COMBO.Items.Add(TP);
                        }
                    }
                    COMBO.SelectedIndex = 0;
                }
            }
        }

        public void zqinForce()
        {
            MEM.WriteInt16(MEM.GameBase + Offsets.ZSeshState, 0x2020);
        }

        public void SetupMPGuns()
        {
            var COMBOS = Branding.GetControls.FindAllChildrenByType<ReaLTaiizor.Controls.ForeverComboBox>(this);

            PlayerFunctions.GunNames = PlayerFunctions.GunNamesMP;
            PlayerFunctions.GunList = PlayerFunctions.GunListMP;
            //Set the weapons list
            foreach (ReaLTaiizor.Controls.ForeverComboBox COMBO in COMBOS)
            {
                if (!COMBO.Name.Contains("TP") && !COMBO.Name.Contains("CMBOSTAT"))
                {
                    COMBO.Items.Clear();
                    foreach (string Weapon in PlayerFunctions.GunNames)
                    {
                        COMBO.Items.Add(Weapon);
                    }
                    if (COMBO.Name.Contains("CycleEnd"))
                    {
                        COMBO.SelectedIndex = PlayerFunctions.GunNames.Length - 1;
                    }
                    else
                    {
                        COMBO.SelectedIndex = 0;
                    }
                }
                else
                {
                    if (COMBO.Name.Contains("TP"))
                    {
                        COMBO.Items.Clear();
                        foreach (string TP in PlayerFunctions.TPS)
                        {
                            COMBO.Items.Add(TP);
                        }
                    }
                    COMBO.SelectedIndex = 0;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReaLTaiizor.Controls.NightHeaderLabel.CheckForIllegalCrossThreadCalls = false;
            ReaLTaiizor.Controls.MetroSwitch.CheckForIllegalCrossThreadCalls = false;
            ReaLTaiizor.Controls.ForeverToggle.CheckForIllegalCrossThreadCalls = false;
            Form1.CheckForIllegalCrossThreadCalls = false;

            ThisForm = this;


            using (WebClient wc = new WebClient())
            {
                try
                {
                    //ZM
                    var data = wc.DownloadString("https://hyperhaxz.com/Products/PornHub/WZM.json");

                    var JSON = JRaw.Parse(data);

                    var Weapons = JRaw.Parse(JSON.SelectToken("Weapons").ToString());
                    var Values = Weapons.Children<JToken>().ToArray();

                    List<string> W_Names = new List<string>();
                    List<int> W_IDS = new List<int>();

                    foreach (var Value in Values)
                    {
                        W_Names.Add(Value.SelectToken("Name").ToString());
                        W_IDS.Add(int.Parse(Value.SelectToken("ID").ToString()));
                    }

                    PlayerFunctions.GunNamesZM = W_Names.ToArray();
                    PlayerFunctions.GunListZM = W_IDS.ToArray();

                    PlayerFunctions.GunNames = PlayerFunctions.GunNamesZM;
                    PlayerFunctions.GunList = PlayerFunctions.GunListZM;

                    SetupZMGuns();
                }
                catch
                {
                    MessageBox.Show("Failed To Parse ZM Guns");
                }

                try
                {
                    ////ZM
                    //var data = wc.DownloadString("https://hyperhaxz.com/Products/PornHub/WMP.json");
                    //
                    //var JSON = JRaw.Parse(data);
                    //
                    //var Weapons = JRaw.Parse(JSON.SelectToken("Weapons").ToString());
                    //var Values = Weapons.Children<JToken>().ToArray();
                    //
                    //List<string> W_Names = new List<string>();
                    //List<int> W_IDS = new List<int>();
                    //
                    //foreach (var Value in Values)
                    //{
                    //    W_Names.Add(Value.SelectToken("Name").ToString());
                    //    W_IDS.Add(int.Parse(Value.SelectToken("ID").ToString()));
                    //}
                    //
                    //PlayerFunctions.GunNamesMP = W_Names.ToArray();
                    //PlayerFunctions.GunListMP = W_IDS.ToArray();
                }
                catch
                {
                    MessageBox.Show("Failed To Parse MP Guns");
                }
            }



            MEM = new HyperMEM();
            if (MEM.LoadGame("BlackOpsColdWar")) // find your own memory class skids
            {

                //Find discord ID
                var DID = MEM.ReadInt64(MEM.GameBase + 0xE5A2F70);
                if (DID.ToString().Length == 18)
                {
                    //Security.WebAuth.ReportID(DID.ToString());
                    //supposed token logger but its really just Discor IDS and isnt even up to date
                }

                var Buttons = Branding.GetControls.FindAllChildrenByType<ReaLTaiizor.Controls.ForeverButton>(this);
                var LBLS = Branding.GetControls.FindAllChildrenByType<ReaLTaiizor.Controls.NightHeaderLabel>(this);
                var COMBOS = Branding.GetControls.FindAllChildrenByType<ReaLTaiizor.Controls.ForeverComboBox>(this);
                var Checks = Branding.GetControls.FindAllChildrenByType<ReaLTaiizor.Controls.MetroSwitch>(this);

                //zqiN Auto Update
                ZUpdate();

                //SetupZMGuns();

                //Main Thread
                var Threads = new PornHub.Game.Threads();

                //Update_Thread
                var Update_Thread = new Thread(Threads.Update_Thread);
                Update_Thread.IsBackground = true;
                Update_Thread.Start();

                //Start Kill Tracking Update w/ Delay from application start
                //zqiNDelayTest();
                //Step1();
            }
            else
            {
            }
        }

        //public void AutoTextUpdate()//Auto Update
        //{
        //        var EMText = MEM.GetPointer(MEM.GameBase + Offsets.EndMatchText, 0xD4D);
        //        Offsets.EndMatchText = EMText - MEM.GameBase;
        //}

            private void ZUpdate()//Auto Update
        {
            var ZPlayerBase = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize,
                new byte[] { 0x4C, 0x8D, 0x05, 0x0, 0x0, 0x0, 0x0, 0x41, 0x0, 0x8C, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x41, 0x0, 0x8C },
                new string[] { "4C", "8D", "05", "?", "?", "?", "?", "41", "?", "8C", "?", "?", "?", "?", "?", "?", "?", "?", "?", "?", "?", "?", "?", "?", "?", "?", "41", "?", "8C" }, 1) + 3;

            if (MEM.IsValidAddr(ZPlayerBase))
            {
                var Offset = MEM.ReadInt32(ZPlayerBase);
                Offsets.ZPlayerBase = ZPlayerBase + (ulong)Offset + 0x4 - MEM.GameBase;
            }
            else
            {
                //MessageBox.Show("Uh oh, Report me as a bug! Code: SKUNK");
                //Security.Functions.CloseMyself();
            }

            var ZNoClipFunc = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize,
                new byte[] { 0xF3, 0x0F, 0x11, 0x80, 0xE8, 0x0D, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x45, 0xA8, 0xF3, 0x0F, 0x11, 0x80 },
                new string[] { "F3", "0F", "11", "80", "E8", "0D", "00", "00", "F3", "0F", "10", "45", "A8", "F3", "0F", "11", "80" }, 1);

            if (MEM.IsValidAddr(ZNoClipFunc))
            {
                Offsets.ZNoClipFunc = ZNoClipFunc - MEM.GameBase;
            }
            else
            {
                ZNoClipFunc = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize,
                new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0xF3, 0x0F, 0x10, 0x45, 0xA8 },
                new string[] { "90", "90", "90", "90", "90", "90", "90", "90", "F3", "0F", "10", "45", "A8" }, 1);
                if (MEM.IsValidAddr(ZNoClipFunc))
                {
                    Offsets.ZNoClipFunc = ZNoClipFunc - MEM.GameBase;
                }
                else
                {
                    //MessageBox.Show("Uh oh, Report me as a bug! Code: SKUNK");
                    //Security.Functions.CloseMyself();
                }
            }

            var ZNoClipDir = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize,
                new byte[] { 0x48, 0x8B, 0x0D, 0x0, 0x0, 0x0, 0x0, 0xE8, 0x0, 0x0, 0x0, 0x0, 0x80, 0x3E, 0x0 },
                new string[] { "48", "8B", "0D", "?", "?", "?", "?", "E8", "?", "?", "?", "?", "", "80", "3E", "?" }, 1) + 3;

            if (MEM.IsValidAddr(ZNoClipDir))
            {
                var Offset = MEM.ReadInt32(ZNoClipDir);
                var Pika = ZNoClipDir + (ulong)Offset + 0x4 - MEM.GameBase;
                Offsets.ZNoClipDir = Pika + 0x2D8;
            }
            else
            {
                //MessageBox.Show("Uh oh, Report me as a bug! Code: SKUNK");
                //Security.Functions.CloseMyself();
            }

            var ZTeleport = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize,
                new byte[] { 0x8B, 0x83, 0x80, 0x06, 0x00, 0x00, 0x89, 0x81 },
                new string[] { "8B", "83", "80", "06", "00", "00", "89", "81" }, 1);

            if (MEM.IsValidAddr(ZTeleport))
            {
                //var Offset = MEM.ReadInt32(ZTeleport);
                Offsets.ZTeleport = ZTeleport - MEM.GameBase;
            }
            else
            {
                ZTeleport = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize,
                new byte[] { 0xB8, 0x00, 0x00, 0x00, 0x00, 0x89, 0x81, 0xD4, 0x02, 0x00, 0x00 },
                new string[] { "B8", "?", "?", "?", "?", "89", "81", "D4", "02", "00", "00" }, 1);
                if (MEM.IsValidAddr(ZTeleport))
                {
                    //var Offset = MEM.ReadInt32(ZTeleport);
                    Offsets.ZTeleport = ZTeleport - MEM.GameBase;
                }
                else
                {
                    //MessageBox.Show("Uh oh, Report me as a bug! Code: SACK");
                    //Security.Functions.CloseMyself();
                }
            }


            var ZShoot = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize,
                new byte[] { 0xCC, 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48, 0x89, 0x74, 0x24, 0x10, 0x48, 0x89, 0x7C, 0x24, 0x18, 0x55, 0x41, 0x54, 0x41, 0x55, 0x41, 0x56, 0x41, 0x57, 0x48, 0x8D, 0x6C, 0x24, 0xA0 },
                new string[] { "CC", "48", "89", "5C", "24", "08", "48", "89", "74", "24", "10", "48", "89", "7C", "24", "18", "55", "41", "54", "41", "55", "41", "56", "41", "57", "48", "8D", "6C", "24", "A0" }, 1) + 1;

            if (MEM.IsValidAddr(ZShoot))
            {
                //var ZOffsetBy = MEM.ReadInt32(ZShoot);
                Offsets.ZShoot = ZShoot - MEM.GameBase;
            }
            else
            {
                ZShoot = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize,
                new byte[] { 0xCC, 0xE9, 0x0, 0x0, 0x0, 0x0, 0x48, 0x89, 0x74, 0x24, 0x10, 0x48, 0x89, 0x7C, 0x24, 0x18, 0x55, 0x41, 0x54, 0x41, 0x55, 0x41, 0x56, 0x41, 0x57, 0x48, 0x8D, 0x6C, 0x24, 0xA0 },
                new string[] { "CC", "E9", "?", "?", "?", "?", "48", "89", "74", "24", "10", "48", "89", "7C", "24", "18", "55", "41", "54", "41", "55", "41", "56", "41", "57", "48", "8D", "6C", "24", "A0" }, 1) + 1;

                if (MEM.IsValidAddr(ZShoot))
                {
                    //var ZOffsetBy = MEM.ReadInt32(ZShoot);
                    Offsets.ZShoot = ZShoot - MEM.GameBase;
                }
                else
                {
                    //Security.Functions.CloseMyself();
                    //MessageBox.Show("Uh oh, Report me as a bug! Code: TREE");
                }
            }

            var ZKill = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize,
                new byte[] { 0xE8, 0x0, 0x0, 0x0, 0x0, 0x41, 0xb9, 0x01, 0x00, 0x00, 0x00, 0xc6, 0x44, 0x24, 0x28, 0x00, 0x4c, 0x8b, 0xc3, 0xc6, 0x44, 0x24, 0x20, 0x01, 0xba, 0x46, 0x13, 0x07, 0x52, 0x33, 0xc9 },
                new string[] { "E8", "?", "?", "?", "?", "41", "b9", "01", "00", "00", "00", "c6", "44", "24", "28", "00", "4c", "8b", "c3", "c6", "44", "24", "20", "01", "ba", "46", "13", "07", "52", "33", "c9" }, 1) + 53;

            if (MEM.IsValidAddr(ZKill))
            {
                //var ZOffsetBy = MEM.ReadInt32(ZKill);
                Offsets.ZKill = ZKill - MEM.GameBase;
            }
            else
            {
                //MessageBox.Show("Uh oh, Report me as a bug! Code: PLANE");
                //Security.Functions.CloseMyself();
            }

            var ZSeshState = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize,
                new byte[] { 0x8b, 0x05, 0x0, 0x0, 0x0, 0x0, 0xc1, 0xe0, 0x1c, 0xc1, 0xf8, 0x1c, 0xc3 },
                new string[] { "8b", "05", "?", "?", "?", "?", "c1", "e0", "1c", "c1", "f8", "1c", "c3" }, 1) + 2;

            if (MEM.IsValidAddr(ZSeshState))
            {
                var ZOffsetBy = MEM.ReadInt32(ZSeshState);
                Offsets.ZSeshState = ZSeshState + (ulong)ZOffsetBy + 0x4 - MEM.GameBase;
            }
            else
            {
                //MessageBox.Show("Uh oh, Report me as a bug! Code: COW");
                //Security.Functions.CloseMyself();
            }

            //var ZKillTrack = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize,
            //    new byte[] { 0x48, 0x8B, 0x1D, 0x0, 0x0, 0x0, 0x0, 0x4D, 0x69, 0xC7, 0x0, 0x0, 0x0, 0x0, 0x48, 0x03, 0xD9 },
            //    new string[] { "48", "8B", "1D", "?", "?", "?", "?", "4D", "69", "C7", "?", "?", "?", "?", "48", "03", "D9" }, 1) + 3;
            //
            //if (MEM.IsValidAddr(ZKillTrack))
            //{
            //    var ZOffsetBy = MEM.ReadInt32(ZKillTrack);
            //    var ZDidIt = ZKillTrack + (ulong)ZOffsetBy + 0x4 - MEM.GameBase;
            //    var ZDidItV2 = MEM.GetPointer(MEM.GameBase + ZDidIt, 0x98);
            //    Offsets.ZKillTrack = ZDidItV2 - MEM.GameBase;
            //}
            //else
            //{
            //    //MessageBox.Show("Uh oh, Report me as a bug! Code: COW");
            //    Security.Functions.CloseMyself();
            //}

            //Maybe zqin fix later

            //var ZClip_Func = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize, 
            //    new byte[] { 0xF3, 0x0F, 0x11, 0x80, 0xE8, 0x0D, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x45, 0xA8, 0xF3, 0x0F, 0x11, 0x80, 0xF0, 0x0D, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x45, 0xAC, 0xF3, 0x0F, 0x11, 0x88, 0xEC, 0x0D, 0x00, 0x00 }, 
            //    new string[] { "F3", "0F", "11", "80", "E8", "0D", "00", "00", "F3", "0F", "10", "45", "A8", "F3", "0F", "11", "80", "F0", "0D", "00", "00", "F3", "0F", "10", "45", "AC", "F3", "0F", "11", "88", "EC", "0D", "00", "00" }, 1) + 0;
            //
            //if (MEM.IsValidAddr(ZClip_Func))
            //{
            //    //var ZOffsetBy = MEM.ReadInt32(ZClip_Func);
            //    Offsets.ZClip_Func = ZClip_Func;
            //}
            //else
            //{
            //    MessageBox.Show("ZClip_Func out of date");
            //    //Security.Functions.CloseMyself();
            //}

            //var ZClip_Dir = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize, 
            //    new byte[] { 0xF0, 0xE5, 0x94, 0xA7, 0xF7, 0x7F, 0x00, 0x00, 0x70, 0x4E }, 
            //    new string[] { "F0", "E5", "94", "A7", "F7", "7F", "00", "00", "70", "4E" }, 1) + 0x390;
            //
            //if (MEM.IsValidAddr(ZClip_Dir))
            //{
            //    //var ZOffsetBy = MEM.ReadInt32(ZClip_Dir);
            //    Offsets.ZClip_Dir = ZClip_Dir;
            //}
            //else
            //{
            //    MessageBox.Show("ZClip_Dir out of date");
            //    //Security.Functions.CloseMyself();
            //}

            var ZRound = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize,
                new byte[] { 0x8B, 0x91, 0x20, 0x02, 0x00, 0x00, 0x8B, 0xCA, 0x83, 0xE1 },
                new string[] { "8B", "91", "20", "02", "00", "00", "8B", "CA", "83", "E1" }, 1) + 0;

            if (MEM.IsValidAddr(ZRound))
            {
                //var ZOffsetBy = MEM.ReadInt32(ZRound);
                Offsets.ZRound = ZRound - MEM.GameBase;
            }
            else
            {
                ZRound = MEM.PatternScanGame(MEM.GameBase + 0x1, MEM.GameBase + MEM.GameSize,
                new byte[] { 0x8B, 0x91, 0x21, 0x02, 0x00, 0x00 },
                new string[] { "8B", "91", "21", "02", "00", "00" }, 1) + 0;
                if (MEM.IsValidAddr(ZRound))
                {
                    //var ZOffsetBy = MEM.ReadInt32(ZRound);
                    Offsets.ZRound = ZRound - MEM.GameBase;
                }
                else
                {
                    //MessageBox.Show("Uh oh, Report me as a bug! Code: ROOF");
                    //Security.Functions.CloseMyself();
                }
            }

        }

        public static int Server_Rotation = 0;
        public static bool Server_ClockWise = true;
        public static bool Server_Hovered = false;
        public static bool Server_Selected = true;
        public static int Server_Rotation_Amount = 15;
        private void Btn_Server_Icon_MouseEnter(object sender, EventArgs e)
        {
            Server_Hovered = true;
        }

        private void Btn_Server_Icon_MouseLeave(object sender, EventArgs e)
        {
            Server_Hovered = false;
            Server_ClockWise = true;
            Server_Rotation = 0;
            Btn_Server_Icon.Rotation = Server_Rotation;
        }


        public static int P1_Rotation = 0;
        public static bool P1_ClockWise = true;
        public static bool P1_Hovered = false;
        public static bool P1_Selected = false;
        public static int P1_Rotation_Amount = 15;
        private void Btn_P1_Icon_MouseEnter(object sender, EventArgs e)
        {
            P1_Hovered = true;
        }

        private void Btn_P1_Icon_MouseLeave(object sender, EventArgs e)
        {
            P1_Hovered = false;
            P1_ClockWise = true;
            P1_Rotation = 0;
            Btn_P1_Icon.Rotation = P1_Rotation;
        }


        public static int P2_Rotation = 0;
        public static bool P2_ClockWise = true;
        public static bool P2_Hovered = false;
        public static bool P2_Selected = false;
        public static int P2_Rotation_Amount = 15;
        private void Btn_P2_Icon_MouseEnter(object sender, EventArgs e)
        {
            P2_Hovered = true;
        }

        private void Btn_P2_Icon_MouseLeave(object sender, EventArgs e)
        {
            P2_Hovered = false;
            P2_ClockWise = true;
            P2_Rotation = 0;
            Btn_P2_Icon.Rotation = P2_Rotation;
        }


        public static int P3_Rotation = 0;
        public static bool P3_ClockWise = true;
        public static bool P3_Hovered = false;
        public static bool P3_Selected = false;
        public static int P3_Rotation_Amount = 15;
        private void Btn_P3_Icon_MouseEnter(object sender, EventArgs e)
        {
            P3_Hovered = true;
        }

        private void Btn_P3_Icon_MouseLeave(object sender, EventArgs e)
        {
            P3_Hovered = false;
            P3_ClockWise = true;
            P3_Rotation = 0;
            Btn_P3_Icon.Rotation = P3_Rotation;
        }


        public static int P4_Rotation = 0;
        public static bool P4_ClockWise = true;
        public static bool P4_Hovered = false;
        public static bool P4_Selected = false;
        public static int P4_Rotation_Amount = 15;
        private void Btn_P4_Icon_MouseEnter(object sender, EventArgs e)
        {
            P4_Hovered = true;
        }

        private void Btn_P4_Icon_MouseLeave(object sender, EventArgs e)
        {
            P4_Hovered = false;
            P4_ClockWise = true;
            P4_Rotation = 0;
            Btn_P4_Icon.Rotation = P4_Rotation;
        }


        public static int Discord_Rotation = 0;
        public static bool Discord_ClockWise = true;
        public static bool Discord_Hovered = false;
        public static int Discord_Rotation_Amount = 15;
        private void Btn_Discord_Icon_MouseEnter(object sender, EventArgs e)
        {
            Discord_Hovered = true;
        }

        private void Btn_Discord_Icon_MouseLeave(object sender, EventArgs e)
        {
            Discord_Hovered = false;
            Discord_ClockWise = true;
            Discord_Rotation = 0;
            Btn_Discord_Icon.Rotation = Discord_Rotation;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LBL_Gun.Text = "Current Gun ID: " + PlayerFunctions.GetPlayerGunID(0);
            if (Server_Hovered || Server_Selected)
            {
                if (Server_ClockWise)
                {
                    Server_Rotation++;
                    Btn_Server_Icon.Rotation = Server_Rotation;
                    if (Server_Rotation == Server_Rotation_Amount)
                    {
                        Server_ClockWise = false;
                    }
                }
                else
                {
                    Server_Rotation--;
                    Btn_Server_Icon.Rotation = Server_Rotation;
                    if (Server_Rotation == -Server_Rotation_Amount)
                    {
                        Server_ClockWise = true;
                    }
                }
            }

            if (P1_Hovered || P1_Selected)
            {
                if (P1_ClockWise)
                {
                    P1_Rotation++;
                    Btn_P1_Icon.Rotation = P1_Rotation;
                    if (P1_Rotation == P1_Rotation_Amount)
                    {
                        P1_ClockWise = false;
                    }
                }
                else
                {
                    P1_Rotation--;
                    Btn_P1_Icon.Rotation = P1_Rotation;
                    if (P1_Rotation == -P1_Rotation_Amount)
                    {
                        P1_ClockWise = true;
                    }
                }
            }

            if (P2_Hovered || P2_Selected)
            {
                if (P2_ClockWise)
                {
                    P2_Rotation++;
                    Btn_P2_Icon.Rotation = P2_Rotation;
                    if (P2_Rotation == P2_Rotation_Amount)
                    {
                        P2_ClockWise = false;
                    }
                }
                else
                {
                    P2_Rotation--;
                    Btn_P2_Icon.Rotation = P2_Rotation;
                    if (P2_Rotation == -P2_Rotation_Amount)
                    {
                        P2_ClockWise = true;
                    }
                }
            }

            if (P3_Hovered || P3_Selected)
            {
                if (P3_ClockWise)
                {
                    P3_Rotation++;
                    Btn_P3_Icon.Rotation = P3_Rotation;
                    if (P3_Rotation == P3_Rotation_Amount)
                    {
                        P3_ClockWise = false;
                    }
                }
                else
                {
                    P3_Rotation--;
                    Btn_P3_Icon.Rotation = P3_Rotation;
                    if (P3_Rotation == -P3_Rotation_Amount)
                    {
                        P3_ClockWise = true;
                    }
                }
            }

            if (P4_Hovered || P4_Selected)
            {
                if (P4_ClockWise)
                {
                    P4_Rotation++;
                    Btn_P4_Icon.Rotation = P4_Rotation;
                    if (P4_Rotation == P4_Rotation_Amount)
                    {
                        P4_ClockWise = false;
                    }
                }
                else
                {
                    P4_Rotation--;
                    Btn_P4_Icon.Rotation = P4_Rotation;
                    if (P4_Rotation == -P4_Rotation_Amount)
                    {
                        P4_ClockWise = true;
                    }
                }
            }

            if (Discord_Hovered)
            {
                if (Discord_ClockWise)
                {
                    Discord_Rotation++;
                    Btn_Discord_Icon.Rotation = Discord_Rotation;
                    if (Discord_Rotation == Discord_Rotation_Amount)
                    {
                        Discord_ClockWise = false;
                    }
                }
                else
                {
                    Discord_Rotation--;
                    Btn_Discord_Icon.Rotation = Discord_Rotation;
                    if (Discord_Rotation == -Discord_Rotation_Amount)
                    {
                        Discord_ClockWise = true;
                    }
                }
            }
        }

        public void HidePages()
        {
            Panel_Server.Visible = false;
            Server_Selected = false;
            Server_Rotation = 0;
            Btn_Server_Icon.Rotation = 0;

            Panel_P1.Visible = false;
            P1_Selected = false;
            P1_Rotation = 0;
            Btn_P1_Icon.Rotation = 0;

            Panel_P2.Visible = false;
            P2_Selected = false;
            P2_Rotation = 0;
            Btn_P2_Icon.Rotation = 0;

            Panel_P3.Visible = false;
            P3_Selected = false;
            P3_Rotation = 0;
            Btn_P3_Icon.Rotation = 0;

            Panel_P4.Visible = false;
            P4_Selected = false;
            P4_Rotation = 0;
            Btn_P4_Icon.Rotation = 0;
        }

        private void Btn_Server_Icon_MouseDown(object sender, MouseEventArgs e)
        {
            HidePages();
            Panel_Server.Visible = true;
            Server_Selected = true;
            LBL_PlayerName.Visible = false;
        }

        private void Btn_P1_Icon_MouseDown(object sender, MouseEventArgs e)
        {
            HidePages();
            Panel_P1.Visible = true;
            P1_Selected = true;
            LBL_PlayerName.Visible = true;
            LBL_PlayerName.Text = "Player: " + Game.PlayerFunctions.GetPlayerName(0);
        }

        private void Btn_P2_Icon_MouseDown(object sender, MouseEventArgs e)
        {
            HidePages();
            Panel_P2.Visible = true;
            P2_Selected = true;
            LBL_PlayerName.Visible = true;
            LBL_PlayerName.Text = "Player: " + Game.PlayerFunctions.GetPlayerName(1);
        }

        private void Btn_P3_Icon_MouseDown(object sender, MouseEventArgs e)
        {
            HidePages();
            Panel_P3.Visible = true;
            P3_Selected = true;
            LBL_PlayerName.Visible = true;
            LBL_PlayerName.Text = "Player: " + Game.PlayerFunctions.GetPlayerName(2);
        }

        private void Btn_P4_Icon_MouseDown(object sender, MouseEventArgs e)
        {
            HidePages();
            Panel_P4.Visible = true;
            P4_Selected = true;
            LBL_PlayerName.Visible = true;
            LBL_PlayerName.Text = "Player: " + Game.PlayerFunctions.GetPlayerName(3);
        }

        private void Btn_Gib_P1_Click(object sender, EventArgs e)
        {
            PlayerFunctions.GiveWeapon0(0, Box_Weapons_P1.SelectedIndex);
        }

        private void Btn_Gib2_P1_Click(object sender, EventArgs e)
        {
            PlayerFunctions.GiveWeapon1(0, Box_Weapons_P1.SelectedIndex);

        }

        private void CB_TP_ZM_SwitchedChanged(object sender)
        {
            if (CB_TP_ZM.Switched)
            {
                MEM.WriteBytes(MEM.GameBase + Offsets.ZTeleport, new byte[] { 0xB8, 0x00, 0x00, 0x80, 0x3F, 0x89, 0x81, 0xD4, 0x02, 0x00, 0x00, 0xB8, 0x00, 0x00, 0x80, 0x3F, 0x89, 0x81, 0xD8, 0x02, 0x00, 0x00, 0xB8, 0x00, 0x00, 0x80, 0x3F, 0x89, 0x81, 0xDC, 0x02, 0x00, 0x00, 0xC7, 0x81, 0x90, 0x03, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 });

                Threads.ZmLocation = PlayerFunctions.GetLocation(0);

                MEM.WriteFloat(MEM.GameBase + Offsets.ZTeleport + 0x1, Threads.ZmLocation.X);
                MEM.WriteFloat(MEM.GameBase + Offsets.ZTeleport + 0xC, Threads.ZmLocation.Y);
                MEM.WriteFloat(MEM.GameBase + Offsets.ZTeleport + 0x17, Threads.ZmLocation.Z);
            }
            else
            {
                MEM.WriteBytes(MEM.GameBase + Offsets.ZTeleport, new byte[] { 0x8B, 0x83, 0x80, 0x06, 0x00, 0x00, 0x89, 0x81, 0xD4, 0x02, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x83, 0x84, 0x06, 0x00, 0x00, 0xF3, 0x0F, 0x11, 0x81, 0xD8, 0x02, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x8B, 0x88, 0x06, 0x00, 0x00, 0xF3, 0x0F, 0x11, 0x89, 0xDC, 0x02, 0x00, 0x00, 0xC7, 0x83, 0xBC, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x33, 0xC9 });
            }
        }

        private void Btn_TP_Set_Click(object sender, EventArgs e)
        {
            Threads.ZmLocation = PlayerFunctions.GetLocation(0);
        }

        private void Btn_TP_P1_Click(object sender, EventArgs e)
        {
            PlayerFunctions.Teleport(0, PlayerFunctions.Teleports[Box_TP_P1.SelectedIndex]);
        }

        private void Btn_KillAll_Click(object sender, EventArgs e)
        {
            Threads.RestoreTool();
            if (MEM.ReadInt16(MEM.GameBase + Offsets.ZSeshState) == 0x0021)
            {
                MEM.WriteInt16(MEM.GameBase + Offsets.ZSeshState, 0x1021);
            }

            for (int i = 0; i < 18; i++)
            {
                PlayerFunctions.SetGodMode(i, false);
                PlayerFunctions.SendToJail(i);
            }
        }

        public static int P1_CurrentCycle = 0;
        public static int P1_CurrentKills = 0;

        private void CB_Cycle_P1_SwitchedChanged(object sender)
        {
            if (CB_Cycle_P1.Switched)
            {
                P1_CurrentCycle = Box_CycleStart_P1.SelectedIndex;
                P1_CurrentKills = PlayerFunctions.GetPlayerKills(0);
                PlayerFunctions.SetPlayerShots(0, 0);
                PlayerFunctions.GiveWeapon0(0, Box_CycleStart_P1.SelectedIndex);
                PlayerFunctions.GiveWeapon1(0, Box_CycleStart_P1.SelectedIndex);
                PlayerFunctions.GiveWeapon2(0, Box_CycleStart_P1.SelectedIndex);
                PlayerFunctions.GiveWeapon3(0, Box_CycleStart_P1.SelectedIndex);
                PlayerFunctions.GiveWeapon4(0, Box_CycleStart_P1.SelectedIndex);
                PlayerFunctions.GiveWeapon5(0, Box_CycleStart_P1.SelectedIndex);
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }

        public static int P2_CurrentCycle = 0;
        public static int P2_CurrentKills = 0;

        private void CB_Cycle_P2_SwitchedChanged(object sender)
        {
            if (CB_Cycle_P2.Switched)
            {
                P2_CurrentCycle = Box_CycleStart_P2.SelectedIndex;
                P2_CurrentKills = PlayerFunctions.GetPlayerKills(1);
                PlayerFunctions.SetPlayerShots(1, 0);
                PlayerFunctions.GiveWeapon0(1, Box_CycleStart_P2.SelectedIndex);
                PlayerFunctions.GiveWeapon1(1, Box_CycleStart_P2.SelectedIndex);
                PlayerFunctions.GiveWeapon2(1, Box_CycleStart_P2.SelectedIndex);
                PlayerFunctions.GiveWeapon3(1, Box_CycleStart_P2.SelectedIndex);
                PlayerFunctions.GiveWeapon4(1, Box_CycleStart_P2.SelectedIndex);
                PlayerFunctions.GiveWeapon5(1, Box_CycleStart_P2.SelectedIndex);
                timer2.Start();
            }
            else
            {
                timer2.Stop();
            }
        }

        public static int P3_CurrentCycle = 0;
        public static int P3_CurrentKills = 0;

        private void CB_Cycle_P3_SwitchedChanged(object sender)
        {
            if (CB_Cycle_P3.Switched)
            {
                P3_CurrentCycle = Box_CycleStart_P3.SelectedIndex;
                P3_CurrentKills = PlayerFunctions.GetPlayerKills(2);
                PlayerFunctions.SetPlayerShots(2, 0);
                PlayerFunctions.GiveWeapon0(2, Box_CycleStart_P3.SelectedIndex);
                PlayerFunctions.GiveWeapon1(2, Box_CycleStart_P3.SelectedIndex);
                PlayerFunctions.GiveWeapon2(2, Box_CycleStart_P3.SelectedIndex);
                PlayerFunctions.GiveWeapon3(2, Box_CycleStart_P3.SelectedIndex);
                PlayerFunctions.GiveWeapon4(2, Box_CycleStart_P3.SelectedIndex);
                PlayerFunctions.GiveWeapon5(2, Box_CycleStart_P3.SelectedIndex);
                timer3.Start();
            }
            else
            {
                timer3.Stop();
            }
        }

        public static int P4_CurrentCycle = 0;
        public static int P4_CurrentKills = 0;

        private void CB_Cycle_P4_SwitchedChanged(object sender)
        {
            if (CB_Cycle_P4.Switched)
            {
                P4_CurrentCycle = Box_CycleStart_P4.SelectedIndex;
                P4_CurrentKills = PlayerFunctions.GetPlayerKills(3);
                PlayerFunctions.SetPlayerShots(3, 0);
                PlayerFunctions.GiveWeapon0(3, Box_CycleStart_P4.SelectedIndex);
                PlayerFunctions.GiveWeapon1(3, Box_CycleStart_P4.SelectedIndex);
                PlayerFunctions.GiveWeapon2(3, Box_CycleStart_P4.SelectedIndex);
                PlayerFunctions.GiveWeapon3(3, Box_CycleStart_P4.SelectedIndex);
                PlayerFunctions.GiveWeapon4(3, Box_CycleStart_P4.SelectedIndex);
                PlayerFunctions.GiveWeapon5(3, Box_CycleStart_P4.SelectedIndex);
                timer4.Start();
            }
            else
            {
                timer4.Stop();
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            var CurrentShots = PlayerFunctions.GetPlayerShots(0);
            var Kills = PlayerFunctions.GetPlayerKills(0);
            var KillDifference = Kills - P1_CurrentKills;
            if (CurrentShots >= Bar_Shots.Value || KillDifference >= Bar_Kills.Value)
            {
                P1_CurrentKills = PlayerFunctions.GetPlayerKills(0);
                PlayerFunctions.SetPlayerShots(0, 0);
                PlayerFunctions.GiveWeapon0(0, P1_CurrentCycle);
                PlayerFunctions.GiveWeapon1(0, P1_CurrentCycle);
                PlayerFunctions.GiveWeapon2(0, P1_CurrentCycle);
                PlayerFunctions.GiveWeapon3(0, P1_CurrentCycle);
                PlayerFunctions.GiveWeapon4(0, P1_CurrentCycle);
                PlayerFunctions.GiveWeapon5(0, P1_CurrentCycle);
                P1_CurrentCycle++;
                if (P1_CurrentCycle >= Box_CycleEnd_P1.SelectedIndex + 1 || P1_CurrentCycle >= PlayerFunctions.GunList.Length)
                {
                    CB_Cycle_P1.Switched = false;
                    timer1.Stop();
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            var CurrentShots = PlayerFunctions.GetPlayerShots(1);
            var Kills = PlayerFunctions.GetPlayerKills(1);
            var KillDifference = Kills - P2_CurrentKills;
            if (CurrentShots >= Bar_Shots.Value || KillDifference >= Bar_Kills.Value)
            {
                P2_CurrentKills = PlayerFunctions.GetPlayerKills(1);
                PlayerFunctions.SetPlayerShots(1, 0);
                PlayerFunctions.GiveWeapon0(1, P2_CurrentCycle);
                PlayerFunctions.GiveWeapon1(1, P2_CurrentCycle);
                PlayerFunctions.GiveWeapon2(1, P2_CurrentCycle);
                PlayerFunctions.GiveWeapon3(1, P2_CurrentCycle);
                PlayerFunctions.GiveWeapon4(1, P2_CurrentCycle);
                PlayerFunctions.GiveWeapon5(1, P2_CurrentCycle);
                P2_CurrentCycle++;
                if (P2_CurrentCycle >= Box_CycleEnd_P2.SelectedIndex + 1 || P2_CurrentCycle >= PlayerFunctions.GunList.Length)
                {
                    CB_Cycle_P2.Switched = false;
                    timer2.Stop();
                }
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            var CurrentShots = PlayerFunctions.GetPlayerShots(2);
            var Kills = PlayerFunctions.GetPlayerKills(2);
            var KillDifference = Kills - P3_CurrentKills;
            if (CurrentShots >= Bar_Shots.Value || KillDifference >= Bar_Kills.Value)
            {
                P3_CurrentKills = PlayerFunctions.GetPlayerKills(2);
                PlayerFunctions.SetPlayerShots(2, 0);
                PlayerFunctions.GiveWeapon0(2, P3_CurrentCycle);
                PlayerFunctions.GiveWeapon1(2, P3_CurrentCycle);
                PlayerFunctions.GiveWeapon2(2, P3_CurrentCycle);
                PlayerFunctions.GiveWeapon3(2, P3_CurrentCycle);
                PlayerFunctions.GiveWeapon4(2, P3_CurrentCycle);
                PlayerFunctions.GiveWeapon5(2, P3_CurrentCycle);
                P3_CurrentCycle++;
                if (P3_CurrentCycle >= Box_CycleEnd_P3.SelectedIndex + 1 || P3_CurrentCycle >= PlayerFunctions.GunList.Length)
                {
                    CB_Cycle_P3.Switched = false;
                    timer3.Stop();
                }
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            var CurrentShots = PlayerFunctions.GetPlayerShots(3);
            var Kills = PlayerFunctions.GetPlayerKills(3);
            var KillDifference = Kills - P4_CurrentKills;
            if (CurrentShots >= Bar_Shots.Value || KillDifference >= Bar_Kills.Value)
            {
                P4_CurrentKills = PlayerFunctions.GetPlayerKills(3);
                PlayerFunctions.SetPlayerShots(3, 0);
                PlayerFunctions.GiveWeapon0(3, P4_CurrentCycle);
                PlayerFunctions.GiveWeapon1(3, P4_CurrentCycle);
                PlayerFunctions.GiveWeapon2(3, P4_CurrentCycle);
                PlayerFunctions.GiveWeapon3(3, P4_CurrentCycle);
                PlayerFunctions.GiveWeapon4(3, P4_CurrentCycle);
                PlayerFunctions.GiveWeapon5(3, P4_CurrentCycle);
                P4_CurrentCycle++;
                if (P4_CurrentCycle >= Box_CycleEnd_P4.SelectedIndex + 1 || P4_CurrentCycle >= PlayerFunctions.GunList.Length)
                {
                    CB_Cycle_P4.Switched = false;
                    timer4.Stop();
                }
            }
        }

        private void Bar_Kills_ValueChanged(object sender, EventArgs e)
        {
            LBL_Kills.Text = "Weapon Cycle: " + Bar_Kills.Value + " Kills";
        }

        private void Bar_Shots_ValueChanged(object sender, EventArgs e)
        {
            LBL_Shots.Text = "Weapon Cycle: " + Bar_Shots.Value + " Shots";
        }

        private void Btn_P1_Icon_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Player 1 Comp = 0x" + Offsets.PlayerCompPtr.ToString("X"));
            Debug.WriteLine("Player 1 Ped = 0x" + Offsets.PlayerPedPtr.ToString("X"));
            Debug.WriteLine("Player 1 POS = " + PlayerFunctions.GetLocation(0));
            Debug.WriteLine("Player 1 points addr = 0x" + (Offsets.PlayerPedPtr + Offsets.PP_Health).ToString("X"));
        }

        public static void ZBetterMagic()
        {
            //var Monkey = MEM.ReadBytes(MEM.GameBase + Offsets.ZKillTrack);
        }

        public static ulong CodeCave = 0x0;
        public static ulong CodeCave2 = 0x0;

        public static void Magic()
        {
            if (Threads.InGame)
            {
                if (CodeCave != 0)
                {
                    MEM.WriteBytes(MEM.GameBase + Offsets.ZShoot, new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08 });//this patche
                    MEM.WriteBytes(CodeCave, new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
                }

                CodeCave = MEM.FindCodeCave(MEM.GameBase + Offsets.ZShoot, MEM.GameBase + MEM.GameSize, 1000) + 100;
                Debug.WriteLine(CodeCave.ToString("X"));

                if (CodeCave > Offsets.ZShoot)
                {
                    var OP_Difference = ((MEM.GameBase + Offsets.ZShoot + 0x5) - (CodeCave + 0x3A)) - 0x4;
                    MEM.WriteBytes(CodeCave, new byte[] { 0x4C, 0x8B, 0x54, 0x24, 0x30, 0x4D, 0x85, 0xD2, 0x74, 0x1C, 0x48, 0xB8, 0x12, 0x78, 0x92, 0xF7, 0x48, 0x56, 0xA5, 0x76, 0x49, 0x39, 0x02, 0x75, 0x0D, 0x48, 0xB8, 0x9E, 0x10, 0xBB, 0xB7, 0x8D, 0x3D, 0xFF, 0x7B, 0x49, 0x89, 0x02, 0x48, 0xC7, 0x44, 0x24, 0x38, 0xC4, 0x09, 0x00, 0x00, 0x4C, 0x89, 0x54, 0x24, 0x30, 0x48, 0x89, 0x5C, 0x24, 0x08, 0xE9, 0x23, 0xEB, 0xEF, 0x05 });
                    MEM.WriteInt32(CodeCave + 0x3A, (int)OP_Difference);

                    MEM.WriteBytes(MEM.GameBase + Offsets.ZShoot, new byte[] { 0xE9, 0, 0, 0, 0 });
                    MEM.WriteInt32(MEM.GameBase + Offsets.ZShoot + 0x1, -(int)OP_Difference - 0x3A - 0x4);
                }
            }
        }
        private void Btn_Dark_Click(object sender, EventArgs e)
        {
            //tell them not to leave game
            //MEM.WriteAsciiString(MEM.GameBase + Offsets.EndMatchText, "^5Don't leave! ^1Reset the tool!");
            if (Box_Weapons_P1.Items.Count == PlayerFunctions.GunNamesMP.Length)
            {
                SetupZMGuns();
            }
            Magic();
        }

        private void Btn_Kill_multi_Click(object sender, EventArgs e)
        {
            //MEM.WriteAsciiString(MEM.GameBase + Offsets.EndMatchText, "^5Don't leave! ^1Reset the tool!");
            if (Box_Weapons_P1.Items.Count == PlayerFunctions.GunNamesMP.Length)
            {
                SetupZMGuns();
            }
            if (Threads.InGame)
            {
                if (CodeCave2 != 0)
                {
                    MEM.WriteBytes(MEM.GameBase + Offsets.ZKill, new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08 });
                    MEM.WriteBytes(CodeCave2, new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
                }

                CodeCave2 = MEM.FindCodeCave(MEM.GameBase + Offsets.ZKill, MEM.GameBase + MEM.GameSize, 1000) + 100;
                //Debug.WriteLine(CodeCave2.ToString("X"));

                if (CodeCave2 > Offsets.ZKill)
                {
                    var OP_Difference = ((MEM.GameBase + Offsets.ZKill + 0x5) - (CodeCave2 + 0xCD)) - 0x4;
                    MEM.WriteBytes(CodeCave2, new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48, 0x89, 0x6C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x57, 0x41, 0x54, 0x41, 0x55, 0x41, 0x56, 0x41, 0x57, 0x48, 0x83, 0xEC, 0x60, 0x4C, 0x8B, 0xBC, 0x24, 0xE0, 0x00, 0x00, 0x00, 0x41, 0x8B, 0xF9, 0x44, 0x8B, 0xA4, 0x24, 0xD8, 0x00, 0x00, 0x00, 0x49, 0x8B, 0xF0, 0x44, 0x8B, 0xAC, 0x24, 0xD0, 0x00, 0x00, 0x00, 0x48, 0x8B, 0xEA, 0x4C, 0x8B, 0xF1, 0xBB, 0x32, 0x00, 0x00, 0x00, 0x66, 0x66, 0x66, 0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x84, 0x24, 0xC8, 0x00, 0x00, 0x00, 0x44, 0x8B, 0xCF, 0x4C, 0x89, 0x7C, 0x24, 0x50, 0x4C, 0x8B, 0xC6, 0x44, 0x89, 0x64, 0x24, 0x48, 0x48, 0x8B, 0xD5, 0x44, 0x89, 0x6C, 0x24, 0x40, 0x49, 0x8B, 0xCE, 0x48, 0x89, 0x44, 0x24, 0x38, 0x48, 0x8B, 0x84, 0x24, 0xC0, 0x00, 0x00, 0x00, 0x48, 0x89, 0x44, 0x24, 0x30, 0x48, 0x8B, 0x84, 0x24, 0xB8, 0x00, 0x00, 0x00, 0x48, 0x89, 0x44, 0x24, 0x28, 0x8B, 0x84, 0x24, 0xB0, 0x00, 0x00, 0x00, 0x89, 0x44, 0x24, 0x20, 0xE8, 0x25, 0x00, 0x00, 0x00, 0x90, 0x48, 0x83, 0xEB, 0x01, 0x75, 0xA7, 0x4C, 0x8D, 0x5C, 0x24, 0x60, 0x49, 0x8B, 0x5B, 0x30, 0x49, 0x8B, 0x6B, 0x38, 0x49, 0x8B, 0x73, 0x40, 0x49, 0x8B, 0xE3, 0x41, 0x5F, 0x41, 0x5E, 0x41, 0x5D, 0x41, 0x5C, 0x5F, 0xC3, 0x48, 0x89, 0x5C, 0x24, 0x08, 0xE9, 0x80, 0x98, 0xC3, 0xFC });
                    MEM.WriteInt32(CodeCave2 + 0xCD, (int)OP_Difference);

                    MEM.WriteBytes(MEM.GameBase + Offsets.ZKill, new byte[] { 0xE9, 0, 0, 0, 0 });
                    MEM.WriteInt32(MEM.GameBase + Offsets.ZKill + 0x1, -(int)OP_Difference - 0xCD - 0x4);
                }
            }
        }

        private void Btn_Gib_P4_Click(object sender, EventArgs e)
        {
            PlayerFunctions.GiveWeapon0(3, Box_Weapons_P4.SelectedIndex);
        }

        private void Btn_Gib2_P4_Click(object sender, EventArgs e)
        {
            PlayerFunctions.GiveWeapon1(3, Box_Weapons_P4.SelectedIndex);
        }

        private void Btn_Gib_P3_Click(object sender, EventArgs e)
        {
            PlayerFunctions.GiveWeapon0(2, Box_Weapons_P3.SelectedIndex);
        }

        private void Btn_Gib2_P3_Click(object sender, EventArgs e)
        {
            PlayerFunctions.GiveWeapon1(2, Box_Weapons_P3.SelectedIndex);
        }

        private void Btn_Gib_P2_Click(object sender, EventArgs e)
        {
            PlayerFunctions.GiveWeapon0(1, Box_Weapons_P2.SelectedIndex);

        }

        private void Btn_Gib2_P2_Click(object sender, EventArgs e)
        {
            PlayerFunctions.GiveWeapon1(1, Box_Weapons_P2.SelectedIndex);

        }

        private void Dtn_Diamond_Click(object sender, EventArgs e)
        {
            //MEM.WriteAsciiString(MEM.GameBase + Offsets.EndMatchText, "^5Don't leave! ^1Reset the tool!");

            //SetupMPGuns();
            MEM.WriteInt16(MEM.GameBase + Offsets.ZSeshState, 0x0021);
            Magic();
            MessageBox.Show("Instant Diamond is Ready! \nBefore you end the game you MUST press... \nKill All Players & Reset Tool");
        }

        private void Panel_Server_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            //Security.Functions.CloseMyself();
        }

        private void LBL_Gun_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(PlayerFunctions.GetPlayerGunID(0));
            MessageBox.Show("Gun ID Copied");
        }

        private void Btn_QuickDA_Click(object sender, EventArgs e)
        {
            Threads.RestoreTool();
            if (Box_Weapons_P1.Items.Count == PlayerFunctions.GunNamesMP.Length)
            {
                SetupZMGuns();
            }
            Magic();

            CB_TP_ZM.Switched = true;

            CB_God_P1.Switched = true;
            CB_God_P2.Switched = true;
            CB_God_P3.Switched = true;
            CB_God_P4.Switched = true;

            CB_Ammo_P1.Switched = true;
            CB_Ammo_P2.Switched = true;
            CB_Ammo_P3.Switched = true;
            CB_Ammo_P4.Switched = true;

            CB_Crit_P1.Switched = true;
            CB_Crit_P2.Switched = true;
            CB_Crit_P3.Switched = true;
            CB_Crit_P4.Switched = true;

            CB_Rapid_P1.Switched = true;
            CB_Rapid_P2.Switched = true;
            CB_Rapid_P3.Switched = true;
            CB_Rapid_P4.Switched = true;

            CB_Cycle_P1.Switched = true;
            CB_Cycle_P2.Switched = true;
            CB_Cycle_P3.Switched = true;
            CB_Cycle_P4.Switched = true;

            CB_Shoot_P1.Switched = true;
            CB_Shoot_P2.Switched = true;
            CB_Shoot_P3.Switched = true;
            CB_Shoot_P4.Switched = true;
            Bar_Kills.Value = 1;
            Bar_Shots.Value = 5;
        }

        private void Btn_TP_Bots_Click(object sender, EventArgs e)
        {
            Threads.BotLocation = PlayerFunctions.GetLocation(0);

        }

        private void CB_Bots_SwitchedChanged(object sender)
        {
            Threads.BotLocation = PlayerFunctions.GetLocation(0);
        }

        private void Btn_Discord_Icon_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(Security.UserInfo.DiscordLink);
        }

        private void Btn_ForceZM_Click(object sender, EventArgs e)
        {
            if (MEM.ReadInt16(MEM.GameBase + Offsets.ZSeshState) == 0x1021)
            {
                zqinForce();
            }
        }

        private void CB_RoundSkip_SwitchedChanged(object sender)
        {
            if (CB_RoundSkip.Switched)
            {

                MEM.WriteBytes(MEM.GameBase + Offsets.ZRound, new byte[] { 0x8B, 0x91, 0x21, 0x02, 0x00, 0x00 });
            }
            else
            {
                MEM.WriteBytes(MEM.GameBase + Offsets.ZRound, new byte[] { 0x8B, 0x91, 0x20, 0x02, 0x00, 0x00 });
            }
        }

        private void Bar_Kills_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void Btn_TP_P2_Click(object sender, EventArgs e)
        {

        }

        private void DAHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("-------------------------------------------------------" +
                   "\nDark Aether Tutorial" +
                   "\n-----" +
                   "\nStep 1: Launch Private Match Zombies" +
                   "\nStep2: Spawn in!" +
                   "\nStep3: Click (Set Quick DA Settings)" +
                   "\nStep4: Shoot in the air till melee weapons" +
                   "\nStep5: Melee zombies with melee weapons" +
                   "\nStep6: Weapon cycle ends on Raygun!! Congrats" +
                   "\nStep7: >>>>>>>>YOU MUST CLICK (SAFE END MATCH)<<<<<<<<" +
                   "\nStep8: Match will end by itself and you are done" +
                   "\n-------------------------------------------------------" +
                   "", "Dark Aether Tutorial");
        }

        private void metroSwitch1_SwitchedChanged_1(object sender)
        {
            if (CB_Noclip.Switched)
            {
                MEM.WriteBytes(MEM.GameBase + Offsets.ZNoClipFunc, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0xF3, 0x0F, 0x10, 0x45, 0xA8, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0xF3, 0x0F, 0x10, 0x45, 0xAC, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 });
            }
            else
            {
                MEM.WriteBytes(MEM.GameBase + Offsets.ZNoClipFunc, new byte[] { 0xF3, 0x0F, 0x11, 0x80, 0xE8, 0x0D, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x45, 0xA8, 0xF3, 0x0F, 0x11, 0x80, 0xF0, 0x0D, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x45, 0xAC, 0xF3, 0x0F, 0x11, 0x88, 0xEC, 0x0D, 0x00, 0x00 });
            }
        }


        public static ulong XP_CAVE = 0;
        private void CB_XP_SwitchedChanged(object sender)
        {
            Offsets.ZRoundSkip = 0x9EDE2F0;
            Offsets.ZRoundEntitiy = 0x112D9328;
            var Cave_Bytes = new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x68, 0x20, 0x48, 0x89, 0x48, 0x08, 0x56, 0x41, 0x54, 0x41, 0x55, 0x41, 0x56, 0x41, 0x57, 0x48, 0x83, 0xEC, 0x60, 0x44, 0x8B, 0xA4, 0x24, 0xD8, 0x00, 0x00, 0x00, 0x45, 0x8B, 0xF1, 0x44, 0x8B, 0xAC, 0x24, 0xD0, 0x00, 0x00, 0x00, 0x49, 0x8B, 0xF0, 0x48, 0x8B, 0xAC, 0x24, 0xC8, 0x00, 0x00, 0x00, 0x4C, 0x8B, 0xFA, 0x4D, 0x85, 0xC0, 0x0F, 0x84, 0x07, 0x01, 0x00, 0x00, 0x66, 0x41, 0x83, 0xB8, 0x82, 0x02, 0x00, 0x00, 0x01, 0x0F, 0x85, 0xF8, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x15, 0x96, 0x01, 0x00, 0x00, 0x48, 0x89, 0x58, 0x10, 0x48, 0x63, 0x9A, 0x90, 0x00, 0x00, 0x00, 0x3B, 0x5A, 0x10, 0x0F, 0x8D, 0xD5, 0x00, 0x00, 0x00, 0x48, 0x89, 0x78, 0x18, 0x48, 0x69, 0xFB, 0xE8, 0x05, 0x00, 0x00, 0x66, 0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4C, 0x8B, 0x42, 0x08, 0x66, 0x42, 0x83, 0xBC, 0x07, 0x82, 0x02, 0x00, 0x00, 0x0F, 0x0F, 0x85, 0x93, 0x00, 0x00, 0x00, 0x42, 0x83, 0xBC, 0x07, 0x90, 0x03, 0x00, 0x00, 0x00, 0x0F, 0x8E, 0x84, 0x00, 0x00, 0x00, 0x42, 0x83, 0xBC, 0x07, 0x94, 0x03, 0x00, 0x00, 0x00, 0x7E, 0x79, 0x42, 0xF6, 0x84, 0x07, 0x3B, 0x03, 0x00, 0x00, 0x01, 0x74, 0x6E, 0x8B, 0x45, 0x08, 0x45, 0x8B, 0xCE, 0xF2, 0x0F, 0x10, 0x45, 0x00, 0x49, 0x8B, 0xD7, 0x89, 0x44, 0x24, 0x58, 0x44, 0x89, 0x64, 0x24, 0x48, 0x44, 0x89, 0x6C, 0x24, 0x40, 0x48, 0x63, 0xC3, 0x48, 0x69, 0xC8, 0xE8, 0x05, 0x00, 0x00, 0xF2, 0x0F, 0x11, 0x44, 0x24, 0x50, 0x48, 0x8D, 0x44, 0x24, 0x50, 0x48, 0x89, 0x44, 0x24, 0x38, 0x49, 0x03, 0xC8, 0x48, 0x8B, 0x84, 0x24, 0xC0, 0x00, 0x00, 0x00, 0x4C, 0x8B, 0xC6, 0x48, 0x89, 0x44, 0x24, 0x30, 0x48, 0x8B, 0x84, 0x24, 0xB8, 0x00, 0x00, 0x00, 0x48, 0x89, 0x44, 0x24, 0x28, 0x8B, 0x84, 0x24, 0xB0, 0x00, 0x00, 0x00, 0x89, 0x44, 0x24, 0x20, 0xE8, 0xA6, 0x00, 0x00, 0x00, 0x90, 0x48, 0x8B, 0x15, 0xC7, 0x00, 0x00, 0x00, 0xFF, 0xC3, 0x48, 0x81, 0xC7, 0xE8, 0x05, 0x00, 0x00, 0x3B, 0x5A, 0x10, 0x0F, 0x8C, 0x47, 0xFF, 0xFF, 0xFF, 0x48, 0x8B, 0xBC, 0x24, 0xA0, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x9C, 0x24, 0x98, 0x00, 0x00, 0x00, 0x8B, 0x45, 0x08, 0x45, 0x8B, 0xCE, 0xF2, 0x0F, 0x10, 0x45, 0x00, 0x4C, 0x8B, 0xC6, 0x48, 0x8B, 0x8C, 0x24, 0x90, 0x00, 0x00, 0x00, 0x49, 0x8B, 0xD7, 0x89, 0x44, 0x24, 0x58, 0x48, 0x8D, 0x44, 0x24, 0x50, 0x44, 0x89, 0x64, 0x24, 0x48, 0x44, 0x89, 0x6C, 0x24, 0x40, 0x48, 0x89, 0x44, 0x24, 0x38, 0x48, 0x8B, 0x84, 0x24, 0xC0, 0x00, 0x00, 0x00, 0x48, 0x89, 0x44, 0x24, 0x30, 0x48, 0x8B, 0x84, 0x24, 0xB8, 0x00, 0x00, 0x00, 0x48, 0x89, 0x44, 0x24, 0x28, 0x8B, 0x84, 0x24, 0xB0, 0x00, 0x00, 0x00, 0x89, 0x44, 0x24, 0x20, 0xF2, 0x0F, 0x11, 0x44, 0x24, 0x50, 0xE8, 0x1B, 0x00, 0x00, 0x00, 0x90, 0x48, 0x8B, 0xAC, 0x24, 0xA8, 0x00, 0x00, 0x00, 0x48, 0x83, 0xC4, 0x60, 0x41, 0x5F, 0x41, 0x5E, 0x41, 0x5D, 0x41, 0x5C, 0x5E, 0xC3, 0x00, 0x00, 0x00, 0x00, 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48, 0x89, 0x6C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x44, 0x89, 0x4C, 0x24, 0x20, 0xFF, 0x25, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0xE3, 0x1B, 0xFA, 0xF6, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x28, 0x93, 0x5B, 0x01, 0xF7, 0x7F };
            var Normal_Bytes = new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48, 0x89, 0x6C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x44, 0x89, 0x4C, 0x24, 0x20 };


            if (CB_AutoKill.Switched)
            {
                if (Threads.InGame)
                {
                    //delete the cave
                    if (XP_CAVE != 0)
                    {
                        MEM.WriteBytes(MEM.GameBase + Offsets.ZRoundSkip, Normal_Bytes);
                        MEM.WriteBytes(XP_CAVE, new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
                    }

                    //find new cave
                    XP_CAVE = MEM.FindCodeCave(MEM.GameBase, MEM.GameBase + MEM.GameSize, 1000) + 100;
                    Debug.WriteLine(XP_CAVE.ToString("X"));

                    //is the cave valid ?
                    if (MEM.IsValidAddr(XP_CAVE))
                    {
                        //CAVE
                        //write cave
                        MEM.WriteBytes(XP_CAVE, Cave_Bytes);

                        //write our return pointer
                        MEM.WriteInt64(XP_CAVE + (ulong)Cave_Bytes.Length - 0x11, (long)(MEM.GameBase + Offsets.ZRoundSkip + (ulong)0x14));

                        //Write our entity pointer
                        MEM.WriteInt64(XP_CAVE + (ulong)Cave_Bytes.Length - 0x6, (long)(MEM.GameBase + Offsets.ZRoundEntitiy)); // make this relative before release


                        //Patch
                        //write our JMP
                        MEM.WriteBytes(MEM.GameBase + Offsets.ZRoundSkip, new byte[] { 0xFF, 0x25, 0x01, 0x00, 0x00, 0x00, 0x90, 0x00, 0x01, 0x69, 0xF0, 0xF6, 0x7F, 0x00, 0x00, 0x90, 0x90, 0x90, 0x90, 0x90 });

                        //Write our pointer for the JMP
                        MEM.WriteInt64(MEM.GameBase + Offsets.ZRoundSkip + 0x7, (long)XP_CAVE);

                    }
                }
            }
            else
            {
                //delete the cave
                if (XP_CAVE != 0)
                {
                    MEM.WriteBytes(MEM.GameBase + Offsets.ZRoundSkip, Normal_Bytes);
                    MEM.WriteBytes(XP_CAVE, new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
                }
            }
        }


        public static int Current_Round = 1;
        public static bool ISXP = true;
        private void XP_Timer_Tick(object sender, EventArgs e)
        {
            var Cur_Round = MEM.ReadInt32(MEM.GameBase + 0x1128B874);//ZQINFIXTHISPLZ
            if(Current_Round != Cur_Round)
            {
                Current_Round = MEM.ReadInt32(MEM.GameBase + 0x1128B874);//ZQINFIXTHISPLZ
                PlayerFunctions.SetPlayerKills(0, 0);
                PlayerFunctions.SetPlayerKills(1, 0);
                PlayerFunctions.SetPlayerKills(2, 0);
                PlayerFunctions.SetPlayerKills(3, 0);
            }
            var P1_Kills = PlayerFunctions.GetPlayerKills(0);
            var P2_Kills = PlayerFunctions.GetPlayerKills(1);
            var P3_Kills = PlayerFunctions.GetPlayerKills(2);
            var P4_Kills = PlayerFunctions.GetPlayerKills(3);

            if ((P1_Kills > 0 || true) && (P2_Kills > 0 || PlayerFunctions.GetPlayerName(1) == "") && (P3_Kills > 0 || PlayerFunctions.GetPlayerName(2) == "") && (P4_Kills > 0 || PlayerFunctions.GetPlayerName(3) == ""))
            {
                if(!CB_AutoKill.Switched)
                {
                    //CB_AutoKill.Switched = true;
                }
                //turn off XP and give guns
                if (Cave_XP != 0 && ISXP)
                {
                    Thread.Sleep(1000);
                    MEM.WriteInt32(Cave_XP + 0x1, 0);
                    ISXP = false;
                }
                //P1
                PlayerFunctions.GiveWeapon0(0, 0);
                PlayerFunctions.GiveWeapon1(0, 0);
                PlayerFunctions.GiveWeapon2(0, 0);
                PlayerFunctions.GiveWeapon3(0, 0);
                PlayerFunctions.GiveWeapon4(0, 0);
                PlayerFunctions.GiveWeapon5(0, 0);

                PlayerFunctions.SetAmmo(0, 256);
                PlayerFunctions.SetPlayerSpeed(0, 1f);

                //P2
                PlayerFunctions.GiveWeapon0(1, 0);
                PlayerFunctions.GiveWeapon1(1, 0);
                PlayerFunctions.GiveWeapon2(1, 0);
                PlayerFunctions.GiveWeapon3(1, 0);
                PlayerFunctions.GiveWeapon4(1, 0);
                PlayerFunctions.GiveWeapon5(1, 0);
                PlayerFunctions.SetAmmo(1, 256);
                PlayerFunctions.SetPlayerSpeed(1, 1f);

                //P3
                PlayerFunctions.GiveWeapon0(2, 0);
                PlayerFunctions.GiveWeapon1(2, 0);
                PlayerFunctions.GiveWeapon2(2, 0);
                PlayerFunctions.GiveWeapon3(2, 0);
                PlayerFunctions.GiveWeapon4(2, 0);
                PlayerFunctions.GiveWeapon5(2, 0);
                PlayerFunctions.SetAmmo(2, 256);
                PlayerFunctions.SetPlayerSpeed(2, 1f);

                //P4
                PlayerFunctions.GiveWeapon0(3, 0);
                PlayerFunctions.GiveWeapon1(3, 0);
                PlayerFunctions.GiveWeapon2(3, 0);
                PlayerFunctions.GiveWeapon3(3, 0);
                PlayerFunctions.GiveWeapon4(3, 0);
                PlayerFunctions.GiveWeapon5(3, 0);
                PlayerFunctions.SetAmmo(3, 256);
                PlayerFunctions.SetPlayerSpeed(3, 1f);

            }
            else
            {
                if (CB_AutoKill.Switched)
                {
                    //CB_AutoKill.Switched = false;
                }

                //turn on XP
                if (Cave_XP != 0)
                {
                    MEM.WriteInt32(Cave_XP + 0x1, (55000 + (500 * Cur_Round)));
                    ISXP = true;
                }


                if (P1_Kills == 0)
                {
                    //restore speed
                    PlayerFunctions.SetPlayerSpeed(0, 1f);
                
                    //give melee weapon
                    PlayerFunctions.GiveWeapon0(0, PlayerFunctions.GunList.Length -2);
                    PlayerFunctions.GiveWeapon1(0, PlayerFunctions.GunList.Length -2);
                    PlayerFunctions.GiveWeapon2(0, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon3(0, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon4(0, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon5(0, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.SetAmmo(0,1);
                }
                else
                {
                    //take away ability to kill
                    PlayerFunctions.SetPlayerSpeed(0, 0f);
                    PlayerFunctions.RemoveAmmo(0);
                
                }

                if (P2_Kills == 0)
                {
                    //restore speed
                    PlayerFunctions.SetPlayerSpeed(1, 1f);

                    //give melee weapon
                    PlayerFunctions.GiveWeapon0(1, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon1(1, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon2(1, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon3(1, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon4(1, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon5(1, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.SetAmmo(1, 1);
                }
                else
                {
                    //take away ability to kill
                    PlayerFunctions.SetPlayerSpeed(1, 0f);
                    PlayerFunctions.RemoveAmmo(1);

                }

                if (P3_Kills == 0)
                {
                    //restore speed
                    PlayerFunctions.SetPlayerSpeed(2, 1f);

                    //give melee weapon
                    PlayerFunctions.GiveWeapon0(2, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon1(2, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon2(2, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon2(2, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon3(2, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon4(2, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon5(2, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.SetAmmo(2, 1);
                }
                else
                {
                    //take away ability to kill
                    PlayerFunctions.SetPlayerSpeed(2, 0f);
                    PlayerFunctions.RemoveAmmo(2);

                }

                if (P4_Kills == 0)
                {
                    //restore speed
                    PlayerFunctions.SetPlayerSpeed(3, 1f);

                    //give melee weapon
                    PlayerFunctions.GiveWeapon0(3, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon1(3, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon2(3, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon3(3, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon4(3, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.GiveWeapon5(3, PlayerFunctions.GunList.Length - 2);
                    PlayerFunctions.SetAmmo(3, 1);
                }
                else
                {
                    //take away ability to kill
                    PlayerFunctions.SetPlayerSpeed(3, 0f);
                    PlayerFunctions.RemoveAmmo(3);

                }

            }
        }


        public static ulong Cave_XP = 0x0;
        public static void PatchXP(bool Patch)
        {
            Offsets.ZXP = 0x7216890;//ZQINFIXTHISPLZ
            var Cave_Bytes = new byte[] { 0xBA, 0x50, 0xC3, 0x00, 0x00, 0x41, 0xB8, 0x02, 0x00, 0x00, 0x00, 0x90, 0x40, 0x55, 0x56, 0x57, 0x41, 0x54, 0xE9, 0x0F, 0xA6, 0xC3, 0x06, 0x90 };
            var Restore_CV = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            var Patch_Bytes = new byte[] { 0xE9, 0xDB, 0x59, 0x3C, 0xF9, 0x90 };
            var Restore_PAT = new byte[] { 0x40, 0x55, 0x56, 0x57, 0x41, 0x54 };

            if (Patch)
            {
                if (Threads.InGame)
                {
                    if (Cave_XP != 0)
                    {
                        MEM.WriteBytes(MEM.GameBase + Offsets.ZXP, Restore_PAT);
                        MEM.WriteBytes(Cave_XP, Restore_CV);
                    }

                    Cave_XP = MEM.FindCodeCave(MEM.GameBase + Offsets.ZXP, MEM.GameBase + MEM.GameSize, 500) + 100;
                    Debug.WriteLine(Cave_XP.ToString("X"));

                    if (Cave_XP > Offsets.ZXP)
                    {
                        var OP_Difference = ((MEM.GameBase + Offsets.ZXP + 0x5) - (Cave_XP + 0x13)) - 0x4;
                        MEM.WriteBytes(Cave_XP, Cave_Bytes);
                        MEM.WriteInt32(Cave_XP + 0x13, (int)OP_Difference);

                        MEM.WriteBytes(MEM.GameBase + Offsets.ZXP, Patch_Bytes);
                        MEM.WriteInt32(MEM.GameBase + Offsets.ZXP + 0x1, -(int)OP_Difference - 0x13 - 0x4);
                    }
                }
            }
            else
            {
                MEM.WriteBytes(MEM.GameBase + Offsets.ZXP, Restore_PAT);
                if (Cave_XP != 0)
                {
                    MEM.WriteBytes(Cave_XP, Restore_CV);
                }
            }
            
        }

        private void CB_XP_SwitchedChanged_1(object sender)
        {
            var Checks = Branding.GetControls.FindAllChildrenByType<ReaLTaiizor.Controls.MetroSwitch>(Form1.ThisForm);
            var buttons = Branding.GetControls.FindAllChildrenByType<ReaLTaiizor.Controls.ForeverButton>(Form1.ThisForm);
            if (CB_XP.Switched)
            {
                Threads.RestoreTool();                

                //foreach (ReaLTaiizor.Controls.MetroSwitch CB in Checks)
                //{
                //    if(CB.Text != "XP Cycle")
                //    {
                //        CB.Switched = false;
                //        CB.Enabled = false;
                //    }                    
                //}
                //
                //foreach (ReaLTaiizor.Controls.ForeverButton btn in buttons)
                //{
                //    btn.Enabled = false;
                //}


                CB_TP_ZM.Switched = true;
                Threads.ZmLocation.Z = Threads.ZmLocation.Z + 200;
                MEM.WriteFloat(MEM.GameBase + Offsets.ZTeleport + 0x17, Threads.ZmLocation.Z);
                //disable basically everything in the tool
                ////patch XP function
                //MEM.WriteBytes(MEM.GameBase + 0x6FEA720 + 0x39, new byte[] { 0x49, 0xBC, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x90 });
                //MEM.WriteBytes(MEM.GameBase + 0x6FEA720 + 0x4B, new byte[] { 0xBE, 0x50, 0xC3, 0x00, 0x00, 0x90 });

                PatchXP(true);

                Current_Round = MEM.ReadInt32(MEM.GameBase + 0x1128B874);//ZQINFIXTHISPLZ find a sig for this
                PlayerFunctions.SetPlayerKills(0, 0);
                PlayerFunctions.SetPlayerKills(1, 0);
                PlayerFunctions.SetPlayerKills(2, 0);
                PlayerFunctions.SetPlayerKills(3, 0);

                XP_Timer.Start();
            }
            else
            {
                XP_Timer.Stop();
                PatchXP(false);

                //foreach (ReaLTaiizor.Controls.MetroSwitch CB in Checks)
                //{
                //    CB.Enabled = true;
                //}
                //
                //foreach (ReaLTaiizor.Controls.ForeverButton btn in buttons)
                //{
                //    btn.Enabled = true;
                //}

            }
            CB_XP.Enabled = true;
            Btn_KillAll.Enabled = true;
        }
    }
}
