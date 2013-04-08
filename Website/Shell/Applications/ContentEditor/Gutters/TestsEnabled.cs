using Sitecore.Analytics.Data.Items;
using Sitecore.Analytics.Testing.TestingUtils;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Layouts;

namespace Sitecore.Shell.Applications.ContentEditor.Gutters
{
    public class TestsEnabled : GutterRenderer
    {
        protected override GutterIconDescriptor GetIconDescriptor(Item item)
        {
            if (!string.IsNullOrEmpty(item[FieldIDs.LayoutField]))
            {
                LayoutDefinition layoutDefinition = LayoutDefinition.Parse(item[FieldIDs.LayoutField]);

                foreach (DeviceItem deviceItem in Context.ContentDatabase.Resources.Devices.GetAll())
                {
                    MultivariateTestDefinitionItem multivariateTestDefinitionItem = null;

                    // Checking for available tests
                    multivariateTestDefinitionItem =
                        TestingUtil.MultiVariateTesting.GetTestDefinition(layoutDefinition, deviceItem.ID, Client.ContentDatabase) ??
                        TestingUtil.MultiVariateTesting.GetTestDefinition(item, deviceItem.ID);

                    if (multivariateTestDefinitionItem == null) return null;

                    // Get the testdefinition item
                    TestDefinitionItem testDefinition = TestDefinitionItem.Create(multivariateTestDefinitionItem);
                    if (testDefinition == null) return null;

                    GutterIconDescriptor iconDescriptor = new GutterIconDescriptor();

                    if (testDefinition.IsDeployed && testDefinition.IsRunning)
                    {
                        iconDescriptor.Icon = "Applications/16x16/bullet_triangle_green.png";
                        iconDescriptor.Tooltip = Translate.Text("Item contains running tests");
                        return iconDescriptor;
                    }

                    if (testDefinition.IsDraft)
                    {
                        iconDescriptor.Icon = "Applications/16x16/bullet_square_red.png";
                        iconDescriptor.Tooltip = Translate.Text("Item contains unpublished test(s) ");
                        return iconDescriptor;
                    }
                }
            }
            return null;
        }
    }
}