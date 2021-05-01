using System;
using System.Collections.Generic;
using System.Xml;
using CBRS.Core.Models;
using CBRS.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CBRS.Core.Services.Implementations
{
    public class XmlService : IXmlService
    {
        private readonly ILogger<XmlService> _logger;
        private readonly IConfiguration _configuration;

        public XmlService(ILogger<XmlService> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public List<Schema> GetSchemas()
        {
            var response = new List<Schema>();
            var schemas = LoadSchemas();
            foreach (XmlNode schema in schemas)
            {
                response.Add(new Schema
                {
                    Name = schema.Attributes?.GetNamedItem("Name")?.Value,
                    PairCount = schema.ChildNodes.Count,
                    Pairs = GetPairs(schema.ChildNodes)
                });
            }
            return response;
        }
        private XmlElement LoadSchemas()
        {
            XmlElement schemas = null;
            try
            {
                var doc = new XmlDocument();
                doc.Load("Schema.xml");
                schemas = doc.DocumentElement;
                CheckForNull(schemas);
            }
            catch
            {
                _logger.LogError("Error while reading schema file");
            }

            return schemas;
        }
        private void CheckForNull(XmlElement schemas)
        {
            if (schemas is null)
            {
                _logger.LogError("Schema file is empty");
                throw new Exception("Schema file is empty");
            }
        }
        private List<CcyPair> GetPairs(XmlNodeList schemaChildNodes)
        {
            var response = new List<CcyPair>();
            foreach (XmlNode schemaChildNode in schemaChildNodes)
            {
                var attributes = schemaChildNode.Attributes;
                response.Add(new CcyPair
                {
                    Iso1 = attributes?.GetNamedItem("Iso1")?.Value,
                    Iso2 = attributes?.GetNamedItem("Iso2")?.Value,
                    Order = Convert.ToInt32(attributes?.GetNamedItem("Order")?.Value)
                });
            }

            return response;
        }
    }
}