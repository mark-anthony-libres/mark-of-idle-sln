using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace setup_v1
{
    public class ResourceExtractor
    {
        public async Task CopyResourceAsync(Assembly assembly, string resourceName, string targetFolder, RichTextBox logBox)
        {
            try
            {
                using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (resourceStream == null)
                    {
                        MessageBox.Show($"Resource stream not found: {resourceName}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string assemblyName = assembly.GetName().Name;
                    string validStructureResource = $"{assemblyName}.resources.";
                    string filename = Path.GetFileName(resourceName);

                    if (!filename.StartsWith(validStructureResource))
                    {
                        return;
                    }

                    string retrieveFileName = filename.Replace(validStructureResource, "");
                    logBox.AppendText($"\nExtracting: {retrieveFileName}");

                    string targetFilePath = Path.Combine(targetFolder, retrieveFileName);

                    using (FileStream fileStream = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write))
                    {
                        await resourceStream.CopyToAsync(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying resource: {resourceName}\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
