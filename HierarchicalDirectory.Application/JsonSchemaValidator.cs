using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace HierarchicalDirectory.Application
{
    public static class JsonSchemaValidator
    {
        public static (bool valid, List<string> errors) Validate(string schemaJson, string dataJson)
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(schemaJson)) return (true, errors);
            try
            {
                var schema = JSchema.Parse(schemaJson);
                var data = JToken.Parse(dataJson);
                bool valid = data.IsValid(schema, out IList<ValidationError> validationErrors);
                if (!valid)
                {
                    foreach (var err in validationErrors)
                        errors.Add(err.Message);
                }
                return (valid, errors);
            }
            catch (System.Exception ex)
            {
                errors.Add($"Schema or data parse error: {ex.Message}");
                return (false, errors);
            }
        }
    }
}
