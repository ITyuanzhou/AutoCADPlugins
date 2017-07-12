using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(cadplugin.CommandMethod.OutInClass))]
namespace cadplugin.CommandMethod
{
    class OutInClass
    {

        [CommandMethod("ShowWeb")]
        public void ShowWeb()
        {
            string url = "http://www.baidu.com";
            System.Diagnostics.Process.Start("explorer.exe", url);
        }


        [CommandMethod("PickPoint")]
        public void PickPoint()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Point3d pt = Pick("\n 拾取点");
            ed.WriteMessage("\n 拾取的点坐标为：（{0},{1},{2}）", pt.X, pt.Y, pt.Z);
        }

        [CommandMethod("SelectEntity")]
        public void SelectEntity()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Entity ent = Select("\n 选择对象");
            ed.WriteMessage("\n 你选择的对象ObjectId为:", ent.ObjectId.ToString());
        }

        [CommandMethod("Selection")]
        public void Selection()
        {
            FilterType LineType = FilterType.Line;
            FilterType TextType = FilterType.DBText;
            FilterType CircleType = FilterType.Circle;

            FilterType[] Types = new FilterType[3];
            Types[0] = LineType;
            Types[1] = TextType;
            Types[2] = CircleType;

            DBObjectCollection EntityCollection = GetSelection(Types);

        }

        private Point3d Pick(string word)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptPointResult pt = ed.GetPoint(word);
            if (pt.Status == PromptStatus.OK)
            {
                return (Point3d)pt.Value;
            }
            else { return new Point3d(); }
        }

        private Entity Select(string word)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Entity entity = null;
            PromptEntityResult ent = ed.GetEntity(word);
            if (ent.Status == PromptStatus.OK)
            {
                using (Transaction transaction = db.TransactionManager.StartTransaction())
                {
                    entity = (Entity)transaction.GetObject(ent.ObjectId, OpenMode.ForWrite, true);
                    transaction.Commit();
                }
            }
            return entity;
        }

        public enum FilterType
        {
            Curve, Dimension, Polyline, BlockRef, Circle, Line, Arc, DBText, MText, Polyline3d, Surface, Region, Solid3d, Hatch, Helix, DBPoint
        }

        private DBObjectCollection GetSelection(FilterType[] tps)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Entity entity = null;
            DBObjectCollection entityCollection = new DBObjectCollection();
            PromptSelectionOptions selops = new PromptSelectionOptions();

            //建立选择的过滤器内容
            TypedValue[] filList = new TypedValue[tps.Length + 2];
            filList[0] = new TypedValue((int)DxfCode.Operator, "<or");
            filList[tps.Length + 1] = new TypedValue((int)DxfCode.Operator, ">or");
            for (int i = 0; i < tps.Length; i++)
            {
                filList[i + 1] = new TypedValue((int)DxfCode.Start, tps[i].ToString());
            }

            //建立过滤器
            SelectionFilter filter = new SelectionFilter(filList);
            PromptSelectionResult ents = ed.GetSelection(selops, filter);
            if (ents.Status == PromptStatus.OK)
            {
                using (Transaction transaction = db.TransactionManager.StartTransaction())
                {
                    SelectionSet SS = ents.Value;
                    foreach (ObjectId id in SS.GetObjectIds())
                    {
                        entity = (Entity)transaction.GetObject(id, OpenMode.ForWrite, true);
                        if (entity != null)
                            entityCollection.Add(entity);
                    }
                    transaction.Commit();
                }
            }
            return entityCollection;

        }
    }
}
