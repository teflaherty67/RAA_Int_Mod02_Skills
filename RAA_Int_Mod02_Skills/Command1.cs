#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

#endregion

namespace RAA_Int_Mod02_Skills
{
    [Transaction(TransactionMode.Manual)]
    public class Command1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document curDoc = uiapp.ActiveUIDocument.Document;

            // 1a. Filtered Element Collector by view

            // set the current view as the active view
            View curView = curDoc.ActiveView;

            // create collector for active view
            FilteredElementCollector collector = new FilteredElementCollector(curDoc, curView.Id);

            // 1b. ElementMultiCategoryFilter

            // create a list of categories
            List<BuiltInCategory> catList = new List<BuiltInCategory>();
            catList.Add(BuiltInCategory.OST_Doors);
            catList.Add(BuiltInCategory.OST_Rooms);

            // create the filter
            ElementMulticategoryFilter catFilter = new ElementMulticategoryFilter(catList);

            // apply filter to collector
            collector.WherePasses(catFilter)
                .WhereElementIsNotElementType();

            // 1c. use LINQ to get the family symbol by name
            FamilySymbol tagDoor = new FilteredElementCollector(curDoc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .Where(x => x.FamilyName.Equals("Door Tag"))
                .First();

            FamilySymbol tagRoom = new FilteredElementCollector(curDoc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .Where(x => x.FamilyName.Equals("Room Tag"))
                .First();

            // 2. create dictionary for tags
            Dictionary<string, FamilySymbol> d_Tags = new Dictionary<string, FamilySymbol>();

            d_Tags.Add("Doors", tagDoor);
            d_Tags.Add("Rooms", tagRoom);

            FamilySymbol curDrTag = d_Tags["Doors"];

            foreach (Element cureElem in collector)
            {
                // 3. get point from location
                XYZ insPoint;
                LocationCurve locCurve;
                LocationPoint locPoint;

            }

            return Result.Succeeded;
        }
        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnCommand1";
            string buttonTitle = "Button 1";

            ButtonDataClass myButtonData1 = new ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Blue_32,
                Properties.Resources.Blue_16,
                "This is a tooltip for Button 1");

            return myButtonData1.Data;
        }
    }
}
