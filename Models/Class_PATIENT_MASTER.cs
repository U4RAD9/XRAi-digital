using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNETCOREEXAMPLE.Models
{
    [Table("PATIENT_MASTER", Schema = "public")]
    public class Class_PATIENT_MASTER
    {
        [Key]
        public int PATIENTID { get; set; }
        public string PATIENTNAME { get; set; }
        public int AGE { get; set; }
        public int WEIGHT { get; set; }
        public string GENDER { get; set; }
        public string ADDRESS { get; set; }
        public string ALTERNATEMOBILENUMBER { get; set; }
        public string EMAIL { get; set; }
        public int USERID { get; set; }
        public string PATIENTFILENAME { get; set; }
        public string PATIENTFILEEXTENSION { get; set; }
    }
}
