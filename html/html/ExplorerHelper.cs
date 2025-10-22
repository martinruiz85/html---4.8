using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using SHDocVw; // Agregar referencia a COM: Microsoft Internet Controls

namespace UtilETWeb
{

    public static class ExplorerHelper
    {
        // Importamos SetForegroundWindow para traer la ventana al frente
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void FocusIfExplorerOpen(string folderPath) {

            if (!FocusIfExplorerOpenValid(folderPath))
            {
                // Si no existe, la abrimos
                Process.Start("explorer.exe", folderPath);
            }
        }

        private static bool FocusIfExplorerOpenValid(string folderPath)
        {            

            

            string normalizedPath = folderPath.TrimEnd('\\').ToLower();

            ShellWindows shellWindows = new ShellWindows();

            foreach (InternetExplorer window in shellWindows)
            {
                try
                {
                    // Verificamos que sea una ventana de Explorer
                    string filename = System.IO.Path.GetFileNameWithoutExtension(window.FullName).ToLower();
                    if (filename != "explorer") continue;

                    // Ruta actual de la ventana
                    //string currentLocation = window.LocationURL
                    //    .Replace("file:///", "")        // Quita prefijo file:///
                    //    .Replace("/", "\\")             // Normaliza separadores
                    //    .TrimEnd('\\')
                    //    .ToLower();

                    // Convertir LocationURL -> ruta local normalizada
                    Uri uri = new Uri(window.LocationURL);
                    string currentLocation = Uri.UnescapeDataString(uri.LocalPath)
                                                .TrimEnd('\\')
                                                .ToLower();



                    if (currentLocation == normalizedPath)
                    {
                        // Traer la ventana al frente
                        IntPtr hWnd = (IntPtr)window.HWND;
                        SetForegroundWindow(hWnd);

                        // Forzar actualización de la vista
                        window.Refresh();

                        return true; // Sí estaba abierta y la enfocamos
                    }
                }
                catch
                {
                    // Algunas ventanas pueden no ser accesibles
                }
            }

            return false; // No estaba abierta
        }


    }
}
