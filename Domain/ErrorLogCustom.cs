using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class ErrorLogCustom
    {
        public int Id { get; set; }
        public int? Number { get; set; }
        public int? Severity { get; set; }
        public int? State { get; set; }
        public string Message { get; set; }
        public int? Line { get; set; }
        public string Proced { get; set; }
    }
}
