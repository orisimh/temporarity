using backend.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace backend.Services
{
    public class FileReaderService
    {
        private readonly string _xmlFilePath;
        private readonly string _jsonFilePath;

        public FileReaderService(IConfiguration cng)
        {

            _xmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), cng["FilePaths:XmlFile"]);
            _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), cng["FilePaths:JsonFile"]);

        }
        public async Task<string> ReadXmlFileAsync()
        {
            if (File.Exists(_xmlFilePath))
            {
                var xmlContent = await Task.Run(() => XElement.Load(_xmlFilePath));
                return xmlContent.ToString();
            }
            else
            {
                return null;//  throw new Exception("XML file not found.");
            }
        }

        public async Task<string> ReadJsonFileAsync()
        {
            if (File.Exists(_jsonFilePath))
            {
                var jsonContent = await File.ReadAllTextAsync(_jsonFilePath);
                return jsonContent;
            }
            else
            {
                return null; //  throw new Exception("JSON file not found.");

            }
        }

        // Parse XML into List<Row>
        public async Task<List<Row>> ParseXmlFileAsync()
        {
            if (File.Exists(_xmlFilePath))
            {
                var xmlContent = await Task.Run(() => XElement.Load(_xmlFilePath));

                var rows = xmlContent.Descendants("row")
                    .Select(row => new Row
                    {
                        Code = row.Element("Code")?.Value,
                        Description = row.Element("Description")?.Value
                    })
                    .ToList();

                return rows;
            }
            else
            {
                return null;
            }
        }

        // Parse JSON into List<Row>
        public async Task<List<Row>> ParseJsonFileAsync()
        {
            if (File.Exists(_jsonFilePath))
            {
                var jsonContent = await File.ReadAllTextAsync(_jsonFilePath);

                var parsedJson = JsonSerializer.Deserialize<JsonRoot>(jsonContent);

                return parsedJson?.root?.row.ToList() ?? new List<Row>();
            }
            else
            {
                return null;
            }
        }
    }
}

