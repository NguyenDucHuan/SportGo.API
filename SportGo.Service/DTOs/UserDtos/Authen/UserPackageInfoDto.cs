using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.DTOs.UserDtos.Authen
{
    public class UserPackageInfoDto
    {
        public string PackageName { get; set; }
        public DateTime EndDate { get; set; }
        public int RemainingNormalTurns { get; set; }
        public int RemainingPriorityTurns { get; set; }
    }
}
