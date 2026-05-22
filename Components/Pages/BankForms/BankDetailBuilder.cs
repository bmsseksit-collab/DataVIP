using DataVipWeb.Models;

namespace DataVipWeb.Components.Pages.BankForms;

public class BankDetailResult
{
    public List<(string Label, string Value)> NormalRows { get; set; } = new();
    public List<(string Label, string Value)> BottomRows { get; set; } = new();
}

public static class BankDetailBuilder
{
    public static BankDetailResult Build(BankAccount account)
    {
        var result = new BankDetailResult();
        var details = BankDetailHelper.ParseDetails(account.DetailsJson);

        string GetRawDetail(string key)
        {
            return details.ContainsKey(key) ? details[key] : "";
        }

        var mainRows = BuildMainRows(account, GetRawDetail);
        var normalRows = BuildNormalRows(account, GetRawDetail);
        var bottomRows = BuildBottomRows(GetRawDetail);

        result.NormalRows = mainRows.Concat(normalRows).ToList();
        result.BottomRows = bottomRows;

        return result;
    }

    private static void AddBottomRow(
        List<(string Label, string Value)> rows,
        string key,
        string label,
        Func<string, string> getRawDetail)
    {
        var value = getRawDetail(key);

        if (!string.IsNullOrWhiteSpace(value))
        {
            rows.Add((
                label,
                BankDetailHelper.FormatValue(key, value)
            ));
        }
    }
    private static List<(string Label, string Value)> BuildMainRows(
    BankAccount account,
    Func<string, string> getRawDetail)
    {
        var rows = new List<(string Label, string Value)>();

        if (!string.IsNullOrWhiteSpace(account.AccountName))
            rows.Add(("ชื่อบัญชี", account.AccountName));

        if (!string.IsNullOrWhiteSpace(account.OwnerName))
            rows.Add(("ชื่อเจ้าของบัญชี", account.OwnerName));

        BankDetailHelper.AddDetailRow(rows, "CitizenId", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "BirthDate", getRawDetail, account.AccountType);

        if (!string.IsNullOrWhiteSpace(account.Branch))
            rows.Add(("สาขา", account.Branch));

        if (!string.IsNullOrWhiteSpace(account.AccountNumber))
            rows.Add(("เลขบัญชี", account.AccountNumber));

        return rows;
    }

    private static List<(string Label, string Value)> BuildNormalRows(
    BankAccount account,
    Func<string, string> getRawDetail)
    {
        if (account.AccountType == "GSB นิติ" ||
            account.AccountType == "KKP นิติ")
        {
            return BuildDualApprovalRows(account, getRawDetail);
        }

        if (account.AccountType == "KBANK นิติ")
        {
            return BuildKbankCorporateRows(account, getRawDetail);
        }

        if (account.AccountType == "TTB นิติ")
        {
            return BuildTtbCorporateRows(account, getRawDetail);
        }

        var rows = new List<(string Label, string Value)>();

        foreach (var key in BankDetailHelper.GetDetailOrder(account.AccountType))
        {
            BankDetailHelper.AddDetailRow(rows, key, getRawDetail, account.AccountType);
        }

        if (!string.IsNullOrWhiteSpace(account.Note))
            rows.Add(("หมายเหตุ", account.Note));

        return rows;
    }

    private static List<(string Label, string Value)> BuildBottomRows(
        Func<string, string> getRawDetail)
    {
        var rows = new List<(string Label, string Value)>();

        AddBottomRow(rows, "Address", "ที่อยู่", getRawDetail);
        AddBottomRow(rows, "ReceivedDate", "วันที่รับบัญชี", getRawDetail);
        AddBottomRow(rows, "ReleasedDate", "วันที่ปล่อยใช้งาน", getRawDetail);

        return rows;
    }
    private static List<(string Label, string Value)> BuildDualApprovalRows(
    BankAccount account,
    Func<string, string> getRawDetail)
    {
        var rows = new List<(string Label, string Value)>();

        BankDetailHelper.AddDetailRow(rows, "CompanyNo", getRawDetail, account.AccountType);

        BankDetailHelper.AddDetailRow(rows, "MakerPhone", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "MakerSimExpire", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "MakerEmail", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "MakerEmailPass", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "MakerUser", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "MakerPass", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "MakerCompanyCode", getRawDetail, account.AccountType);

        BankDetailHelper.AddDetailRow(rows, "ApproverPhone", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "ApproverSimExpire", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "ApproverEmail", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "ApproverEmailPass", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "ApproverUser", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "ApproverPass", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "ApproverCompanyCode", getRawDetail, account.AccountType);

        BankDetailHelper.AddDetailRow(rows, "AppCode", getRawDetail, account.AccountType);

        if (!string.IsNullOrWhiteSpace(account.Note))
            rows.Add(("หมายเหตุ", account.Note));

        return rows;
    }
    private static List<(string Label, string Value)> BuildKbankCorporateRows(
    BankAccount account,
    Func<string, string> getRawDetail)
    {
        var rows = new List<(string Label, string Value)>();

        BankDetailHelper.AddDetailRow(rows, "CompanyNo", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "Phone", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "SimExpire", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "BizUser", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "BizPass", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "Email", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "EmailPass", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "AppCode", getRawDetail, account.AccountType);

        if (!string.IsNullOrWhiteSpace(account.Note))
            rows.Add(("หมายเหตุ", account.Note));

        return rows;
    }

    private static List<(string Label, string Value)> BuildTtbCorporateRows(
        BankAccount account,
        Func<string, string> getRawDetail)
    {
        var rows = new List<(string Label, string Value)>();

        BankDetailHelper.AddDetailRow(rows, "CompanyNo", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "TokenPin", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "CompanyCode", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "BizUser", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "BizPass", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "Email", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "EmailPass", getRawDetail, account.AccountType);
        BankDetailHelper.AddDetailRow(rows, "AppCode", getRawDetail, account.AccountType);

        if (!string.IsNullOrWhiteSpace(account.Note))
            rows.Add(("หมายเหตุ", account.Note));

        return rows;
    }
}