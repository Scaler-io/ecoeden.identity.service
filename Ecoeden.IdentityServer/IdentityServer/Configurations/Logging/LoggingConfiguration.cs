﻿namespace IdentityServer.Configurations.Logging
{
    public class LoggingConfiguration
    {
        public string IncludeScopes { get; set; }
        public string LogOutputTemplate { get; set; }
        public Console Console { get; set; }
        public Elastic Elastic { get; set; }
    }

    public class Console
    {
        public bool Enabled { get; set; }
        public string LogLevel { get; set; }
    }

    public class Elastic
    {
        public bool Enabled { get; set; }
        public string LogLevel { get; set; }
    }
}
