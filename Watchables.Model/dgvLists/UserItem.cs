﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Watchables.Model.dgvLists
{
    public class UserItem
    {
        public int UserId { get; set; }
        public string Mail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string Username { get; set; }
        public bool Active { get; set; }
    }
}
