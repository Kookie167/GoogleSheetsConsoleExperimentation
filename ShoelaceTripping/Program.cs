using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace ShoelaceTripping
{
    class Program
    {
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static GoogleCredential credential;
        static SheetsService service;
        static string ApplicationName = "Google Sheets API Experimentation";

        public static void printSpreadsheet(string range, string spreadsheetId)
        {   
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                for (int i = 1; i < values.Count; ++i)
                {
                    IList<Object> row = values[i];
                    foreach (Object o in row)
                    {
                        Console.Write(o + " ");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
        }

        public static void printSpreadsheet1(string spreadsheetId)
        {
            BatchGetRequest br = new BatchGetRequest(service, spreadsheetId);
            br.MajorDimension = BatchGetRequest.MajorDimensionEnum.ROWS;
            List<string> ranges = new List<string>();
            ranges.Add("Responses!A:I");
            br.Ranges = ranges;

            BatchGetValuesResponse response = br.Execute();
            ValueRange vr = response.ValueRanges[0];
            Console.WriteLine(vr.Range);
            foreach (IList<object> row in vr.Values)
            {
                foreach (object cell in row)
                {
                    Console.Write($"[{cell}], ");
                }
                Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {
            credential = GoogleCredential.FromFile("casual-galaxy.json").CreateScoped(new string[] { SheetsService.Scope.Spreadsheets });
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            string spreadsheetId = "1h61wHfOV_5FSBbsOuaQAJxvUB6wHjd6P0yDkBU23irY";
            string range = "Responses!A:I";

            //printSpreadsheet(range, spreadsheetId);
            Spreadsheet s = service.Spreadsheets.Get(spreadsheetId).Execute();
            IList<Sheet> sheets = s.Sheets;
            foreach (Sheet sheet in sheets) {
                Console.WriteLine($"{sheet.Properties.Title}, {sheet.Properties.SheetId}");
            }

            Console.WriteLine();
            printSpreadsheet1(spreadsheetId);

            string sheetName = "new sheet";
            AddSheetRequest addSheetRequest = new AddSheetRequest();
            addSheetRequest.Properties = new SheetProperties();
            addSheetRequest.Properties.Title = sheetName;

            Console.WriteLine(addSheetRequest.Properties.SheetId);

            UpdateCellsRequest updateCellsRequest = new UpdateCellsRequest();
            updateCellsRequest.Start = new GridCoordinate();
            updateCellsRequest.Start.SheetId = addSheetRequest.Properties.SheetId;

            BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
            batchUpdateSpreadsheetRequest.Requests = new List<Request>();
            batchUpdateSpreadsheetRequest.Requests.Add(new Request
            {
                AddSheet = addSheetRequest
            });
            SpreadsheetsResource.BatchUpdateRequest batchUpdateRequest = service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, spreadsheetId);
            //batchUpdateRequest.Execute();
        }
    }
}
