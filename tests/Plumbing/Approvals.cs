using Newtonsoft.Json;

namespace tests.Plumbing
{
    public static class Approvals
    {
        public static void VerifyAsJson(this object input)
        {
            var json = JsonConvert.SerializeObject(input);
            ApprovalTests.Approvals.VerifyJson(json);
        }
    }
}