﻿namespace Mango.web.Models
{
    public class CartDto
    {
        public CartHeaderDto CardHeader { get; set; }
        public IEnumerable<CartDetailsDto> CartDetails { get; set; }
    }
}
