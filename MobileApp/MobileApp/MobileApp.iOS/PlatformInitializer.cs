using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Prism;
using Prism.Ioc;
using UIKit;

namespace MobileApp.iOS
{
    class PlatformInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IDbFolderService, iOSDbFolderService>();
        }
    }
}