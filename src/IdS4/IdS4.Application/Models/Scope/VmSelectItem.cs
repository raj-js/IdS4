﻿namespace IdS4.Application.Models.Scope
{
    public class VmSelectItem
    {
        public string Label { get; set; }
        public string Value { get; set; }

        public VmSelectItem(string label, string value)
        {
            Label = label;
            Value = value;
        }
    }
}
