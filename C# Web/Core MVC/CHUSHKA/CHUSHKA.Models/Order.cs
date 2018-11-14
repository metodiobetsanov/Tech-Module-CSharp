﻿using System;

namespace CHUSHKA.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public virtual ChushkaUser Client { get; set; }

        public DateTime OrderedOn { get; set; }

    }
}
