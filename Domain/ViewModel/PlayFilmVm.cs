﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class PlayFilmVm
    {
        public Guid Id { get; set; }
        public string ContentType { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
