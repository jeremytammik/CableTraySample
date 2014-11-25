using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.ApplicationServices;
using System.Diagnostics;

namespace CableTraySample
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CmdCableTray : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                UIApplication app = commandData.Application;
                UIDocument uidoc = app.ActiveUIDocument;
                Document doc = uidoc.Document;

                //ElementId idType = new ElementId(411325);
                //ElementId idLevel = new ElementId(311);

                ElementId idType = Util.FindCableTrayTypeId(doc, "Default");

                ElementId idLevel = Util.FindLevelId(doc, "Level 1");

                Transaction transaction = new Transaction(doc);
                transaction.Start("test");
                XYZ start1 = new XYZ(-30.498257567, 38.420015690, 10.058014598);
                XYZ end1 = new XYZ(-20.435555001, 30.837225417, 10.058014598);
                CableTray tray1 = CableTray.Create(doc, idType, start1, end1, idLevel);

                XYZ start2 = new XYZ(-20.435555001, 30.837225417, 10.058014598);
                XYZ end2 = new XYZ(-20.435555001, 30.837225417, 13.338854493);
                CableTray tray2 = CableTray.Create(doc, idType, start2, end2, idLevel);

                XYZ start3 = new XYZ(-20.435555001, 30.837225417, 13.338854493);
                XYZ end3 = new XYZ(-11.525321413, 24.122882809, 13.338854493);
                CableTray tray3 = CableTray.Create(doc, idType, start3, end3, idLevel);

                XYZ start4 = new XYZ(-11.525321413, 24.122882809, 13.338854493);
                XYZ end4 = new XYZ(-11.525321413, 24.122882809, 10.058014598);
                CableTray tray4 = CableTray.Create(doc, idType, start4, end4, idLevel);

                XYZ start5 = new XYZ(-11.525321413, 24.122882809, 10.058014598);
                XYZ end5 = new XYZ(-2.001326892, 16.946038164, 10.058014598);
                CableTray tray5 = CableTray.Create(doc, idType, start5, end5, idLevel);

                Connector c1start, c1end = null;

                foreach (Connector c in tray1.ConnectorManager.Connectors)
                {
                    if (c.Origin.IsAlmostEqualTo(start1))
                    {
                        c1start = c;
                    }
                    else if (c.Origin.IsAlmostEqualTo(end1))
                    {
                        c1end = c;
                    }
                }

                Connector c2start = null, c2end;

                foreach (Connector c in tray2.ConnectorManager.Connectors)
                {
                    if (c.Origin.IsAlmostEqualTo(start2))
                    {
                        c2start = c;
                    }
                    else if (c.Origin.IsAlmostEqualTo(end2))
                    {
                        c2end = c;
                    }
                }

                if (null != c1end && null != c2start)
                {
                    c1end.ConnectTo(c2start);

                    // this throws
                    // Autodesk.Revit.Exceptions.InvalidOperationException: 
                    // "failed to insert elbow":

                    doc.Create.NewElbowFitting(c1end, c2start);
                }
                transaction.Commit();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }

    public class Util
    {
        static ElementId GetNamedElementId(
          FilteredElementCollector a,
          string name)
        {
            IList<Element> elements = a
              .Where(x => x.Name.Equals(name))
              .Cast<Element>()
              .ToList();

            return 0 < elements.Count
              ? elements[0].Id
              : ElementId.InvalidElementId;
        }

        public static ElementId FindLevelId(Document doc, string name)
        {
            FilteredElementCollector a
              = new FilteredElementCollector(doc)
                .OfClass(typeof(Level));

            return GetNamedElementId(a, name);
        }

        public static ElementId FindCableTrayTypeId(Document doc, string name)
        {
            FilteredElementCollector a
              = new FilteredElementCollector(doc)
                .OfClass(typeof(CableTrayType));

            return GetNamedElementId(a, name);
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CmdConduit : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                UIApplication app = commandData.Application;
                UIDocument uidoc = app.ActiveUIDocument;
                Document doc = uidoc.Document;

                Transaction transaction = new Transaction(doc);
                transaction.Start("test");

                ElementId idType = new FilteredElementCollector(doc)
                  .OfClass(typeof(ConduitType))
                  .FirstElementId();

                ElementId idLevel = Util.FindLevelId(doc, "Level 1");

                XYZ start1 = new XYZ(10, 10, 10);
                XYZ end1 = new XYZ(20, 10, 10);
                Conduit c1 = Conduit.Create(doc, idType, start1, end1, idLevel);

                XYZ start2 = new XYZ(30, 20, 10);
                XYZ end2 = new XYZ(30, 30, 10);
                Conduit c2 = Conduit.Create(doc, idType, start2, end2, idLevel);

                Connector c1start, c1end = null;

                foreach (Connector c in c1.ConnectorManager.Connectors)
                {
                    if (c.Origin.IsAlmostEqualTo(start1))
                    {
                        c1start = c;
                    }
                    else if (c.Origin.IsAlmostEqualTo(end1))
                    {
                        c1end = c;
                    }
                }

                Connector c2start = null, c2end;

                foreach (Connector c in c2.ConnectorManager.Connectors)
                {
                    if (c.Origin.IsAlmostEqualTo(start2))
                    {
                        c2start = c;
                    }
                    else if (c.Origin.IsAlmostEqualTo(end2))
                    {
                        c2end = c;
                    }
                }

                if (null != c1end && null != c2start)
                {
                    c1end.ConnectTo(c2start);

                    doc.Create.NewElbowFitting(c1end, c2start);
                }
                transaction.Commit();

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CmdCableTray2 : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                UIApplication app = commandData.Application;
                UIDocument uidoc = app.ActiveUIDocument;
                Document doc = uidoc.Document;

                Transaction transaction = new Transaction(doc);
                transaction.Start("test");

                ElementId idType = Util.FindCableTrayTypeId(doc, "Default");

                ElementId idLevel = Util.FindLevelId(doc, "Level 1");

                XYZ start1 = new XYZ(10, 10, 10);
                XYZ end1 = new XYZ(20, 10, 10);
                CableTray c1 = CableTray.Create(doc, idType, start1, end1, idLevel);

                XYZ start2 = new XYZ(30, 20, 10);
                XYZ end2 = new XYZ(30, 30, 10);
                CableTray c2 = CableTray.Create(doc, idType, start2, end2, idLevel);

                Connector c1start, c1end = null;

                foreach (Connector c in c1.ConnectorManager.Connectors)
                {
                    if (c.Origin.IsAlmostEqualTo(start1))
                    {
                        c1start = c;
                    }
                    else if (c.Origin.IsAlmostEqualTo(end1))
                    {
                        c1end = c;
                    }
                }

                Connector c2start = null, c2end;

                foreach (Connector c in c2.ConnectorManager.Connectors)
                {
                    if (c.Origin.IsAlmostEqualTo(start2))
                    {
                        c2start = c;
                    }
                    else if (c.Origin.IsAlmostEqualTo(end2))
                    {
                        c2end = c;
                    }
                }

                if (null != c1end && null != c2start)
                {
                    Transform t1 = c1end.CoordinateSystem;
                    Transform t2 = c2start.CoordinateSystem;

                    c1end.ConnectTo(c2start);

                    doc.Create.NewElbowFitting(c1end, c2start);
                }
                transaction.Commit();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CmdCableTray3 : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                UIApplication app = commandData.Application;
                UIDocument uidoc = app.ActiveUIDocument;
                Document doc = uidoc.Document;

                Transaction transaction = new Transaction(doc);
                transaction.Start("test");

                ElementId idType = Util.FindCableTrayTypeId(doc, "default");
                    //"Ladder Cable Tray");

                ElementId idLevel = Util.FindLevelId(doc, "Level 1");

                XYZ start1 = new XYZ(10, 10, 10);
                XYZ end1 = new XYZ(20, 10, 10);
                CableTray c1 = CableTray.Create(doc, idType, start1, end1, idLevel);

                XYZ start2 = new XYZ(30, 10, 10);
                XYZ end2 = new XYZ(30, 10, 30);
                CableTray c2 = CableTray.Create(doc, idType, start2, end2, idLevel);

                Line axis = app.Application.Create.NewLineUnbound(start2, new XYZ(0, 0, 1));
                c2.Location.Rotate(axis, Math.PI / 2);

                Connector c1start, c1end = null;

                foreach (Connector c in c1.ConnectorManager.Connectors)
                {
                    if (c.Origin.IsAlmostEqualTo(start1))
                    {
                        c1start = c;
                    }
                    else if (c.Origin.IsAlmostEqualTo(end1))
                    {
                        c1end = c;
                    }
                }

                Connector c2start = null, c2end;

                foreach (Connector c in c2.ConnectorManager.Connectors)
                {
                    if (c.Origin.IsAlmostEqualTo(start2))
                    {
                        c2start = c;
                    }
                    else if (c.Origin.IsAlmostEqualTo(end2))
                    {
                        c2end = c;
                    }
                }

                if (null != c1end && null != c2start)
                {
                    Transform t1 = c1end.CoordinateSystem;
                    Transform t2 = c2start.CoordinateSystem;

                    c1end.ConnectTo(c2start);

                    doc.Create.NewElbowFitting(c1end, c2start);
                }
                transaction.Commit();

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CmdCableTray4 : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Application app = uiapp.Application;
                Document doc = uidoc.Document;

                Transaction transaction = new Transaction(doc);
                transaction.Start("test");

                //ElementId idType = new ElementId(411325);
                //ElementId idLevel = new ElementId(311);
                ElementId idType = Util.FindCableTrayTypeId(doc, "Default");

                ElementId idLevel = Util.FindLevelId(doc, "Level 1");

                XYZ start1 = new XYZ(-30.498257567, 38.420015690, 10.058014598);
                XYZ end1 = new XYZ(-20.435555001, 30.837225417, 10.058014598);
                CableTray tray1 = CableTray.Create(doc, idType, start1, end1, idLevel);

                XYZ start2 = new XYZ(-20.435555001, 30.837225417, 10.058014598);
                XYZ end2 = new XYZ(-20.435555001, 30.837225417, 13.338854493);
                CableTray tray2 = CableTray.Create(doc, idType, start2, end2, idLevel);

                Connector c1start, c1end = null;

                foreach (Connector c in tray1.ConnectorManager.Connectors)
                {
                    if (c.Origin.IsAlmostEqualTo(start1))
                    {
                        c1start = c;
                    }
                    else if (c.Origin.IsAlmostEqualTo(end1))
                    {
                        c1end = c;
                    }
                }

                Connector c2start = null, c2end;

                foreach (Connector c in tray2.ConnectorManager.Connectors)
                {
                    if (c.Origin.IsAlmostEqualTo(start2))
                    {
                        c2start = c;
                    }
                    else if (c.Origin.IsAlmostEqualTo(end2))
                    {
                        c2end = c;
                    }
                }

                if (null != c1end && null != c2start)
                {
                    Transform t1 = c1end.CoordinateSystem;
                    Transform t2 = c2start.CoordinateSystem;

#if DEBUG
                    LocationCurve lc = tray1.Location as LocationCurve;
                    Line line = lc.Curve as Line;
                    XYZ direction1 = line.get_EndPoint(1) - line.get_EndPoint(0);

                    Debug.Assert(direction1.Normalize().IsAlmostEqualTo(t1.BasisZ),
                      "expected connector Z direction to point straight out of cable tray");
#endif // DEBUG

                    // rotate tray2 so that it matches direction of tray1:

#if DEBUG
                    lc = tray2.Location as LocationCurve;
                    line = lc.Curve as Line;
                    XYZ direction2 = line.get_EndPoint(1) - line.get_EndPoint(0);

                    Debug.Assert(direction2.Normalize().IsAlmostEqualTo(-t2.BasisZ),
                      "expected connector Z direction to point straight out of cable tray");

                    Debug.Assert(direction2.Normalize().IsAlmostEqualTo(XYZ.BasisZ),
                      "expected cable tray 2 to be pointing straight up");
#endif // DEBUG

                    // this produces a twisted fitting swapping
                    // the cable tray x and y directions:
                    //
                    //double angle = t2.BasisX.AngleOnPlaneTo( t1.BasisZ, XYZ.BasisZ );

                    double angle = t2.BasisY.AngleOnPlaneTo(t1.BasisZ, XYZ.BasisZ);

                    // these both throw
                    // "Unable to modify the curve.":
                    //doc.Rotate( tray2, line, angle );
                    //tray2.Location.Rotate( line, angle );

                    Line axis = app.Create.NewLineUnbound(start2, XYZ.BasisZ);
                    tray2.Location.Rotate(axis, angle);

                    c1end.ConnectTo(c2start);

                    doc.Create.NewElbowFitting(c1end, c2start);
                }
                transaction.Commit();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}
