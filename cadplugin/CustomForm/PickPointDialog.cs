using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace cadplugin
{
    public partial class PickPointDialog : Form
    {


        [DllImport("user32.dll", EntryPoint = "SetFocus")]
        public static extern int SetFocus(IntPtr hWnd);
        private bool flag = true;
        public PickPointDialog()
        {
            InitializeComponent();
        }

        private void pickPointButton_Click(object sender, EventArgs e)
        {
            SetFocus(Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Window.Handle);
            flag = true;
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            while (flag)
            {
                Point3d pt = GetPoint("\n 选择点");
                string ptData = "(" + pt.X.ToString() + "," + pt.Y.ToString() + "," + pt.Z.ToString() + ")";
                ed.WriteMessage(ptData);
                this.positionText.Text = ptData;
             }
        }

        private Point3d GetPoint(string word)
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            PromptPointResult pt = ed.GetPoint(word);
            
            if (pt.Status == PromptStatus.OK)
            {
                return (Point3d)pt.Value;
            }
            else { return new Point3d(); }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cancleButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void stopPickButton_Click(object sender, EventArgs e)
        {
            flag = false;
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            SetFocus(Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Window.Handle);
            
        }
    }
}
