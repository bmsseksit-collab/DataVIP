using DataVipWeb.Models;

namespace DataVipWeb.Components.Pages.BankForms;

public static class BankDetailHelper
{
    public static string GetLabel(string key, string accountType)
    {
        return key switch
        {
            "CitizenId" => "เลขบัตรประชาชน",
            "BirthDate" => "วันเกิด",

            "CompanyNo" => "ทะเบียนนิติบุคคลเลขที่",
            "CompanyCode" => GetCompanyCodeLabel(accountType),

            "TokenPin" => "SCB Digital Token PIN",

            "AtmNumber" => "เลขบัตร ATM",
            "AtmPin" => "รหัส ATM",

            "PhoneNetwork" => "เครือข่าย",
            "Phone" => "เบอร์โทร",
            "SimExpire" => "วันหมดอายุซิม",

            "BizUser" => GetBizUserLabel(accountType),
            "BizPass" => GetBizPassLabel(accountType),

            "ScbUser" => "SCB Username",
            "ScbPass" => "SCB Password",

            "Limit" => "วงเงิน",
            "BookBank" => "สมุดบัญชี",

            "Email" => "Email",
            "EmailPass" => "Email Password",

            "AppCode" => "รหัส APP",

            "Address" => "ที่อยู่",

            "ReceivedDate" => "วันที่รับบัญชี",
            "ReleasedDate" => "วันที่ปล่อยใช้งาน",
            "MakerSimExpire" => "วันหมดอายุซิม (ผู้ทำ)",
            "MakerEmail" => "Email (ผู้ทำ)",
            "MakerEmailPass" => "Email Password (ผู้ทำ)",
            "MakerUser" => $"{GetBizUserLabel(accountType)} (ผู้ทำ)",
            "MakerPass" => $"{GetBizPassLabel(accountType)} (ผู้ทำ)",
            "MakerCompanyCode" => $"{GetCompanyCodeLabel(accountType)} (ผู้ทำ)",

            "ApproverSimExpire" => "วันหมดอายุซิม (ผู้อนุมัติ)",
            "ApproverEmail" => "Email (ผู้อนุมัติ)",
            "ApproverEmailPass" => "Email Password (ผู้อนุมัติ)",
            "ApproverUser" => $"{GetBizUserLabel(accountType)} (ผู้อนุมัติ)",
            "ApproverPass" => $"{GetBizPassLabel(accountType)} (ผู้อนุมัติ)",
            "ApproverCompanyCode" => $"{GetCompanyCodeLabel(accountType)} (ผู้อนุมัติ)",

            _ => key
        };
    }

    public static string GetBizUserLabel(string accountType)
    {
        if (accountType.StartsWith("TTB"))
            return "TTB User";

        if (accountType.StartsWith("KTB"))
            return "KTB Biz User";

        if (accountType.StartsWith("KBANK"))
            return "K Biz User";

        if (accountType.StartsWith("BAY"))
            return "BAY Biz User";

        if (accountType.StartsWith("GSB"))
            return "GSB User";

        if (accountType.StartsWith("KKP"))
            return "KKP User";

        return "Biz User";
    }

    public static string GetBizPassLabel(string accountType)
    {
        if (accountType.StartsWith("TTB"))
            return "TTB Pass";

        if (accountType.StartsWith("KTB"))
            return "KTB Biz Pass";

        if (accountType.StartsWith("KBANK"))
            return "K Biz Pass";

        if (accountType.StartsWith("BAY"))
            return "BAY Biz Pass";

        if (accountType.StartsWith("GSB"))
            return "GSB Pass";

        if (accountType.StartsWith("KKP"))
            return "KKP Pass";

        return "Biz Pass";
    }

    public static string GetCompanyCodeLabel(string accountType)
    {
        if (accountType.StartsWith("KTB"))
            return "KTB รหัสบริษัท";

        if (accountType.StartsWith("BAY"))
            return "BAY รหัสบริษัท";

        if (accountType.StartsWith("GSB"))
            return "รหัสบริษัท";

        if (accountType.StartsWith("TTB"))
            return "TTB รหัสบริษัท";

        return "รหัสบริษัท";
    }

    public static string[] GetDetailOrder(string accountType)
    {
        return accountType switch
        {
            "SCB นิติ" => new[]
            {
            "CompanyNo", "TokenPin", "Phone", "SimExpire",
            "ScbUser", "ScbPass", "Email", "EmailPass", "AppCode"
        },

            "KBANK นิติ" => new[]
            {
            "Phone", "SimExpire", "BizUser", "BizPass",
            "Email", "EmailPass", "AppCode"
        },

            "KBANK บุคคล" => new[]
            {
            "CitizenId", "BirthDate", "AtmNumber", "AtmPin",
            "Phone", "SimExpire", "BizUser", "BizPass",
            "Email", "EmailPass", "AppCode"
        },

            "KTB นิติ" or "BAY นิติ" => new[]
            {
            "Phone", "SimExpire", "CompanyNo", "CompanyCode",
            "BizUser", "BizPass", "Email", "EmailPass", "AppCode"
        },

            "TTB นิติ" => new[]
            {
            "CompanyCode", "BizUser", "BizPass",
            "Email", "EmailPass", "AppCode"
        },

            "GSB นิติ" or "KKP นิติ" => new[]
            {
            "CompanyNo",
            "MakerPhone", "MakerSimExpire", "MakerEmail", "MakerEmailPass",
            "MakerUser", "MakerPass", "MakerCompanyCode",
            "ApproverPhone", "ApproverSimExpire", "ApproverEmail", "ApproverEmailPass",
            "ApproverUser", "ApproverPass", "ApproverCompanyCode",
            "AppCode"
        },

            _ => new[]
            {
            "AtmNumber", "AtmPin", "Phone", "SimExpire",
            "Limit", "BookBank", "Email", "EmailPass", "AppCode"
        }
        };
    }

