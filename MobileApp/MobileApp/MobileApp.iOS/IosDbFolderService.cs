using MobileApp.iOS;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSDbFolderService))]
namespace MobileApp.iOS
{
    class iOSDbFolderService : IDbFolderService
    {
        public string GetDbPath(string fileName)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // папка библиотеки
            var path = Path.Combine(libraryPath, fileName);

            return path;
        }
    }
}