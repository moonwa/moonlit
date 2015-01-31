using System;
using System.Collections.Generic;

namespace Moonlit.Data.Design.Entity
{
    [Serializable]
    public class ModelMetadata
    {
        public string Name { get; set; }
        public string TypeName { get; set; }

        public ModelMetadata()
        {
            Properties = new List<PropertyMetadata>();
        }
        public List<PropertyMetadata> Properties { get; set; }
    }
}