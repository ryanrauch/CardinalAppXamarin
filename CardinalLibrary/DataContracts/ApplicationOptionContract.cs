using System;
using System.Collections.Generic;
using System.Text;

namespace CardinalLibrary.DataContracts
{
    public class ApplicationOptionContract
    {
        public string EndUserLicenseAgreementSource { get; set; }
        public string TermsConditionsSource { get; set; }
        public string PrivacyPolicySource { get; set; }
        public TimeSpan DataTimeWindow { get; set; }
        public int Version { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
    }
}
