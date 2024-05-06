﻿using SharpDX.Direct3D;
using System;
using System.Windows.Forms;

using Device11 = SharpDX.Direct3D11.Device;

namespace Fishing_SharpDX
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!(Device11.GetSupportedFeatureLevel() == FeatureLevel.Level_11_0))
            {
                MessageBox.Show("DirectX11 not Supported");
                return;
            }
            Game game = new Game();
            game.Run();
            game.Dispose();
        }
    }
}
