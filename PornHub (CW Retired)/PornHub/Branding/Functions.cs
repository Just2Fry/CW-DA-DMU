using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PornHub.Branding
{
    internal static class GetControls
    {
        public static IEnumerable<T> FindAllChildrenByType<T>(this Control control)
        {
            IEnumerable<Control> controls = control.Controls.Cast<Control>();
            return controls
                .OfType<T>()
                .Concat<T>(controls.SelectMany<Control, T>(ctrl => FindAllChildrenByType<T>(ctrl)));
        }
    }
}
