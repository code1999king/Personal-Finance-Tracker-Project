
using System.Windows.Forms;

namespace Presentation.ExtensionMethods
{
    internal static class ErrorProviderExtensions
    {
        /// <summary>
        /// Checks if there are errors assigned by the current ErrorProvider instance to the controls inside the given parent control
        /// </summary>
        /// <param name="errorProvider"></param>
        /// <param name="parent"></param>
        /// <returns>true if any error found, otherwise false</returns>
        public static bool HasErrors(this ErrorProvider errorProvider, Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (!string.IsNullOrEmpty(errorProvider.GetError(c)))
                    return true;

                // Recursively check child controls
                if (HasErrors(errorProvider, c))
                    return true;
            }
            return false;
        }
    }
}
