using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using MobileApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XF.Material.Forms.UI.Internals;

[assembly: ExportRenderer(typeof(Entry), typeof(TdsMaterialTextFieldRenderer))]
namespace MobileApp.Droid.Renderers
{
    class TdsMaterialTextFieldRenderer : EntryRenderer
    {
        public TdsMaterialTextFieldRenderer(Context context) : base(context)
        {
        }

        //protected override NumberKeyListener GetDigitsKeyListener(InputTypes inputTypes)
        //{
        //    /*return DigitsKeyListener.GetInstance(Java.Util.Locale.Default,
        //        inputTypes.HasFlag(InputTypes.NumberFlagSigned),
        //        inputTypes.HasFlag(InputTypes.NumberFlagDecimal));*/
        //}
    }
}