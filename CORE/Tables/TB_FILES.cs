using CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Tables
{
    public class TB_FILES : BusinessObject
    {
        public enum TB_FILES_Field
        {
            FileId,
            FileOrg,
            FilePath,
            FileData,
            FileStatus,
            FileType,
            FileReferenceId

        }
        [PrimaryKey]
        public int FileId { get; set; }
        public string FileOrg { get; set; }
        public string FilePath { get; set; }
        public string FileData { get; set; }
        public string FileStatus { get; set; }
        public string FileType { get; set; }
        public string FileReferenceId { get; set; }

    }
}
