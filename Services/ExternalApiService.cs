using DataVipWeb.Models;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DataVipWeb.Services
{
    public class ExternalApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public class ExternalReportSyncResult
        {
            public List<ExternalBankReport> Reports { get; set; } = new();
            public List<ExternalReportSummary> Summaries { get; set; } = new();
        }

        public async Task<ExternalReportSyncResult> GetBmReportsAsync(
            ExternalSite site,
            DateTime reportDate)
        {
            if (string.IsNullOrWhiteSpace(site.ApiUrl))
                throw new Exception("ยังไม่ได้ตั้งค่า API URL");

            if (string.IsNullOrWhiteSpace(site.BearerToken))
                throw new Exception("ยังไม่ได้ใส่ Bearer Token");

            var apiDate = reportDate.Year > 2500
                ? reportDate.AddYears(-543)
                : reportDate;

            var startDate = apiDate.Date.ToString(
                "yyyy-MM-dd 00:00:00",
                CultureInfo.InvariantCulture);

            var endDate = apiDate.Date.ToString(
                "yyyy-MM-dd 23:59:59",
                CultureInfo.InvariantCulture);

            var url =
                $"{site.ApiUrl}?start_date={Uri.EscapeDataString(startDate)}&end_date={Uri.EscapeDataString(endDate)}";

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", site.BearerToken);

            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("th");
            client.DefaultRequestHeaders.Add("languagecode", "th");
            client.DefaultRequestHeaders.Add("origin", "https://manage.boboonme.com");
            client.DefaultRequestHeaders.Referrer = new Uri("https://manage.boboonme.com/");

            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"ดึงข้อมูลไม่สำเร็จ: {(int)response.StatusCode} {json}");

            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            var reports = new List<ExternalBankReport>();

            AddItems(root, "auto_deposit", "การฝากเงินอัตโนมัติ");
            AddItems(root, "slip_deposit", "ฝากด้วยสลิป");
            AddItems(root, "auto_slip_deposit", "ฝากด้วยสลิปอัตโนมัติ");
            AddItems(root, "truewallet_deposit", "ฝากด้วยทรูวอลเล็ท");
            AddItems(root, "gateway_deposit", "เกตเวย์");

            AddItems(root, "withdraw", "รายการถอน");
            AddItems(root, "auto_withdraw", "การถอนอัตโนมัติ");
            AddItems(root, "gateway_withdraw", "ถอนเกตเวย์");

            var summaries = new List<ExternalReportSummary>();

            AddSummary(root, "summary_auto_deposit", "การฝากเงินอัตโนมัติ");
            AddSummary(root, "summary_slip_deposit", "ฝากด้วยสลิป");
            AddSummary(root, "summary_auto_slip_deposit", "ฝากด้วยสลิปอัตโนมัติ");
            AddSummary(root, "summary_truewallet_deposit", "ฝากด้วยทรูวอลเล็ท");
            AddSummary(root, "summary_gateway_deposit", "เกตเวย์");

            AddSummary(root, "summary_withdraw", "รายการถอน");
            AddSummary(root, "summary_auto_withdraw", "การถอนอัตโนมัติ");
            AddSummary(root, "summary_gateway_withdraw", "ถอนเกตเวย์");

            return new ExternalReportSyncResult
            {
                Reports = reports,
                Summaries = summaries
            };

            void AddItems(JsonElement jsonRoot, string key, string groupName)
            {
                if (!jsonRoot.TryGetProperty(key, out var element))
                    return;

                if (element.ValueKind != JsonValueKind.Array)
                    return;

                var isWithdraw = key.Contains("withdraw");
   

                foreach (var item in element.EnumerateArray())
                {
                    reports.Add(new ExternalBankReport
                    {
                        SiteId = site.Id,
                        ReportDate = apiDate.Date,
                        GroupName = groupName,

                        BankName = isWithdraw
                            ? GetText(item,
                                "settlement_bank_display",
                                "withdraw_bank_display",
                                "withdraw_bank_name",
                                "bank_name_display",
                                "bank_name",
                                "payment_bank_display")
                            : GetText(item,
                                "payment_bank_display",
                                "bank_name_display",
                                "bank_name"),

                        AccountName = isWithdraw
                            ? GetText(item,
                                "settlement_acc_name",
                                "withdraw_name",
                                "withdraw_account_name",
                                "account_name",
                                "name",
                                "payment_name")
                            : GetText(item,
                                "payment_name",
                                "account_name",
                                "name"),

                        AccountNo = isWithdraw
                            ? GetText(item,
                                "settlement_bank_no",
                                "withdraw_account_no",
                                "account_no",
                                "bank_account_no",
                                "payment_account_no")
                            : GetText(item,
                                "payment_account_no",
                                "account_no",
                                "bank_account_no"),

                        TransactionCount = isWithdraw
                            ? GetInt(item, "count_withdraw", "count", "count_deposit")
                            : GetInt(item, "count_deposit", "count"),

                        TotalAmount = isWithdraw
                            ? GetDecimal(item, "total_withdraw", "total", "total_deposit")
                            : GetDecimal(item, "total_deposit", "total"),

                        IsActive = true,
                        CreatedAt = DateTime.Now
                    });
                }
            }

            void AddSummary(JsonElement jsonRoot, string key, string groupName)
            {
                if (!jsonRoot.TryGetProperty(key, out var element))
                    return;

                if (element.ValueKind != JsonValueKind.Object)
                    return;

                summaries.Add(new ExternalReportSummary
                {
                    SiteId = site.Id,
                    ReportDate = apiDate.Date,
                    GroupName = groupName,

                    SummaryCount = GetInt(element,
                        "count_deposit",
                        "count_withdraw",
                        "count"),

                    SummaryTotal = GetDecimal(element,
                        "total_deposit",
                        "total_withdraw",
                        "total"),

                    CreatedAt = DateTime.Now
                });
            }

            string GetText(JsonElement item, params string[] names)
            {
                foreach (var name in names)
                {
                    if (item.TryGetProperty(name, out var value) &&
                        value.ValueKind != JsonValueKind.Null &&
                        value.ValueKind != JsonValueKind.Undefined)
                    {
                        return value.ToString();
                    }
                }

                return "";
            }

            int GetInt(JsonElement item, params string[] names)
            {
                foreach (var name in names)
                {
                    if (!item.TryGetProperty(name, out var value))
                        continue;

                    if (value.ValueKind == JsonValueKind.Number &&
                        value.TryGetInt32(out var number))
                        return number;

                    if (int.TryParse(value.ToString(), out var parsed))
                        return parsed;
                }

                return 0;
            }

            decimal GetDecimal(JsonElement item, params string[] names)
            {
                foreach (var name in names)
                {
                    if (!item.TryGetProperty(name, out var value))
                        continue;

                    if (value.ValueKind == JsonValueKind.Number &&
                        value.TryGetDecimal(out var number))
                        return number;

                    if (decimal.TryParse(value.ToString(), out var parsed))
                        return parsed;
                }

                return 0;
            }
        }
    }
}