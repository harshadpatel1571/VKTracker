using System;

namespace VKTracker.Model.ViewModel
{
    public class CaptchaResponceViewModel
    {
        public bool Success { get; set; }
        public DateTime Challenge_ts { get; set; }
        public string Hostname { get; set; }
        public double Score { get; set; }
        public string Action { get; set; }
    }
}
