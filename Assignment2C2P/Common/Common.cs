using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2C2P
{
    public static class CommonString
    {
        public static string ExceptionCode = "999";
        public static string SuccessStatausCode = "200";
        public static string Msg_MS = "MS";
        public static string Msg_MI = "MI";
        public static string Msg_MW = "MW";
        public static string Msg_ME = "ME";
    }
}
public enum FileExtension
{
    [Description(".xml")]
    xml,
    [Description(".csv")]
    csv
}

public enum EnumStatus
{
    [Description("A")]
    Approved,
    [Description("R")]
    Rejected,
    [Description("D")]
    Done,
    [Description("R")]
    Failed,
    [Description("D")]
    Finished
}

//public enum XmlStatus
//{
//    [Description("A")]
//    Approved,
//    [Description("R")]
//    Rejected,
//    [Description("D")]
//    Done,
//}
//public enum CSVStatus
//{
//    [Description("A")]
//    Approved,
//    [Description("R")]
//    Failed,
//    [Description("D")]
//    Finished
//}