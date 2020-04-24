using System;
using System.Collections.Generic;
using System.Text;

namespace TDSDTO
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class DisplayFormatAttribute : Attribute
    {
        public string DisplayName { get; }
        public string Format { get; }
        public double MinWidth { get; }
        public Alignments HorizontalAlignment { get; }

        public DisplayFormatAttribute(string displayName, string format = null, double minWidth = 0, Alignments horizontalAlignment = Alignments.Left)
        {
            this.DisplayName = displayName;
            this.Format = format;
            this.MinWidth = minWidth;
            this.HorizontalAlignment = horizontalAlignment;
        }
    }

    public enum Alignments
    {
        Left,
        Right,
        Center
    }
}
