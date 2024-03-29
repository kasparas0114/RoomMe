﻿using RoomMe.Webservice.Models.API;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMe.Webservice.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public virtual Career Job { get; set; }
        public virtual Housing Housing { get; set; }
        public Status Status { get; set; }
        public List<int> FavoritedUserIDs {get; set;}
        public virtual List<Tag> Tags { get; set; }
        public virtual Preferences Preferences { get; set; } // hmmm maybe a list of preferences using enum/value structure...?
        public double? HousingPrice { get; set; }

        public string AuthToken { get; set; }

        // todo: matches and suggestions. Suggestions shouldn't be stored with the user because it stores state and could be stale.

        public APIUser ToAPIModel()
        {
            return new APIUser
            {
                UserID = this.UserID,
                Age = this.Age,
                Email = this.Email,
                Gender = this.Gender,
                HousingPrice = this.HousingPrice,
                Name = this.Name,
                PhoneNumber = this.PhoneNumber,
                Status = this.Status,
                Bio = this.Bio,
                FavoritedUserIDs = this.FavoritedUserIDs,
                AuthToken = this.AuthToken
            };
        }
    }
}
