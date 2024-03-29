﻿using System;
using WebsiteManagement.Application.Websites.Commands.UpdateWebsite;

namespace WebsiteManagement.Api.Models.Input
{
    public class UpdateWebsiteRequest : WebsiteManipulationRequest
    {
        public UpdateWebsite ToUpdateWebsite(Guid websiteId)
        {
            return new UpdateWebsite(websiteId,
                                     Name,
                                     Url,
                                     Categories,
                                     GetImage(),
                                     Login.Email,
                                     Login.Password);
        }
    }
}
