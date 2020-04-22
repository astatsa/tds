using System;
using System.Collections.Generic;
using System.Text;

namespace TDSDTO
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    sealed class DisplayFormatAttribute : Attribute
    {
        public string DisplayName { get; }
        public string Format { get; }

        public DisplayFormatAttribute(string displayName, string format = null)
        {
            this.DisplayName = displayName;
            this.Format = format;
        }
    }
}
