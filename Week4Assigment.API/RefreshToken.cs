using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Week4Assigment.API
{
    public class RefreshToken
    {
        [Key]
        public string UserName { get; set; }
        public string Guid { get; set; }
        public DateTime ExpDate { get; set; }
    }
}
