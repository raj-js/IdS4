﻿namespace IdS4.Application.Models.Client
{
    public class VmClientGrantType
    {
        public int Id { get; set; }
        public string GrantType { get; set; }

        public int ClientId { get; set; }

        public VmClientGrantType()
        {
            
        }

        public VmClientGrantType(string grantType)
        {
            GrantType = grantType;
        }
    }
}
