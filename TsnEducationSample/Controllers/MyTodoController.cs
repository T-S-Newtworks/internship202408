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
                            Result = rowValues[4]//��Ŏ���
                        });
                    }
                }
            }

            // TempData �Ƀf�[�^��ۑ�
            TempData["TodoItems"] = todoItems;

            return View(todoItems);
        }
        
        public ActionResult MyTodoInsert()
        {
            var todoItems = new List<MyTodoInsertItem>(); // �f�[�^�̎擾�܂��͐���
            return View(todoItems);
        }

        public ActionResult MyTodoSearch()
        {

            var todoItems = TempData["TodoItems"] as List<MyTodoItem>;

            if (todoItems != null)
            {
                // MyTodoItem����SearchItem�ɕϊ�
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

            // �f�[�^���Ȃ��ꍇ�͋�̃��X�g��n��
            return View(new List<SearchItem>());
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

            TempData["SuccessMessage"] = "Todo���X�g��ۑ����܂����B";

            return RedirectToAction("MyTodoIndex");
        }

        public ActionResult delTodoItems(List<MyTodoItem> todoItems)
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

            TempData["SuccessMessage"] = "Todo���X�g���폜���܂����B";

            return RedirectToAction("MyTodoIndex");
        }

        public ActionResult tuikaTodoItems(List<MyTodoInsertItem> todoItems)
        {
            if (ModelState.IsValid)
            {
                if (todoItems == null || !todoItems.Any())
                {
                    // ���f������̏ꍇ�̏���
                    ModelState.AddModelError("", "�f�[�^�����M����Ă��܂���B");
                    todoItems = new List<MyTodoInsertItem>();
                    return View("MyTodoInsert", todoItems);
                }

                string filePath = @"C:\temp\TsnEducation2024\todoItem.csv";

                using (var writer = new StreamWriter(filePath, true, Encoding.GetEncoding("shift-jis")))
                {
                    foreach (var item in todoItems)
                    {
                        // �e�A�C�e����CSV�`���ŏ�������
                        writer.WriteLine($"{item.Day:yyyy-MM-dd},{item.Time:HH:mm},{item.Title},{item.Description},{item.Repeat}");
                    }
                }

                TempData["SuccessMessage"] = "TODO���X�g���ǉ�����܂����B";
                return RedirectToAction("MyTodoInsert");
            }

            // ���f���������ȏꍇ�A�o���f�[�V�����G���[�̃��b�Z�[�W��\��
            return View("MyTodoInsert", todoItems);
        }

        [HttpPost]
        public ActionResult Search(string title)
        {
            // CSV�t�@�C������f�[�^��ǂݍ���
            var filePath = @"C:\temp\TsnEducation2024\todoItem.csv";
            var todos = ReadTodoItemsFromCsv(filePath);

            // �^�C�g���Ńt�B���^�����O�i�啶������������ʂ��Ȃ��j
            var filteredTodos = todos
                .Where(item => item.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            // �t�B���^�����O���ꂽ���X�g���r���[�ɓn��
            return View("MyTodoSearch", filteredTodos);
        }

        public ActionResult DeleteTodoItems(List<MyTodoItem> deleteItems)
        {
            var filePath = @"C:\temp\TsnEducation2024\todoItem.csv";
            var todos = ReadTodoItemsFromCsv(filePath);

            // �폜����A�C�e�������
            var remainingTodos = todos
                .Where(todo => !deleteItems.Any(d => d.Day == todo.Day && d.Time == todo.Time && d.Title == todo.Title && d.Description == todo.Description && d.Result == todo.Result))
                .ToList();

            SaveTodoItemsToCsv(filePath, remainingTodos);

            TempData["SuccessMessage"] = "�I���������ڂ��폜���܂����B";
            return RedirectToAction("MyTodoSearch");
        }

        private void SaveTodoItemsToCsv(string filePath, List<MyTodoItem> todos)
        {
            var csvLines = new List<string>();

            // CSV�w�b�_�[
            csvLines.Add("Day,Time,Title,Description,Result");

            // �eMyTodoItem�I�u�W�F�N�g��CSV�`���ɕϊ����ă��X�g�ɒǉ�
            foreach (var todo in todos)
            {
                var line = $"{todo.Day:yyyy-MM-dd},{todo.Time:HH:mm},{todo.Title},{todo.Description},{todo.Result}";
                csvLines.Add(line);
            }

            // �t�@�C���ɏ�������
            System.IO.File.WriteAllLines(filePath, csvLines);
        }
        private List<MyTodoItem> ReadTodoItemsFromCsv(string filePath)
        {
            var todos = new List<MyTodoItem>();

            try
            {
                // CSV�t�@�C���̑S�s��ǂݍ���
                var lines = System.IO.File.ReadAllLines(filePath);

                // 1�s�ڂ̓w�b�_�[�s�ł���Ɖ��肵�āA�X�L�b�v����
                foreach (var line in lines.Skip(1))
                {
                    // �J���}�ŋ�؂�ꂽ�l���擾
                    var values = line.Split(',');

                    // �K�v�ȃt�B�[���h�������邩�m�F
                    if (values.Length >= 5)
                    {
                        // MyTodoItem�I�u�W�F�N�g���쐬
                        var todo = new MyTodoItem
                        {
                            Day = DateTime.Parse(values[0]),
                            Time = DateTime.Parse(values[1]),
                            Title = values[2],
                            Description = values[3],
                            Result = values[4]
                        };

                        // ���X�g�ɒǉ�
                        todos.Add(todo);
                    }
                }
            }
            catch (Exception ex)
            {
                // �G���[�n���h�����O: ���O���L�^���邩�A�K�؂ȏ������s��
                Console.WriteLine("CSV�ǂݍ��ݒ��ɃG���[���������܂���: " + ex.Message);
            }

            return todos;
        }

        //public ActionResult SearchTodoItems(List<SerachItem> todoItems)
        //{


        //}


        public  ActionResult list()
        {
            // �f�t�H���g�ŕ\������y�[�W���u�ꗗ�v�ɐݒ�
            var defaultlist = "list";
            return RedirectToAction("List", new { day = defaultlist });
        }

        public ActionResult week(string day)
        {
            ViewBag.SelectedDay = day;
            // �K�v�ȃf�[�^���r���[�ɓn���āuDay�v�r���[��Ԃ�
            return View("day");
        }

        public ActionResult weeklist(string day)
        {
            ViewBag.SelectedDay = day;

            return View("List");
        }
        /// <param name="filePath">�쐬��̃p�X</param>
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
            var selectedTodoItems = GetTodoItemsForDay(day); // `GetTodoItemsForDay` ���\�b�h�͉���
            ViewBag.SelectedDay = day;
            return View(selectedTodoItems); // Day.cshtml ���K�؂ȃ��f�����󂯎�邱�Ƃ��m�F
        }

        private List<MyTodoItem> GetTodoItemsForDay(string day)
        {
            // day �Ɋ�Â��ă��X�g���t�B���^�����O���郍�W�b�N��ǉ�
            return new List<MyTodoItem>(); // ��Ƃ��ċ�̃��X�g��Ԃ�
        }

        public ActionResult Index()
        {
            var model = new MyViewModel { IsChecked = true }; // �f�t�H���g�Ń`�F�b�N�{�b�N�X���I���ɂȂ��
            return View(model);
        }

        public ActionResult Index(MyViewModel model)
        {
            if (model.IsChecked)
            {
                // �`�F�b�N�{�b�N�X���I���̏ꍇ�̏���
            }
            else
            {
                // �`�F�b�N�{�b�N�X���I�t�̏ꍇ�̏���
            }

            return View(model);
        }
    }
}