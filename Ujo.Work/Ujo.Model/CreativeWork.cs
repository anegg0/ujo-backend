﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ujo.Model
{
    public abstract class CreativeWork: Entity
    {
        public CreativeWork()
        {
            CreativeWorksUsed = new List<CreativeWork>();
            CreativeWorksIn = new List<CreativeWork>();
            OtherArtists = new List<CreativeWorkArtist>();
        }

        [Key]
        public string Address { get; set; }

        [ForeignKey("ByArtist")]
        public string ByArtistAddress { get; set; }
        public virtual Artist ByArtist { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Audio { get; set; }
        /// <summary>
        /// Creator address
        /// </summary>
        public string Creator { get; set; }
        public string Genre { get; set; }
        public string Keywords { get; set; }
        public string Publisher { get; set; }
        public bool? HasPart { get; set; }
        public bool? IsPartOf { get; set; }
        public string IsFamilyFriendly { get; set; }
        public string License { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public ICollection<CreativeWorkArtist> OtherArtists { get; set; }
        public virtual ICollection<CreativeWork> CreativeWorksUsed { get; set; }
        public virtual ICollection<CreativeWork> CreativeWorksIn { get; set; }

    }

    
}