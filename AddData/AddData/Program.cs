using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AddData
{
    class Program
    {
        static void Main(string[] args)
        {


            ////创建数据库访问网关
            //using (BimAnalysisDbEntities dbEntities = new BimAnalysisDbEntities())
            //{
            //    //查询到老师对应的班级的外键，注意是使用的linq to ef ,它是生成的命令树，然后是生成的sql

            //    string path = "D:\\workspace\\cad\\docs\\std";

            //    var folder = new DirectoryInfo(path);
            //    foreach (var file in folder.GetFiles("*.dwg"))
            //    {
            //        string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FullName);
            //        string[] fileInfo = fileName.Split('_');


            //        //创建block一个实体
            //        cad_block block = new cad_block();
            //        if (fileInfo[0].Contains("Old"))
            //            block.new_block = false;
            //        else
            //            block.new_block = true;

            //        if (fileInfo.Count() == 3)
            //        {
            //            block.depart_type = "无科室图框";
            //            block.block_major = fileInfo[1];
            //            block.block_size = fileInfo[2];

            //        }
            //        else
            //        {
            //            block.depart_type = "中机建筑";
            //            block.block_major = fileInfo[2];
            //            block.block_size = fileInfo[3];
            //        }
            //        block.block_path = @"D:\cad-plugins\stds\" + System.IO.Path.GetFileName(file.FullName);
            //        block.valid = true;

            //        //将创建的实体，放入网关的数据实体的集合
            //        dbEntities.cad_block.Add(block);
            //    }

                
            //    //写回数据库
            //    dbEntities.SaveChanges();
            //}
            //Console.WriteLine("OK");
        }
    }
}
