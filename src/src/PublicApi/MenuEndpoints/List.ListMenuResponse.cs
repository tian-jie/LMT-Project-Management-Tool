using Microsoft.eShopWeb.ApplicationCore.Entities;
using System;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.PublicApi.MenuEndpoints
{
    public class ListMenuResponse : BaseResponse
    {
        public ListMenuResponse(Guid correlationId) : base(correlationId)
        {
        }

        public ListMenuResponse()
        {
        }


        public List<AspNetMenu> Menus { get; set; } = new List<AspNetMenu>();

    }
}
