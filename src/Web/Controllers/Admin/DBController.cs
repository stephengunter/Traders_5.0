using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Views;
using ApplicationCore.Helpers;
using ApplicationCore.Settings;
using Microsoft.Extensions.Options;
using ApplicationCore.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using Web.Controllers;
using System.Reflection;

namespace Web.Controllers.Admin
{
	public class DBController : BaseAdminController
	{
		private readonly AdminSettings _adminSettings;
		private readonly HistoryContext _context;
		private readonly IDBImportService _dBImportService;

		public DBController(IOptions<AdminSettings> adminSettings, HistoryContext context, IDBImportService dBImportService)
		{
			_adminSettings = adminSettings.Value;
			_context = context;
			_dBImportService = dBImportService;
		}

		#region Properties

		string _connectionString;
		string ConnectionString
		{
			get
			{
				if (String.IsNullOrEmpty(_connectionString))
				{
					_connectionString = _context.Database.GetDbConnection().ConnectionString;
				}
				return _connectionString;
			}
		}

		string _dbName;
		string DbName
		{
			get
			{
				if (String.IsNullOrEmpty(_dbName))
				{
					_dbName = new SqlConnectionStringBuilder(ConnectionString).InitialCatalog;
				}
				return _dbName;
			}
		}



		string _backupFolder;
		string BackupFolder
		{
			get
			{
				if (String.IsNullOrEmpty(_backupFolder))
				{
					var path = Path.Combine(_adminSettings.BackupPath, DateTime.Today.ToDateNumber().ToString());
					if (!Directory.Exists(path)) Directory.CreateDirectory(path);

					_backupFolder = path;
				}
				return _backupFolder;
			}
		}
		#endregion

		async Task<string> ReadFileTextAsync(IFormFile file)
		{
			var result = new StringBuilder();
			using (var reader = new StreamReader(file.OpenReadStream()))
			{
				while (reader.Peek() >= 0) result.AppendLine(await reader.ReadLineAsync());
			}
			return result.ToString();

		}

		[HttpGet("dbname")]
		public ActionResult DBName() => Ok(DbName);

		[HttpPost("migrate")]
		public ActionResult Migrate([FromBody] AdminRequest model)
		{
			
			ValidateRequest(model, _adminSettings);
			if (!ModelState.IsValid) return BadRequest(ModelState);

			_context.Database.Migrate();

			return Ok();
		}

		[HttpPost("backup")]
		public ActionResult Backup([FromBody] AdminRequest model)
		{
			ValidateRequest(model, _adminSettings);
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var fileName = Path.Combine(BackupFolder, $"{DbName}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.bak");

			string cmdText = $"BACKUP DATABASE [{DbName}] TO DISK = '{fileName}'";
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				using (SqlCommand cmd = new SqlCommand(cmdText, conn))
				{
					int result = cmd.ExecuteNonQuery();

				}
				conn.Close();
			}

			return Ok();
		}

		[HttpPost("export")]
		public ActionResult Export([FromBody] AdminRequest model)
		{
			ValidateRequest(model, _adminSettings);
			if (!ModelState.IsValid) return BadRequest(ModelState);
			
			var folderPath = BackupFolder;
		
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

			var kLines = _context.KLines.ToList();
			SaveJson(folderPath, new KLine().GetType().Name, JsonConvert.SerializeObject(kLines));

			return Ok();
		}

		[HttpPost("import")]
		public async Task<IActionResult> Import([FromForm] AdminFileRequest model)
		{
			ValidateRequest(model, _adminSettings);
			if (!ModelState.IsValid) return BadRequest(ModelState);


			var fileNames = new List<string>();

			if (model.Files.Count < 1)
			{
				ModelState.AddModelError("files", "必須上傳檔案");
				return BadRequest(ModelState);
			}

			var extensions = model.Files.Select(item => Path.GetExtension(item.FileName).ToLower());
			if (extensions.Any(x => x != ".json"))
			{
				ModelState.AddModelError("files", "檔案格式錯誤");
				return BadRequest(ModelState);
			}

			string content = "";
			string fileName = new KLine().GetType().Name;
			var file = model.GetFile(fileName);
			if (file != null)
			{
				fileNames.Add(fileName);
				content = await ReadFileTextAsync(file);
				var kLineList = JsonConvert.DeserializeObject<List<KLine>>(content);
				_dBImportService.ImportKLines(_context, kLineList);

				_dBImportService.SyncKLines(_context, kLineList);

			}

			//end of import

			return Ok();
		}

		void SaveJson(string folderPath, string name, string content)
		{
			var filePath = Path.Combine(folderPath, $"{name}.json");
			System.IO.File.WriteAllText(filePath, content);
		}

	}
}
