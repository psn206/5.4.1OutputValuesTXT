using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OutputValuesTXT
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            StringBuilder str = new StringBuilder();

            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            List<Wall> walls = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .WhereElementIsNotElementType()
                .Cast<Wall>()
                .ToList();

            foreach (Element wall in walls)
            {
                string nameTypeWall = wall.Name;  //Задание можно понять так wall.get_Parameter(BuiltInParameter.SYMBOL_NAME_PARAM).ToString()=null;
                string volumeWall = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsValueString();
                str.Append(nameTypeWall);
                str.Append("\t");
                str.Append(volumeWall);
                str.Append("\t\n");
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовый файл (*.txt)|*.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, str.ToString());
            }
            TaskDialog.Show("!", $"Информация записана");
            return Result.Succeeded;
        }
    }
}
