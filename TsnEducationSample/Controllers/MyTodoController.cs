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
    public class MyTodoController : Controller
    {
        public ActionResult MyTodoIndex()
        {
            string filePath = @"C:\temp\TsnEducation2024\todoItem.csv";

            List<MyTodoItem> todoItems = new List<MyTodoItem>();

            if (System.IO.File.Exists(filePath))
            {
                using (var readFile = new StreamReader(filePath, Encoding.GetEncoding("shift-jis")))
                {
                    while (!readFile.EndOfStream)
                    {
                        string[] rowValues = readFile.ReadLine().Split(',');
                        if (rowValues.Length <= 2)
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
        
        public ActionResult MyTodoInsert()
        {
            var todoItems = new List<MyTodoInsertItem>(); // データの取得または生成
            return View(todoItems);
        }
        public ActionResult SaveTodoItems(List<MyTodoItem> todoItems)
        {
            string filePath = @"C:\temp\TsnEducation2024\todoItem.csv";
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

        public ActionResult tuikaTodoItems(List<MyTodoInsertItem> todoItems)
        {
            if (ModelState.IsValid)
            {
                if (todoItems == null || !todoItems.Any())
                {
                    // モデルが空の場合の処理
                    ModelState.AddModelError("", "データが送信されていません。");
                    todoItems = new List<MyTodoInsertItem>();
                    return View("MyTodoInsert", todoItems);
                }

                string filePath = @"C:\temp\TsnEducation2024\todoItem.csv";

                using (var writer = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    foreach (var item in todoItems)
                    {
                        // 各アイテムをCSV形式で書き込む
                        writer.WriteLine($"{item.Day:yyyy-MM-dd},{item.Time:HH:mm},{item.Title},{item.Description},{item.Repeat}");
                    }
                }

                TempData["SuccessMessage"] = "TODOリストが追加されました。";
                return RedirectToAction("MyTodoInsert");
            }

            // モデルが無効な場合、バリデーションエラーのメッセージを表示
            return View("MyTodoInsert", todoItems);
        }

 
        public  ActionResult list()
        {
            // デフォルトで表示するページを「一覧」に設定
            var defaultlist = "list";
            return RedirectToAction("List", new { day = defaultlist });
        }

        public ActionResult week(string day)
        {
            ViewBag.SelectedDay = day;
            // 必要なデータをビューに渡して「Day」ビューを返す
            return View("day");
        }

        public ActionResult weeklist(string day)
        {
            ViewBag.SelectedDay = day;

            return View("List");
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

        public ActionResult Day(string day)
        {
            var selectedTodoItems = GetTodoItemsForDay(day); // `GetTodoItemsForDay` メソッドは仮定
            ViewBag.SelectedDay = day;
            return View(selectedTodoItems); // Day.cshtml が適切なモデルを受け取ることを確認
        }

        private List<MyTodoItem> GetTodoItemsForDay(string day)
        {
            // day に基づいてリストをフィルタリングするロジックを追加
            return new List<MyTodoItem>(); // 例として空のリストを返す
        }

        public ActionResult Index()
        {
            var model = new MyViewModel { IsChecked = true }; // デフォルトでチェックボックスがオンになる例
            return View(model);
        }

        public ActionResult Index(MyViewModel model)
        {
            if (model.IsChecked)
            {
                // チェックボックスがオンの場合の処理
            }
            else
            {
                // チェックボックスがオフの場合の処理
            }

            return View(model);
        }
    }
}