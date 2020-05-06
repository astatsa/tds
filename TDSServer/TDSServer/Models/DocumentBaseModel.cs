using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public abstract class DocumentBaseModel : BaseModel
    {
        public DocumentBaseModel(int documentTypeId)
        {
            this.DocumentTypeId = documentTypeId;
        }

        [NotMapped]
        public int DocumentTypeId { get; }
    }
}
