using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;



[assembly: ExtensionApplication(typeof(cadplugin.PluginApp))]
namespace cadplugin
{
    public class PluginApp : IExtensionApplication
    {

     
        public void Initialize()
        {
            Document dwg = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor aced = dwg.Editor;
            aced.WriteMessage("\n 开始初始化AutoCAD CMCU插件:");

          
        }




        public void Terminate()
        {
        }
    }
}