    public static string GetBankThemeClass(string bankName)
    {
        if (bankName.Contains("SCB"))
            return "theme-scb";

        if (bankName.Contains("GSB"))
            return "theme-gsb";

        if (bankName.Contains("BAY"))
            return "theme-bay";

        if (bankName.Contains("KTB"))
            return "theme-ktb";

        if (bankName.Contains("KBANK"))
            return "theme-kbank";

        if (bankName.Contains("BBL"))
            return "theme-bbl";

        return "theme-default";
    }

    public static string GetBankShortName(string bankName)
    {
        if (string.IsNullOrWhiteSpace(bankName))
            return "";

        return bankName
            .Replace(" บุคคล", "")
            .Replace(" นิติ", "")
            .Trim();
    }

    public static string GetStatusClass(string status)
    {
        return status switch
        {
            "ใช้งานอยู่" => "status-active",
            "พร้อมใช้งาน" => "status-ready",
            "มีปัญหา" => "status-problem",
            _ => "status-ready"
        };
    }

    public static string FormatValue(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "-";

        if (key.Contains("Date") || key.Contains("Expire"))
        {
            if (DateTime.TryParse(value, out var date))
                return FormatThaiDate(date);
        }

        return value;
    }

    public static string FormatThaiDate(DateTime date)
    {
        string[] months =
        {
        "ม.ค.", "ก.พ.", "มี.ค.", "เม.ย.",
        "พ.ค.", "มิ.ย.", "ก.ค.", "ส.ค.",
        "ก.ย.", "ต.ค.", "พ.ย.", "ธ.ค."
    };

        return $"{date.Day} {months[date.Month - 1]} {date.Year + 543}";
    }
    public static Dictionary<string, string> ParseDetails(string? detailsJson)
    {
        if (string.IsNullOrWhiteSpace(detailsJson))
            return new();

        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(detailsJson) ?? new();
        }
        catch
        {
            return new();
        }
    }
    public static void AddDetailRow(
    List<(string Label, string Value)> rows,
    string key,
    Func<string, string> getRawDetail,
    string accountType)
    {
        if (key == "MakerPhone")
        {
            var network = getRawDetail("MakerPhoneNetwork");
            var phone = getRawDetail("MakerPhone");

            if (!string.IsNullOrWhiteSpace(phone))
            {
                rows.Add((
                    string.IsNullOrWhiteSpace(network)
                        ? "เบอร์โทร (ผู้ทำ)"
                        : $"เบอร์ {network} (ผู้ทำ)",
                    phone
                ));
            }

            return;
        }

        if (key == "ApproverPhone")
        {
            var network = getRawDetail("ApproverPhoneNetwork");
            var phone = getRawDetail("ApproverPhone");

            if (!string.IsNullOrWhiteSpace(phone))
            {
                rows.Add((
                    string.IsNullOrWhiteSpace(network)
                        ? "เบอร์โทร (ผู้อนุมัติ)"
                        : $"เบอร์ {network} (ผู้อนุมัติ)",
                    phone
                ));
            }

            return;
        }

        if (key == "Phone")
        {
            var network = getRawDetail("PhoneNetwork");
            var phone = getRawDetail("Phone");

            if (!string.IsNullOrWhiteSpace(phone))
            {
                rows.Add((
                    string.IsNullOrWhiteSpace(network)
                        ? "เบอร์โทร"
                        : $"เบอร์ {network}",
                    phone
                ));
            }

            return;
        }

        var value = getRawDetail(key);

        if (!string.IsNullOrWhiteSpace(value))
        {
            rows.Add((
                GetLabel(key, accountType),
                FormatValue(key, value)
            ));
        }
    }
    public static string GetBankLogo(string bankName)
    {
        if (bankName.Contains("SCB")) return "/images/banks/scb.png";
        if (bankName.Contains("GSB")) return "/images/banks/gsb.png";
        if (bankName.Contains("KTB")) return "/images/banks/ktb.png";
        if (bankName.Contains("KBANK")) return "/images/banks/kbank.png";
        if (bankName.Contains("BAY")) return "/images/banks/bay.png";
        if (bankName.Contains("BBL")) return "/images/banks/bbl.png";
        if (bankName.Contains("TTB")) return "/images/banks/ttb.png";
        if (bankName.Contains("KKP")) return "/images/banks/kkp.png";
        if (bankName.Contains("BAAC")) return "/images/banks/baac.png";

        return "/images/banks/default.png";
    }

}