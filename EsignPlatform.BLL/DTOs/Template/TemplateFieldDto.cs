using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.DTOs.Template
{
    // SchemaJson-dakı bir field-in tərifi
    public class TemplateFieldDto
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = null!;

        [JsonPropertyName("label")]
        public string Label { get; set; } = null!;

        // text | number | date | textarea
        [JsonPropertyName("type")]
        public string Type { get; set; } = "text";

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

}
