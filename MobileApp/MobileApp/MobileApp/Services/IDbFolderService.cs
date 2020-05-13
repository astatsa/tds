using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp
{
    public interface IDbFolderService
    {
        string GetDbPath(string fileName);
    }
}
