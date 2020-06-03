using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OFIN.Models.Account;

namespace OFIN.Models
{
    public class ActionResponse : DbContext
    {
        public ActionResponse(DbContextOptions<ActionResponse> options) : base(options)
        {
        }

        public DbSet<ActionResponses> tActionResponses { get; set; }
    }

    public class ActionResponses
    {
        [Key]
        [JsonProperty(PropertyName = "ErrorCode")]
        public string fErrorCode { get; set; }
        [JsonProperty(PropertyName = "ErrorDesc")]
        public string fErrorDesc { get; set; }
        [JsonProperty(PropertyName = "ErrorMsg")]
        public string fErrorMsg { get; set; }
    }
}
