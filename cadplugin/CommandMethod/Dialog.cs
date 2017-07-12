
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Windows;
using CmcuCadPlugin;
using System;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;

[assembly:CommandClass(typeof(cadplugin.DialogClass))]
namespace cadplugin
{
    public class DialogClass
    {
       
        [CommandMethod("ModalDialog")]
        public void ShowModalDialog()
        {
            using (PickPointDialog form = new PickPointDialog())
            {
                form.ShowInTaskbar = false;
                Application.ShowModalDialog(form);
                if (form.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\n" + form.positionText.Text);
                }
            }
        }

        [CommandMethod("ModelessDialog", CommandFlags.UsePickSet)]
        public void ShowModelessDialog()
        {
            PickPointDialog form = new PickPointDialog();
            form.ShowInTaskbar = false;
            Application.ShowModelessDialog(form);
            if (form.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\n" + form.positionText.Text);
            }
        }


    }
}
