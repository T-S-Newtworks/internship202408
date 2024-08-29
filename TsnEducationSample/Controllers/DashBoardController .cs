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
    public class DashBoardController : Controller
    {
        public ActionResult Index()
        {


            return View();
        }

        public ActionResult Sample()
        {
            //TODOリストの読み込み
            string filePath = @"C:\temp\TsnEducation2024\todoItem.csv";

            List<TodoItem> todoItems = new List<TodoItem>();
            //ファイルを読み込めなかった場合は空のリストを返す
            if (System.IO.File.Exists(filePath))
            {
                //ファイル読み込み
                using (var readFile = new StreamReader(filePath, Encoding.GetEncoding("shift-jis")))
                {
                    while (!readFile.EndOfStream)
                    {
                        //csvファイルなのでカンマ区切りでデータを抽出
                        string[] rowValues = readFile.ReadLine().Split(',');
                        //データ形式が正しくない場合は扱わない
                        if (rowValues.Length != 2)
                        {
                            continue;
                        }
                        //データを画面表示用に加工
                        todoItems.Add(new TodoItem()
                        {
                            Title = rowValues[0],
                            Description = rowValues[1]
                        });
                    }
                }
            }
            //データモデルの作成、引数に設定
            return View(todoItems);
        }

        /// <summary>
        /// TODOリストの保存
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveTodoItems(List<TodoItem> todoItems)
        {
            //TODO情報を保存するファイル
            string filePath = @"C:\temp\TsnEducation2024\todoItem.csv";
            //保存用ファイルの作成
            CreateFile(filePath);
            //アイテムが空の場合は保存先ファイルを空にする
            if (todoItems == null || !todoItems.Any())
            {
                System.IO.File.WriteAllText(filePath, string.Empty);
            }
            else
            {
                //ファイルの書き込み、保存
                //デリートインサート形式で保存(直前の内容をすべて消し、現在の情報を再登録しなおす)
                using (var overWriteFile = new StreamWriter(filePath, false, Encoding.GetEncoding("shift-jis")))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in todoItems)
                    {
                        sb.AppendLine($"{item.Title},{item.Description}");
                    }

                    overWriteFile.Write(sb.ToString());
                }
            }
            //サンプル画面へリダイレクト
            return RedirectToAction("Sample");
        }

        /// <summary>
        /// ファイルを作成する
        /// </summary>
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