using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.SharedBlazor.Domain
{
    public struct ModelDescription(
        string fieldName,
        string filedType,
        string description,
        bool isRequired = false)
    {
        public string FieldName { get; init; } =fieldName;
        public string FieldType { get; init; } =filedType;
        public string Description {  get; init; } =description;
        public bool Required { get; init; } = isRequired;
    }
}
