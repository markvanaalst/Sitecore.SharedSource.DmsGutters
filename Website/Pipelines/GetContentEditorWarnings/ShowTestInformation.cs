using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Analytics.Data.Items;
using Sitecore.Analytics.Testing.Statistics;
using Sitecore.Analytics.Testing.TestingUtils;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Pipelines.GetContentEditorWarnings;

namespace Sitecore.Pipelines.GetContentEditorWarnings
{
    public class ShowTestInformation
    {
        public void Process(GetContentEditorWarningsArgs args)
        {
            Item item = args.Item;

            if (string.IsNullOrEmpty(item[Sitecore.FieldIDs.LayoutField])) return;

            Sitecore.Layouts.LayoutDefinition layoutDefinition = Sitecore.Layouts.LayoutDefinition.Parse(item[Sitecore.FieldIDs.LayoutField]);

            foreach (DeviceItem deviceItem in Sitecore.Context.ContentDatabase.Resources.Devices.GetAll())
            {
                MultivariateTestDefinitionItem multivariateTestDefinitionItem = null;
                multivariateTestDefinitionItem = TestingUtil.MultiVariateTesting.GetTestDefinition(layoutDefinition, deviceItem.ID, Client.ContentDatabase) ??
                                                 TestingUtil.MultiVariateTesting.GetTestDefinition(item, deviceItem.ID);

                if (multivariateTestDefinitionItem != null)
                {
                    TestDefinitionItem testDefinition = TestDefinitionItem.Create(multivariateTestDefinitionItem);
                    if (testDefinition == null) continue;

                    GetContentEditorWarningsArgs.ContentEditorWarning warning = args.Add();
                    warning.Title = Translate.Text("This item contains one of more tests");

                    if (testDefinition.IsDeployed && testDefinition.IsRunning)
                        warning.Text = Translate.Text("This item contains one of more tests which are running.");
                    
                    if (testDefinition.IsDraft)
                        warning.Text = Translate.Text("This item contains one of more tests which are not published yet.");
                    
                    warning.IsExclusive = false;
                }
            }
        }
    }
}
