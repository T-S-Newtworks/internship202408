using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TsnEducation2024.Models;

namespace TsnEducation2024.Controllers
{
    public class MyTodoInsertController : Controller
    {
        public ActionResult MyTodoInsert()
        {
            //string filePath = @"C:\temp\TsnEducation2024\todoItem.csv";
            string filePath = AppContext.BaseDirectory + @"\App_Data\todoItem.csv";

            List<MyTodoItem> todoItems = new List<MyTodoItem>();

            if (System.IO.File.Exists(filePath))
            {
                using (var readFile = new StreamReader(filePath, Encoding.GetEncoding("shift-jis")))
                {
                    while (!readFile.EndOfStream)
                    {
                        string[] rowValues = readFile.ReadLine().Split(',');
                        if (rowValues.Length != 2)
                        {
                            continue;
                        }
                        todoItems.Add(new MyTodoItem()
                        {
                            Day = DateTime.Parse(rowValues[0]),
                            Time = DateTime.Parse(rowValues[1]),
                            Title = rowValues[2],
                            Description = rowValues[3],
                            Result = rowValues[4]//後で治す
                        });
                    }
                }
            }
            return View(todoItems);

        }
        public ActionResult SaveTodoItems(List<MyTodoItem> todoItems)
        {
            //string filePath = @"C:\temp\TsnEducation2024\todoItem.csv";
            string filePath = AppContext.BaseDirectory + @"\App_Data\todoItem.csv";
            CreateFile(filePath);
            if (todoItems == null || !todoItems.Any())
            {
                System.IO.File.WriteAllText(filePath, string.Empty);
            }
            else
            {
                using (var overWriteFile = new StreamWriter(filePath, false, Encoding.GetEncoding("shift-jis")))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in todoItems)
                    {
                        sb.AppendLine($"{item.Day},{item.Time},{item.Title},{item.Description},{item.Result}");
                    }

                    overWriteFile.Write(sb.ToString());
                }
            }

            TempData["SuccessMessage"] = "Todoリストを保存しました。";

            return RedirectToAction("Index");
        }


        /// <param name="filePath">作成先のパス</param>
        public void CreateFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                return;
            }

            string folderPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var file = System.IO.File.Create(filePath);
            file.Close();

        }
    }
}