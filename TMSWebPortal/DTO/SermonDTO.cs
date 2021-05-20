 
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TMSWebPortal.Controllers;

namespace TMSWebPortal.DTO
{
    public class SermonDTO : ResultHandler
    {
        public long SermonId { get; set; }
        [Display(Name = "File Location")]
        public string FileLocation { get; set; }
        [Display(Name ="Sermon Name")]
        public string SermonName { get; set; }
        public string Preacher { get; set; }
        [Display(Name = "Sermon Category")]
        public int SermonCategoryId { get; set; }
        [Display(Name = "Level allowed to View")]
        public int LevelAllowedToView { get; set; }
        [Display(Name = "Is this for all level?")]
        public bool IsForAllLevels { get; set; }
        [Display(Name = "Position")]
        public int PositionId { get; set; }
        public string ImageUrl { get; set; }
        public string OldImageUrl { get; set; }
        public string OldFileLocationUrl { get; set; }

        
        public IEnumerable<Branch> Branches { get; set; }
        public IEnumerable<Roles> Roles { get; set; }
        public IEnumerable<Positions> Positions { get; set; }

    }
}
