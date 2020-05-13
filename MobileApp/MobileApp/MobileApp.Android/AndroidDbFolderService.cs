using MobileApp.Droid;
using System;
using System.IO;
using Xamarin.Forms;

namespace MobileApp.Droid
{
    class AndroidDbFolderService : IDbFolderService
    {
        public string GetDbPath(string fileName)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, fileName);
            return path;
        }
    }
}