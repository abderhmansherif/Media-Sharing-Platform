using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace BeatBox.Models
{
    public class FileOperationResults<T>
    {
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
        public T Output { get; set; }
    }
}