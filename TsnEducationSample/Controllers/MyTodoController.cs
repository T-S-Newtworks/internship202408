using System;
using System.Collections.Generic;
using System.Globalization;
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

            // TempData にデータを保存
            TempData["TodoItems"] = todoItems;

            return View(todoItems);
        }
        
        public ActionResult MyTodoInsert()
        {
            var todoItems = new List<MyTodoInsertItem>(); // データの取得または生成
            return View(todoItems);
        }

        public ActionResult MyTodoSearch()
        {

            var todoItems = TempData["TodoItems"] as List<MyTodoItem>;

            if (todoItems != null)
            {
                // MyTodoItemからSearchItemに変換
                var searchItems = todoItems.Select(item => new SearchItem
                {
                    Day = item.Day,
                    Time = item.Time,
                    Title = item.Title,
                    Description = item.Description,
                    Result = item.Result
                }).ToList();

                return View(searchItems);
            }

            // データがない場合は空のリストを渡す
            return View(new List<SearchItem>());
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

            return RedirectToAction("MyTodoIndex");
        }

        public ActionResult delTodoItems(List<MyTodoItem> todoItems)
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

            TempData["SuccessMessage"] = "Todoリストを削除しました。";

            return RedirectToAction("MyTodoIndex");
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

                //string filePath = @"C:\temp\TsnEducation2024\todoItem.csv";
                string filePath = AppContext.BaseDirectory + @"\App_Data\todoItem.csv";
                CreateFile(filePath);

                using (var writer = new StreamWriter(filePath, true, Encoding.GetEncoding("shift-jis")))
                {
                    foreach (var item in todoItems)
                    {
                        // 各アイテムをCSV形式で書き込む
                        writer.WriteLine($"{item.Day:yyyy/MM/dd},{item.Time:HH:mm},{item.Title},{item.Description},{item.Repeat}");
                    }
                }

                TempData["SuccessMessage"] = "TODOリストが追加されました。";
                return RedirectToAction("MyTodoInsert");
            }

            // モデルが無効な場合、バリデーションエラーのメッセージを表示
            return View("MyTodoInsert", todoItems);
        }

        public ActionResult Search(string title)
        {
            //string filePath = @"C:\temp\TsnEducation2024\todoItem.csv";
            string filePath = AppContext.BaseDirectory + @"\App_Data\todoItem.csv";
            var todoItems = ReadTodoItemsFromFile(filePath);

            // タイトルが部分一致するタスクのみをフィルター
            if (!string.IsNullOrEmpty(title))
            {
                todoItems = todoItems.Where(t => t.Title.Contains(title)).ToList();
            }

            var serachItems = new List<SearchItem>();

            foreach (var item in todoItems)
            {
                serachItems.Add(new SearchItem() { Day = item.Day , Time = item.Time ,Title = item.Title, Description = item.Description });
            }

            return View("MyTodoSearch", serachItems);
        }

        private List<MyTodoItem> ReadTodoItemsFromFile(string filePath)
        {
            var todoItems = new List<MyTodoItem>();
            var lines = System.IO.File.ReadAllLines(filePath, Encoding.GetEncoding("shift-jis"));

            foreach (var line in lines)
            {
                var values = line.Split(',');
                todoItems.Add(new MyTodoItem
                {
                    Day = DateTime.Parse(values[0]),
                    Time = DateTime.Parse(values[1]),
                    Title = values[2],
                    Description = values[3],
                    Result = values[4]
                });
            }

            return todoItems;
        }
        public ActionResult Description(string description)
        {
            //string filePath = @"C:\temp\TsnEducation2024\todoItem.csv";
            string filePath = AppContext.BaseDirectory + @"\App_Data\todoItem.csv";
            var todoItems = ReadTodoItemsFromFile(filePath);

            // タイトルが部分一致するタスクのみをフィルター
            if (!string.IsNullOrEmpty(description))
            {
                todoItems = todoItems.Where(d => d.Description.Contains(description)).ToList();
            }

            var serachItems = new List<SearchItem>();

            foreach (var item in todoItems)
            {
                serachItems.Add(new SearchItem() { Day = item.Day, Time = item.Time, Title = item.Title, Description = item.Description });
            }

            return View("MyTodoSearch", serachItems);
        }

        public ActionResult DeleteTodoItems(List<MyTodoItem> deleteItems)
        {
            //var filePath = @"C:\temp\TsnEducation2024\todoItem.csv";
            string filePath = AppContext.BaseDirectory + @"\App_Data\todoItem.csv";
            var todos = ReadTodoItemsFromCsv(filePath);

            // 削除するアイテムを特定
            var remainingTodos = todos
                .Where(todo => !deleteItems.Any(d => d.Day == todo.Day && d.Time == todo.Time && d.Title == todo.Title && d.Description == todo.Description && d.Result == todo.Result))
                .ToList();

            SaveTodoItemsToCsv(filePath, remainingTodos);

            TempData["SuccessMessage"] = "選択した項目を削除しました。";
            return RedirectToAction("MyTodoSearch");
        }

        private void SaveTodoItemsToCsv(string filePath, List<MyTodoItem> todos)
        {
            var csvLines = new List<string>();

            // CSVヘッダー
            csvLines.Add("Day,Time,Title,Description,Result");

            // 各MyTodoItemオブジェクトをCSV形式に変換してリストに追加
            foreach (var todo in todos)
            {
                var line = $"{todo.Day:yyyy-MM-dd},{todo.Time:HH:mm},{todo.Title},{todo.Description},{todo.Result}";
                csvLines.Add(line);
            }

            // ファイルに書き込む
            System.IO.File.WriteAllLines(filePath, csvLines);
        }
        private List<MyTodoItem> ReadTodoItemsFromCsv(string filePath)
        {
            var todos = new List<MyTodoItem>();

            try
            {
                // CSVファイルの全行を読み込む
                var lines = System.IO.File.ReadAllLines(filePath);

                // 1行目はヘッダー行であると仮定して、スキップする
                foreach (var line in lines.Skip(1))
                {
                    // カンマで区切られた値を取得
                    var values = line.Split(',');

                    // 必要なフィールド数があるか確認
                    if (values.Length >= 5)
                    {
                        // MyTodoItemオブジェクトを作成
                        var todo = new MyTodoItem
                        {
                            Day = DateTime.Parse(values[0]),
                            Time = DateTime.Parse(values[1]),
                            Title = values[2],
                            Description = values[3],
                            Result = values[4]
                        };

                        // リストに追加
                        todos.Add(todo);
                    }
                }
            }
            catch (Exception ex)
            {
                // エラーハンドリング: ログを記録するか、適切な処理を行う
                Console.WriteLine("CSV読み込み中にエラーが発生しました: " + ex.Message);
            }

            return todos;
        }

        //public ActionResult SearchTodoItems(List<SerachItem> todoItems)
        //{


        //}


        public ActionResult list()
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