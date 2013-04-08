using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Layouts;

namespace Sitecore.Shell.Applications.ContentEditor.Gutters
{
    public class PersonalizedPresentation : GutterRenderer
    {
        protected override GutterIconDescriptor GetIconDescriptor(Item item)
        {
            if (!item.Paths.FullPath.StartsWith("/sitecore/content/")) return null;

            GutterIconDescriptor descriptor = new GutterIconDescriptor
                {
                    Icon = "Software/16x16/element_replace.png",
                    Tooltip = Translate.Text("Item contains personalized content.")
                };

            if (!string.IsNullOrEmpty(item[FieldIDs.LayoutField]))
            {
                foreach (DeviceItem device in Context.ContentDatabase.Resources.Devices.GetAll())
                {
                    RenderingReference[] references = item.Visualization.GetRenderings(device, false);
                    IEnumerable<RenderingReference> filteredReferences = references.Where(x => x.Settings.Rules.Count > 0);

                    if (filteredReferences.Select(rendering => rendering.Settings.Rules).Any(rules => rules.Rules.Any(rule => rule.Actions.Count > 0)))
                    {
                        return descriptor;
                    }
                }
            }
            return null;
        }
    }
}