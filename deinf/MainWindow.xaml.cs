// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

using Microsoft.UI;           // Needed for WindowId
using Microsoft.UI.Windowing; // Needed for AppWindow
using WinRT.Interop;          // Needed for XAML/HWND interop
using deinf.Pages;
using System.Runtime.InteropServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace deinf
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private AppWindow m_AppWindow;

        public MainWindow()
        {
            this.InitializeComponent();

            // Get the current AppWindow (we can have multiple windows in Windows App SDK).
            m_AppWindow = GetAppWindowForCurrentWindow();

            // Check to see if customization is supported.
            // The method returns true on Windows 10 since Windows App SDK 1.2, and on all versions of
            // Windows App SDK on Windows 11.
            if (AppWindowTitleBar.IsCustomizationSupported())
            {
                // Hide the default title bar.
                var titleBar = m_AppWindow.TitleBar;
                titleBar.ExtendsContentIntoTitleBar = true;
            }
            else
            {
                // If title bar customization is not supported, hide the custom title element.
                AppTitleBar.Visibility = Visibility.Collapsed;
            }

            SystemBackdrop = new MicaBackdrop() { Kind = MicaKind.BaseAlt };
            ContentFrame.Navigate(typeof(Home));
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }

        private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
        {
            // Check to see if customization is supported.
            // The method returns true on Windows 10 since Windows App SDK 1.2, and on all versions of
            // Windows App SDK on Windows 11.
            if (AppWindowTitleBar.IsCustomizationSupported())
            {
                SetDragRegionForCustomTitleBar(m_AppWindow);
            }
        }

        private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Check to see if customization is supported.
            // The method returns true on Windows 10 since Windows App SDK 1.2, and on all versions of
            // Windows App SDK on Windows 11.
            if (AppWindowTitleBar.IsCustomizationSupported()
                && m_AppWindow.TitleBar.ExtendsContentIntoTitleBar)
            {
                // Update drag region if the size of the title bar changes.
                SetDragRegionForCustomTitleBar(m_AppWindow);
            }
        }

        [DllImport("Shcore.dll", SetLastError = true)]
        internal static extern int GetDpiForMonitor(IntPtr hmonitor, Monitor_DPI_Type dpiType, out uint dpiX, out uint dpiY);

        internal enum Monitor_DPI_Type : int
        {
            MDT_Effective_DPI = 0,
            MDT_Angular_DPI = 1,
            MDT_Raw_DPI = 2,
            MDT_Default = MDT_Effective_DPI
        }

        private double GetScaleAdjustment()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            DisplayArea displayArea = DisplayArea.GetFromWindowId(wndId, DisplayAreaFallback.Primary);
            IntPtr hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

            // Get DPI.
            int result = GetDpiForMonitor(hMonitor, Monitor_DPI_Type.MDT_Default, out uint dpiX, out uint _);
            if (result != 0)
            {
                throw new Exception("Could not get DPI for monitor.");
            }

            uint scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
            return scaleFactorPercent / 100.0;
        }

        private void SetDragRegionForCustomTitleBar(AppWindow appWindow)
        {
            // Check to see if customization is supported.
            // The method returns true on Windows 10 since Windows App SDK 1.2, and on all versions of
            // Windows App SDK on Windows 11.
            if (AppWindowTitleBar.IsCustomizationSupported()
                && appWindow.TitleBar.ExtendsContentIntoTitleBar)
            {
                double scaleAdjustment = GetScaleAdjustment();

                RightPaddingColumn.Width = new GridLength(appWindow.TitleBar.RightInset / scaleAdjustment);
                LeftPaddingColumn.Width = new GridLength(appWindow.TitleBar.LeftInset / scaleAdjustment);

                List<Windows.Graphics.RectInt32> dragRectsList = new();

                Windows.Graphics.RectInt32 dragRectL = new()
                {
                    X = (int)((LeftPaddingColumn.ActualWidth) * scaleAdjustment),
                    Y = 0,
                    Height = (int)(AppTitleBar.ActualHeight * scaleAdjustment)
                };
                dragRectsList.Add(dragRectL);

                Windows.Graphics.RectInt32 dragRectR = new()
                {
                    X = (int)((LeftPaddingColumn.ActualWidth
                                    + TitleTextBlock.ActualWidth) * scaleAdjustment),
                    Y = 0,
                    Height = (int)(AppTitleBar.ActualHeight * scaleAdjustment)
                };
                dragRectsList.Add(dragRectR);

                Windows.Graphics.RectInt32[] dragRects = dragRectsList.ToArray();

                appWindow.TitleBar.SetDragRectangles(dragRects);
            }
        }
    }
}
